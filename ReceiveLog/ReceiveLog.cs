using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ReceiveLog
{
    class Program
    {
        static void Main(string[] args)
        {
            ConnectionFactory factory = new ConnectionFactory() { HostName = "localhost" };
            using IConnection connection = factory.CreateConnection();
            using IModel channel = connection.CreateModel();


            const string excName = "logs";
            channel.ExchangeDeclare(exchange: excName, type: ExchangeType.Fanout);

            string queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queue: queueName,
                              exchange: excName,
                              routingKey: "");

            Console.WriteLine(" [*] Connected & Waiting for logs.");


            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {

                byte[] body = ea.Body;
                string message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] {0} at {1}", message, System.DateTime.Now);
            };


            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
