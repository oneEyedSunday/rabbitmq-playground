using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading;

namespace Worker
{
    class Worker
    {
        private static void Main(string[] args)
        {
            ConnectionFactory factory = new ConnectionFactory
            {
                HostName = "localhost"
            };

            using IConnection connection = factory.CreateConnection();
            using IModel channel = connection.CreateModel();
            channel.QueueDeclare(queue: "task_queue", durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                byte[] body = ea.Body;
                string message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Received {0}", message);

                Computation.TimeConsuming(message.Split('.').Length - 1);

                Console.WriteLine(" [x] Done");

                channel.BasicAck(deliveryTag:ea.DeliveryTag, multiple:false);
            };

            channel.BasicConsume(queue: "task_queue", autoAck: false, consumer: consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
