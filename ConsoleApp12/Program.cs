using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Task3
{
    class Program
    {
        private static bool inputCheck(string[] args)
        {
            if (args.Length >= 3 && args.Length % 2 == 1 && args.Distinct().Count() == args.Length)
            {
                return true;
            }
            else if (args.Length < 3)
            {
                Console.WriteLine("Arguments input error. At least 3 arguments");
            }
            else if (args.Length % 2 != 1)
            {
                Console.WriteLine("Arguments input error. Odd number of args only");
            }
            else if (args.Distinct().Count() != args.Length)
            {
                Console.WriteLine("Arguments input error. Unique arguments only");
            }

            return false;
        }

        private static string KeyGenerator(int keyLengthBits)
        {
            int keyLengthBytes = keyLengthBits / 8;
            byte[] bytes = new Byte[keyLengthBytes];

            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(bytes);

            return BitConverter.ToString(bytes).Replace("-", string.Empty);
        }

        private static string HmacGenerator(string key, string text)
        {
            var hash = new HMACSHA256(Encoding.UTF8.GetBytes(key));
            var hmac = hash.ComputeHash(Encoding.UTF8.GetBytes(text));

            return BitConverter.ToString(hmac).Replace("-", string.Empty);
        }

        private static int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        public static void Main(string[] args)
        {
            if (!inputCheck(args))
            {
                Console.WriteLine("Example: rock scissors paper");

                return;
            }

            string key = KeyGenerator(128);
            int computerMove = RandomNumber(1, args.Length + 1);
            string hmac = HmacGenerator(key, args[computerMove - 1]);

            Console.WriteLine($"\nHMAC = {hmac}");
            Console.WriteLine("Available moves:");
            for (int i = 0; i < args.Length; i++)
            {
                Console.WriteLine($"{i + 1} - {args[i]}");
            }
            Console.WriteLine("0 - exit");

            int userMove;
            while (true)
            {
                Console.Write("Enter your move: ");
                string userChoice = Console.ReadLine();

                try
                {
                    int checkUserMove = Convert.ToInt32(userChoice);

                    if (checkUserMove < 0 || checkUserMove > args.Length)
                    {
                        throw new Exception();
                    }

                    if (checkUserMove == 0)
                    {
                        return;
                    }

                    userMove = checkUserMove;
                    break;
                }
                catch
                {
                    Console.WriteLine($"Input error.\nEnter from 0 to {args.Length}");
                }
            }
            Console.WriteLine($"Your move: {args[userMove - 1]}");
            Console.WriteLine($"Computer move: {args[computerMove - 1]}");

            int half = args.Length / 2;
            List<int> movesWhenLose = new List<int>();

            if (computerMove + half > args.Length)
            {
                for (int i = computerMove + 1; i <= args.Length; i++)
                {
                    movesWhenLose.Add(i);
                    half -= 1;
                }
                for (int i = 1; i <= half; i++)
                {
                    movesWhenLose.Add(i);
                }
            }

            else
            {
                for (int i = computerMove + 1; i <= computerMove + half; i++)
                {
                    movesWhenLose.Add(i);
                }
            }

            if (userMove == computerMove)
            {
                Console.WriteLine("Draw.");
            }
            else if (!movesWhenLose.Contains(userMove))
            {
                Console.WriteLine("You Win!");
            }
            else
            {
                Console.WriteLine("You Lose.");
            }
            Console.WriteLine($"HMAC key: {key}");
        }
    }
}
