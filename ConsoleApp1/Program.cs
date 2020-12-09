using System;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Game(args);
        }
        private static string HmacGenerator(string key)
        {
            StringBuilder hmac = new StringBuilder();
            foreach (byte el in SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(key.ToString())))
            {
                hmac.Append(el.ToString("x2"));
            }
            return hmac.ToString();
        }
        private static string KeyGenerator()
        {
            byte[] rand = new byte[16];
            RandomNumberGenerator.Create().GetBytes(rand);
            StringBuilder key = new StringBuilder();
            foreach (byte el in rand)
            {
                key.Append(el.ToString("x2"));
            }
            return key.ToString();
        }
        private static void Game(string[] args)
        {
            string key;
            string hmac;
            int aiTurn;
            int userTurn = -1;
            if (CheckArgs(args))
            {
                key = KeyGenerator();
                hmac = HmacGenerator(key);
                aiTurn = new Random().Next(0, args.Length);
                while (userTurn == -1)
                {
                    CreatingMenu(args, hmac);
                    userTurn = TakeTurn(args.Length);
                }
                CompareTurns(aiTurn, userTurn, args);
                Console.WriteLine($"HMAC Key: {key}");
            }
        }
        private static int TakeTurn(int max)
        {
            try
            {
                int turn = Convert.ToInt32(Console.ReadLine(), fromBase: 10);
                if (turn > 0 && turn <= max + 1)
                    return turn;
                throw new Exception();
            }
            catch (Exception)
            {
                Console.Clear();
                return -1;
            }
        }
        private static void CreatingMenu(string[] args, string hmac)
        {
            int i = 0;
            Console.WriteLine("HMAC: " + hmac + "\nAvailable moves: ");
            foreach (string el in args)
                Console.WriteLine($"{++i} - {el}");
            Console.Write($"{i + 1} - 'exit' \nEnter your move: ");
        }
        private static bool CheckArgs(string[] args)
        {
            HashSet<string> set = new HashSet<string>();
            foreach (var el in args)
                set.Add(el);
            if (set.Count >= 3 && set.Count % 2 != 0 && set.Count == args.Length)
                return true;
            Console.WriteLine("The number of arguments must be odd, greater than three and there must be no duplicates among them. \nFor example: game.exe 1 2 3 or game.exe 1 2 3 4 5");
            return false;
        }
        private static void CompareTurns(int aiTurn, int userTurn, string[] args)
        {
            string message;
            if (userTurn == args.Length + 1)
                Environment.Exit(0);
            userTurn--;
            Console.WriteLine($"You moved: {args[userTurn]}\nComputer moved: {args[aiTurn]}");
            if (userTurn == aiTurn)
            {
                Console.WriteLine("Draw");
                Environment.Exit(0);
            }
            if (Math.Abs(userTurn - aiTurn) <= args.Length / 2)
                   message = userTurn > aiTurn? "You win" : "Computer wins";
            else message = userTurn > aiTurn ? "Computer wins": "You win";
            Console.WriteLine(message);
        }
    }
}
