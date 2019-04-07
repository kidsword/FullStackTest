using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQ.Helper
{
    public class Receiver : IDisposable
    {

        private IConnectionFactory factory;
        private IConnection connection;
        private IModel channel;
        private EventingBasicConsumer consumer;

        public EventHandler<string> MessageReceived { get; set; }

        private string _hostname;
        private string _queue;

        public static Receiver Create(string hostname, string queue)
        {
            return new Receiver(hostname, queue);
        }

        public Receiver(string hostname, string queue)
        {
            _hostname = hostname;
            _queue = queue;
        }

        public Receiver SetCallback(Action<object, string> callback)
        {
            MessageReceived += new EventHandler<string>(callback);
            return this;
        }

        public Receiver Start()
        {
            factory = new ConnectionFactory() { HostName = _hostname };

            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            channel.QueueDeclare(queue: _queue,
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

            consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Received {0}", message);

                MessageReceived?.Invoke(body, message);

            };
            channel.BasicConsume(queue: _queue,
                                 autoAck: true,
                                 consumer: consumer);

            Console.WriteLine($"RabbitMq receiver running");

            return this;
        }
        

        public void Dispose()
        {
            try
            {
                channel?.Dispose();
                channel?.Dispose();
                connection?.Dispose();
            }
            catch (Exception){}
        }
    }
}
