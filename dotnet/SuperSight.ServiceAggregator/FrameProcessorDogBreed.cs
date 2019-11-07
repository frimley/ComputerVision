using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace SuperSight.ServiceAggregator
{
    /// <summary>
    /// Connects to RabbitMQ and receives/sends images...
    /// </summary>
    class FrameProcessorDogBreed: IBasicConsumer, IDisposable
    {
        private const int DATA_LIFESPAN_MS = 500;
        /// <summary>
        /// Connection to RabbitMQ
        /// </summary>
        private IConnection _rabbitMQconnection;

        private List<RabbitMQ.JSONDogBreed> _dogBreed = new List<RabbitMQ.JSONDogBreed>();

        public List<RabbitMQ.JSONDogBreed> GetCurrent()
        {
            lock(_dogBreed)
            {
                return _dogBreed.FindAll(i => (Environment.TickCount - i.ReceivedTimeStamp < DATA_LIFESPAN_MS));
            }
        }

        private void AddDogBreed(RabbitMQ.JSONDogBreed ageGender)
        {
            lock (_dogBreed)
            {
                _dogBreed.Add(ageGender);
                // remove - lean up our collection of DogBreeds
                for (int i = _dogBreed.Count - 1; i >= 0; i--)
                {
                    if (Environment.TickCount - _dogBreed[i].ReceivedTimeStamp > DATA_LIFESPAN_MS)
                        _dogBreed.RemoveAt(i);
                }
            }
        }

        private static readonly AutoResetEvent _closing = new AutoResetEvent(false);

        public void Process()
        {
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
                this.Model.BasicConsume(Config.RabbitMQQueueFrameDogBreed, true, this);

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
            Debug.WriteLine(String.Format("Recieved: {0}", Encoding.UTF8.GetString(body)));

            RabbitMQ.JSONDogBreed o = JsonConvert.DeserializeObject<RabbitMQ.JSONDogBreed>(Encoding.UTF8.GetString(body));
            o.ReceivedTimeStamp = Environment.TickCount;
            // keep track of this data
            this.AddDogBreed(o);
        }
        #endregion

        public void Dispose()
        {
            if (_rabbitMQconnection != null)
                _rabbitMQconnection.Dispose();
        }
    }
}
