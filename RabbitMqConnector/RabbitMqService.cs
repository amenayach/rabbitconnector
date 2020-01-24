using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RabbitMqConnector
{
    public class RabbitMqService
    {
        private readonly IConnection connection;
        private readonly IModel model;
        private const string queueName = "TEST";
        private const string exchangeName = "exname";

        public RabbitMqService(IEnumerable<string> hosts, string username, string password, string vhost, int port)
        {
            var factory = new ConnectionFactory
            {
                UserName = username,
                Password = password,
                VirtualHost = vhost,
                Port = port
            };

            connection = factory.CreateConnection(hosts.ToList());

            model = connection.CreateModel();

            model.ExchangeDeclare(exchangeName, ExchangeType.Direct);
            model.QueueDeclare(queueName, false, false, false, null);
            model.QueueBind(queueName, exchangeName, queueName, null);
        }

        internal void Run()
        {
            //Listening to messages
            var consumer = new EventingBasicConsumer(model);

            Console.WriteLine("Listening to messages...");

            consumer.Received += (ch, ea) =>
            {
                var body = ea.Body;

                Console.WriteLine(Encoding.UTF8.GetString(body));

                model.BasicAck(ea.DeliveryTag, false);
            };

            model.BasicConsume(queueName, false, consumer);

            //Sending test message
            Console.WriteLine("Sending test message...");

            model.BasicPublish(exchangeName, queueName, null, Encoding.UTF8.GetBytes("{\"Msg\": \"Message from RabbitMQ connector\"}"));
        }

        internal void Close()
        {
            model.Dispose();
            connection.Dispose();
        }
    }
}
