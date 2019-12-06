using System;
using RabbitMQ.Client;
using System.Text;

namespace Send
{
    class Send
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            using(var connection = factory.CreateConnection())
            { // collapsable
                using (var channel = connection.CreateModel())
                {
                    // channel.QueueDeclare("hello", false, false, false, null);
                    // alternatively with named func args
                    channel.QueueDeclare(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null);

                    string message = "Hello from OneEyedSunday!!!";

                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "", routingKey: "hello",
                        basicProperties: null,
                        body: body);

                    Console.WriteLine(" [x] Sent {0} ", message);
                }
            }

            Console.WriteLine(" Press [enter] to exit. ");

            Console.ReadLine();
        }
    }
}
