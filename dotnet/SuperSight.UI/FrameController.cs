using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SuperSight
{
    /// <summary>
    /// Sends captured images to our message broker (RabbitMQ) for processing at a limited rate
    /// </summary>
    class FrameController: IDisposable, IBasicConsumer
    {
        /// <summary>
        /// Number of consumer threads working simultaneously to process incoming/outgoing frames
        /// </summary>
        private const int CONSUMER_COUNT = 4;

        /// <summary>
        /// Defining event for completed (augmented) frames
        /// </summary>
        /// <param name="frameComplete"></param>
        public delegate void FrameReturnedHandler(Bitmap frameComplete);
        public event FrameReturnedHandler FrameComplete;

        /// <summary>
        /// How frequently will a frame be sent to RabbitMQ?
        /// </summary>
        public int FrameRateMilliseconds { get; set; }

        /// <summary>
        /// Logger - passed in the constructor of this class
        /// </summary>
        public ILogger Logger { get; set; }

        /// <summary>
        /// When was the last frame sent to RMQ?
        /// </summary>
        private int TimestampLastFrameSentMilliseconds { get; set; } = 0;

        /// <summary>
        /// Depicts whether this controller is running or not
        /// </summary>
        private volatile bool _running;
        public bool Running { get { return _running; } }

        /// <summary>
        /// Used to stop frame processing
        /// </summary>
        private CancellationTokenSource _frameProcessorCancellation;


        private BlockingCollection<Bitmap> _blockingCollectionFrameSource = new BlockingCollection<Bitmap>();

        /// <summary>
        /// Used to process complete frames from RabbitMQ
        /// </summary>
        private BlockingCollection<byte[]> _blockingCollectionFrameComplete = new BlockingCollection<byte[]>();

        /// <summary>
        /// Connection to RabbitMQ
        /// </summary>
        private IConnection _rabbitMQconnection;

        /// <summary>
        /// Image converter to convert to byte[]
        /// </summary>
        private ImageConverter _imgConverter = new ImageConverter();

        public FrameController(int frameRateMilliseconds = 250, ILogger logger = null)
        {
            this.FrameRateMilliseconds = frameRateMilliseconds;
            this.Logger = logger;
        }

        public void Start()
        {
            try
            {
                if (!this.Running)
                {
                    this.Logger.Log("Starting ProcessFrame");

                    _running = true;

                    //Setup cancellation token for parallel tasks
                    _frameProcessorCancellation = new CancellationTokenSource();
                    var cancelToken = _frameProcessorCancellation.Token;

                    // Open single connection to RMQ
                    if (this.OpenRabbitMQConnection())
                    {
                        this.LaunchProcessingThreadPoolSource(cancelToken);
                        this.LaunchProcessingThreadPoolComplete(cancelToken);
                    }
                }
            }
            catch(Exception ex)
            {
                _running = false;
                this.Logger.Log($"Could not start: {ex}");
            }
        }

        /// <summary>
        /// Stops processing frames
        /// </summary>
        public void Stop()
        {
            if (this.Running)
            {
                _running = false;
                if (_frameProcessorCancellation != null && !_frameProcessorCancellation.IsCancellationRequested)
                    _frameProcessorCancellation.Cancel();
                this.CloseRabbitMQConnection();
            }
        }

        /// <summary>
        /// Disposes other disposable members
        /// </summary>
        public void Dispose()
        {
            if (_blockingCollectionFrameSource != null)
                _blockingCollectionFrameSource.Dispose();
            if (_blockingCollectionFrameComplete != null)
                _blockingCollectionFrameComplete.Dispose();
            if (_rabbitMQconnection != null)
                _rabbitMQconnection.Dispose();
        }

        /// <summary>
        /// Sends the frame to our internal blocking queue
        /// </summary>
        /// <param name="frame"></param>
        public void ProcessFrame(Bitmap frame)
        {
            if (this.TimestampLastFrameSentMilliseconds.Equals(0) || (Environment.TickCount - this.TimestampLastFrameSentMilliseconds) > this.FrameRateMilliseconds)
            {
                // Add to our blocking collection to be processed
                _blockingCollectionFrameSource.Add(frame);

                // keep track of when our last frame was sent to RMQ
                this.TimestampLastFrameSentMilliseconds = Environment.TickCount;
            }
        }

        /// <summary>
        /// Opens a connection to RMQ - returns true/false based on success
        /// </summary>
        /// <returns></returns>
        private bool OpenRabbitMQConnection()
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = Config.RabbitMQServer,
                    Port = Config.RabbitMQPort,
                    UserName = Config.RabbitMQUser,
                    Password = Config.RabbitMQPassword,
                    VirtualHost = Config.RabbitMQVirtualHost
                };
                _rabbitMQconnection = factory.CreateConnection();
            }
            catch(Exception ex)
            {
                this.Logger.Log($"Could not connect to RabbmitMQ: {ex}");
                return false;
            }
            return true;
        }


        /// <summary>
        /// Closes connection to RMQ
        /// </summary>
        private void CloseRabbitMQConnection()
        {
            if (_rabbitMQconnection != null && _rabbitMQconnection.IsOpen)
            {
                if (this.Model != null)
                    this.Model.Dispose();
                this.Logger.Log("Stopped ProcessFrame - disconnected from RMQ");
                _rabbitMQconnection.Close();
            }
        }


        /// <summary>
        /// Starts thread pool that sends frames to RMQ
        /// </summary>
        private void LaunchProcessingThreadPoolSource(CancellationToken cancelToken)
        {
            for (int taskCount = 0; taskCount < CONSUMER_COUNT; taskCount++)
            {
                var task = Task.Factory.StartNew(() =>
                {
                    using (var channel = _rabbitMQconnection.CreateModel())
                    {
                        while (_running && !cancelToken.IsCancellationRequested)
                        {
                            try
                            {
                                // take frames from internal queue - blocks thread until a frame is available
                                Bitmap originalBitmap = _blockingCollectionFrameSource.Take(cancelToken);

                                // TODO: Move this scale factor to the GUI.. Scale the frame size sent to RMQ
                                double scaleFactor = Convert.ToDouble(originalBitmap.Width) / 640;
                                Size newBitmapSize = new Size(
                                    Convert.ToInt32(Convert.ToDouble(originalBitmap.Width) / scaleFactor), 
                                    Convert.ToInt32(Convert.ToDouble(originalBitmap.Height) / scaleFactor)
                                );
                                Bitmap scaledBitmap = new Bitmap(originalBitmap, newBitmapSize);

                                // send to the exchange (convert to JSON and then to a byte[]
                                if(_running && !cancelToken.IsCancellationRequested)
                                    channel.BasicPublish(Config.RabbitMQExchangeFrameInput, String.Empty, null, (byte[])_imgConverter.ConvertTo(scaledBitmap, typeof(byte[])));
                                originalBitmap.Dispose(); // slight hack - dispose of this processed frame
                                scaledBitmap.Dispose();
                            }
                            // user likely stopped processing frames
                            catch (OperationCanceledException) { } 
                            catch (AlreadyClosedException) { }
                            catch (Exception ex)
                            {
                                this.Logger.Log($"Error occurred processing frame: {ex}");
                            }
                        }
                        this.Logger.Log("Thread cancelled...");
                    }
                },
                    TaskCreationOptions.LongRunning
                );
            }
        }

        /// <summary>
        /// Starts thread pool that receives frames from RMQ
        /// </summary>
        private void LaunchProcessingThreadPoolComplete(CancellationToken cancelToken)
        {
            // Create the RMQ model to listen to items put on queue
            this.Model = _rabbitMQconnection.CreateModel();
            this.Model.BasicConsume(Config.RabbitMQQueueFrameOutput, true, this);

            // launch a threadpool to process the received frames from RMQ
            for (int taskCount = 0; taskCount < CONSUMER_COUNT; taskCount++)
            {
                    var task = Task.Factory.StartNew(() =>
                    {
                        using (var channel = _rabbitMQconnection.CreateModel())
                        {
                            while (_running && !cancelToken.IsCancellationRequested)
                            {
                                try
                                {
                                    // blocks threads until frame is available
                                    byte[] receivedData = _blockingCollectionFrameComplete.Take(cancelToken);

                                    if (this.FrameComplete != null)
                                        this.FrameComplete(this.ConvertBytesToBitMap(receivedData));
                                }
                                // user likely stopped processing frames
                                catch (OperationCanceledException) {}
                                catch (Exception ex)
                                {
                                    this.Logger.Log($"Error occurred processing frame: {ex}");
                                }
                            }
                            this.Logger.Log("Thread cancelled...");
                        }
                    },
                    TaskCreationOptions.LongRunning
                );
            }
        }

        /// <summary>
        /// Converts a byte[] to a Bitap
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        private Bitmap ConvertBytesToBitMap(byte[] frame)
        {
            using (var ms = new MemoryStream(frame))
                return new Bitmap(ms);
        }

        //RabbitMQ IBasicConsumer methods...
        #region IBasicConsumerImplementation
        public void HandleBasicCancel(string consumerTag) {
            Console.WriteLine($"HandleBasicCancel: {consumerTag}");
        }

        public void HandleBasicCancelOk(string consumerTag) {
            Console.WriteLine($"HandleBasicCancelOk: {consumerTag}");
        }

        public void HandleBasicConsumeOk(string consumerTag) {
            Console.WriteLine($"HandleBasicConsumeOk: {consumerTag}");
        }
        public void HandleModelShutdown(object model, ShutdownEventArgs reason) {
            Console.WriteLine($"HandleModelShutdown: {reason}");
        }
        public event EventHandler<ConsumerEventArgs> ConsumerCancelled;

        public void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, byte[] body)
        {
            // Add processed frame data to blocking queue to be processed above
            if (body != null && body.Length > 0)
                _blockingCollectionFrameComplete.Add(body);
        }

        public IModel Model { get; set; }

        #endregion
    }
}