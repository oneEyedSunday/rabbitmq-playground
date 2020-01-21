using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace TopicReceive
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                const string exchangeName = "topic_logs";

                channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Topic);
                var queueName = channel.QueueDeclare().QueueName;

                if (args.Length < 1)
                {
                    Console.Error.WriteLine("Usage: {0} [binding_key...]",
                                        Environment.GetCommandLineArgs()[0]);

                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();
                    Environment.ExitCode = 1;
                    return;
                }

                foreach (string bindingKey in args)
                {
                    channel.QueueBind(queue: queueName, exchange: exchangeName,
                                  routingKey: bindingKey);
                }

                Console.WriteLine(" [*] Waiting for messages. To exit press CTRL+C");

                EventingBasicConsumer consumer = new EventingBasicConsumer(channel);

                consumer.Received += (model, ea) =>
                {
                    byte[] body = ea.Body;
                    string message = Encoding.UTF8.GetString(body);
                    string routingKey = ea.RoutingKey;
                    Console.WriteLine(" [x] Received from routing key: message >>> '{0}':'{1}'",routingKey, message);
                };

                channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
