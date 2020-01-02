using System;
using System.Threading;
using NetVips;

namespace Worker
{
    public class Computation
    {

        private Computation(int dots)
        {
            
            int cmpSwitch = new Random().Next(1, 4);

            switch (cmpSwitch)
            {
                case 1:
                    FindPrimeNumber(dots * new Random().Next(1000, 10001));
                    break;
                case 2:
                    threadSleep(dots);
                    break;
                case 3:
                    ResizeImage();
                    break;
                default:
                    FindPrimeNumber(1000);
                    break;
            }

        }

        public static void TimeConsuming(int factor)
        {
            var consumeTime = new Computation(factor);
        }

        public void threadSleep(int dots)
        {
            Thread.Sleep(dots * 1000);
        }

        public void ResizeImage()
        {
            Console.WriteLine("Before resing");
            Image image, attentionImg;
            try
            {
                image = Image.NewFromFile("/Users/ispoa/Projects/RabbitMQ/Worker/assets/original.jpg").ThumbnailImage(300, 300);
                Console.WriteLine(image);
                attentionImg = Image.Thumbnail("/Users/ispoa/Projects/RabbitMQ/Worker/assets/original.jpg", 300, 300, crop: "attention");
                Console.WriteLine(attentionImg);

                try
                {
                    image.WriteToFile("/Users/ispoa/Projects/RabbitMQ/Worker/assets/resize.jpg");
                    attentionImg.WriteToFile("/Users/ispoa/Projects/RabbitMQ/Worker/assets/resize_attention.jpg");

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.GetBaseException().Message);
                }
                finally
                {
                    Console.WriteLine("Post Resixing");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetBaseException().Message);
            }
        }

        // https://stackoverflow.com/a/13001749
        public long FindPrimeNumber(int n)
        {
            int count = 0;
            long a = 2;
            while (count < n)
            {
                long b = 2;
                int prime = 1;// to check if found a prime
                while (b * b <= a)
                {
                    if (a % b == 0)
                    {
                        prime = 0;
                        break;
                    }
                    b++;
                }
                if (prime > 0)
                {
                    count++;
                }
                a++;
            }
            return (--a);
        }
    }
}
