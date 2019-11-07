using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text;
using System.Threading;

namespace SuperSight.ServiceAggregator
{
    /// <summary>
    /// Connects to RabbitMQ and receives/sends images...
    /// </summary>
    class FrameProcessorFrames: IBasicConsumer, IDisposable
    {
        /// <summary>
        /// Connection to RabbitMQ
        /// </summary>
        private IConnection _rabbitMQconnection;

        /// <summary>
        /// Holds a reference to the list of current age/gender detections
        /// </summary>
        private FrameProcessorAgeGender _ageGenderProcessor { get; set; }

        /// <summary>
        /// Holds a reference to the list of current dog breed detections
        /// </summary>
        private FrameProcessorDogBreed _dogBreedProcessor { get; set; }

        /// <summary>
        /// Used to detect a CTRL-C key combination to terminate program execution
        /// </summary>
        private static readonly AutoResetEvent _closing = new AutoResetEvent(false);

        public void Process(FrameProcessorAgeGender ageGenderProcessor, FrameProcessorDogBreed dogBreedProcessor)
        {
            _ageGenderProcessor = ageGenderProcessor;
            _dogBreedProcessor = dogBreedProcessor;
            try
            {
                // Setup key event processing
                Console.CancelKeyPress += new ConsoleCancelEventHandler(OnExit);

                var factory = new ConnectionFactory()
                {
                    HostName = Config.RabbitMQServer,
                    Port = Config.RabbitMQPort,
                    UserName = Config.RabbitMQUser,
                    Password = Config.RabbitMQPassword,
                    VirtualHost = Config.RabbitMQVirtualHost
                };

                Console.WriteLine($"Connecting to {Config.RabbitMQServer}...");
                _rabbitMQconnection = factory.CreateConnection();
                this.Model = _rabbitMQconnection.CreateModel();
                this.Model.BasicConsume(Config.RabbitMQQueueFrameSource, true, this);

                _closing.WaitOne();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not connect to RabbmitMQ: {ex}");
            }
        }

        protected static void OnExit(object sender, ConsoleCancelEventArgs args)
        {
            Console.WriteLine("Exiting");
            _closing.Set();
        }

        //RabbitMQ IBasicConsumer methods...
        #region IBasicConsumerImplementation
        public IModel Model { get; set; }

        public event EventHandler<ConsumerEventArgs> ConsumerCancelled;
        public void HandleBasicCancel(string consumerTag) {}
        public void HandleBasicCancelOk(string consumerTag) {}
        public void HandleBasicConsumeOk(string consumerTag) {}
        public void HandleModelShutdown(object model, ShutdownEventArgs reason) {}

        public void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, byte[] body)
        {
            // convert body payload to BitMap
            Bitmap frame;
            using (var ms = new MemoryStream(body))
                frame = new Bitmap(ms);

            // let's draw something on it..            
            using (Graphics g = Graphics.FromImage(frame))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighSpeed;
                using (Pen pen = new Pen(Color.Yellow))
                {
                    // Render Age and gender
                    List<RabbitMQ.JSONAgeGender> ageGender = _ageGenderProcessor.GetCurrent();
                    if (ageGender != null)
                    {
                        foreach (RabbitMQ.JSONAgeGender item in ageGender)
                        {
                            g.DrawEllipse(pen, item.left, item.top, item.right - item.left, item.right - item.left);

                            PointF textPosition = new PointF(
                                    item.right - (item.right - item.left) / 2 - 15,
                                    item.bottom + 5
                                );

                            g.DrawString(
                                String.Format("{0} (Age: {1})", item.gender.Equals("1") ? "Male" : "Female", item.age),
                                new Font("Tahoma", 10), Brushes.Yellow, textPosition);
                        }
                    }

                    List<RabbitMQ.JSONDogBreed> dogBreeds = _dogBreedProcessor.GetCurrent();
                    if (dogBreeds != null)
                    {
                        foreach (RabbitMQ.JSONDogBreed item in dogBreeds)
                        {
                            g.DrawEllipse(pen, item.left, item.top, item.right - item.left, item.bottom - item.top);

                            PointF textPosition = new PointF(
                                    item.right - (item.right - item.left) / 2 - 15,
                                    item.top
                                );

                            g.DrawString(
                                String.Format("{0}", item.breeds),
                                new Font("Tahoma", 10), Brushes.OrangeRed, textPosition);
                        }
                    }
                }
            }

            // republish this image back to RMQ
            this.Model.BasicPublish(Config.RabbitMQExchangeFrameComplete, String.Empty, null, ImageToBytes(frame));
            frame.Dispose(); // slight hack - dispose of this processed frame

        }
        #endregion

        private byte[] ImageToBytes(Bitmap img)
        {
            using (var stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                return stream.ToArray();
            }
        }

        public void Dispose()
        {
            if (_rabbitMQconnection != null)
                _rabbitMQconnection.Dispose();
        }
    }
}
