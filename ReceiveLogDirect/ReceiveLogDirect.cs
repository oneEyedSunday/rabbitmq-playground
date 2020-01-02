using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ReceiveLogDirect
{
    class Program
    {
        static void Main(string[] args)
        {
            ConnectionFactory factory = new ConnectionFactory() { HostName = "localhost" };
            using IConnection connection = factory.CreateConnection();
            using IModel channel = connection.CreateModel();

            const string excName = "direct_logs";

            channel.ExchangeDeclare(exchange: excName,
                                    type: ExchangeType.Direct);

            string queueName = channel.QueueDeclare().QueueName;

            if (false)
            {
                Console.WriteLine("Environent variables: ");

                foreach (var e in Environment.GetEnvironmentVariables().Keys)
                {
                    Console.WriteLine("{0}: {1}", e.ToString(), Environment.GetEnvironmentVariable(e.ToString()));
                }
            }

            

            if (args.Length < 1)
            {
                Console.Error.WriteLine("Usage: {0} [info] [warning] [error]",
                                        Environment.GetCommandLineArgs()[0]);
                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
                Environment.ExitCode = 1;
                return;
            }


            foreach (var severity in args)
            {
                channel.QueueBind(queue: queueName,
                                  exchange: excName,
                                  routingKey: severity);
            }

            Console.WriteLine(" [*] Waiting for messages.");

            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                byte[] body = ea.Body;
                string message = Encoding.UTF8.GetString(body);
                string routingKey = ea.RoutingKey;
                Console.WriteLine(" [x] Received '{0}':'{1}'",
                                  routingKey, message);
            };
            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();

        }
    }
}
