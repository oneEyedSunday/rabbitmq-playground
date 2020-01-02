using System;
using System.Linq;
using RabbitMQ.Client;
using System.Text;

namespace EventLogDirect
{
    class EventLogDirect
    {
        static void Main(string[] args)
        {
            ConnectionFactory factory = new ConnectionFactory() { HostName = "localhost" };
            using IConnection connection = factory.CreateConnection();
            using IModel channel = connection.CreateModel();

            const string excName = "direct_logs";

            channel.ExchangeDeclare(exchange: excName,
                                    type: ExchangeType.Direct);


            string severity = (args[0] ??= "info");

            string message = (args.Length > 1)
                          ? string.Join(" ", args.Skip(1).ToArray())
                          : "Hello World!";

            byte[] body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: excName,
                                 routingKey: severity,
                                 basicProperties: null,
                                 body: body);
            Console.WriteLine(" [x] Sent '{0}':'{1}'", severity, message);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
