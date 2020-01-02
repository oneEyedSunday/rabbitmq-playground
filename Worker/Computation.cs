using System;
using System.Threading;

namespace Worker
{
    public class Computation
    {

        private Computation(int dots)
        {
            int cmpSwitch = new Random().Next(1, 3);

            switch (cmpSwitch)
            {
                case 1:
                    FindPrimeNumber(dots * new Random().Next(1000, 10001));
                    break;
                case 2:
                    threadSleep(dots);
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
