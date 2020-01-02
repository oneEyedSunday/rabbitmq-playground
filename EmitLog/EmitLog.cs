using System;
using RabbitMQ.Client;
using System.Text;
using System.Collections.Generic;
using System.Collections;

namespace EmitLog
{
    class EmitLog
    {
        private static readonly List<string> messageBag = new List<string>{
            "Hello And Welcome",
            "Keys open Doors",
            "Hello new World, I'm on my way",
            "Hotel...Trivago"
        };

        static List<string> authors = new List<string> {
            "Key & Peele",
            "Shadman",
            "Piers Morgan",
            "Dietmar Hamman",
            "Giuseppe Verdi"
        };

        private static List<string> MessageBag { get => messageBag; }

        private static string[] allMessages = GenerateMessages(1000);

        private static IEnumerator messsagesEnumerator = null;

        static void Main(string[] args)
        {

            messsagesEnumerator = allMessages.GetEnumerator();
            ConnectionFactory factory = new ConnectionFactory() { HostName = "localhost" };
            using IConnection connection = factory.CreateConnection();
            using IModel channel = connection.CreateModel();

            const string excName = "logs";

            channel.ExchangeDeclare(exchange: excName, type: ExchangeType.Fanout);

            int noMsgs = System.Convert.ToInt32((args.Length > 0) ? args[0] : "100", 10);

            for (int i = 0; i < noMsgs; i++)
            {
                string message = GetMessage();
                byte[] body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: excName, routingKey: "", basicProperties: null, body);

                Console.WriteLine($" [x] Sent {message} at {System.DateTime.Now}");

                System.Threading.Thread.Sleep(5000);
            }

           

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

        private static string GetMessage()
        {
            messsagesEnumerator.MoveNext();
            if (messsagesEnumerator.Current.Equals(null)){
                messsagesEnumerator.Reset();
            }
            return (string)messsagesEnumerator.Current;
        }

        private static string[] GenerateMessages(int msgCount)
        {
            string[] messages = new string[msgCount];

            for (int i = 0; i < msgCount; i++)
            {
                messages[i] = string.Join(" say(s) ", authors[ new Random().Next(authors.Count)], MessageBag[new Random().Next(MessageBag.Count)]);
            }
            return messages;
        }
    }
}
