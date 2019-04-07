using System;
using RabbitMQ.Client;
using System.Text;

namespace RabbitMQ.Helper
{
    public class Sender
    {
        private string _hostname;
        private string _queue;

        public static Sender Create(string hostname, string queue)
        {
            return new Sender(hostname, queue);
        }

        public Sender(string hostname, string queue)
        {
            _hostname = hostname;
            _queue = queue;
        }


        public void SendMessage(string msg)
        {
            var factory = new ConnectionFactory() { HostName = _hostname };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: _queue,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var body = Encoding.UTF8.GetBytes(msg);

                channel.BasicPublish(exchange: "",
                                     routingKey: _queue,
                                     basicProperties: null,
                                     body: body);
            }
        }


    }
}
