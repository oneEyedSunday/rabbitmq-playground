using System;
using RabbitMQ.Client;
using System.Text;
using System.Linq;

namespace Topic
{
    class EmitLogTopic
    {
        static void Main(string[] args)
        {
            const string exchangeName = "topic_logs"; 
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Topic);
                // 😢 why no !!(args.Length)
                string routingKey = (args.Length > 0) ? args[0] : "anonymous.info";
                string message = (args.Length > 1) ? string.Join(" ", args.Skip(1).ToArray()) : "Something just happened right now!!!";

                byte[] body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: exchangeName, routingKey: routingKey, basicProperties: null, body: body);

                Console.WriteLine(" [x] Sent '{0}':'{1}'", routingKey, message);
            }
        }
    }
}
