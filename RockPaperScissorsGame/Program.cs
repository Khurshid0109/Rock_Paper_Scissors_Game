﻿using System;
using System.Linq;
using System.Security.Cryptography;

namespace RockPaperScissorsGame
{
    class Program
    {
        static bool CheckArgs(string[] args)
        {
            if (args.Length < 3 || args.Length % 2 == 0)
            {
                Console.WriteLine("Invalid number of options: please pass odd number of moves (3 or more).");
                return false;
            }

            if (args.Length != args.Distinct().Count())
            {
                Console.WriteLine("Please try to hold space between options!");
                return false;
            }

            return true;
        }

        static void Main(string[] args)
        {
             if (!CheckArgs(args))
                return;
            Console.WriteLine("Hello there.We will enjoy together! Lets start!");
            var sec = new Security();
            var a = new Table(args);
            var judge = new Judge(args.Length);

            bool gameFinished = false;

            while (!gameFinished)
            {
                var key = sec.GenerateKey();
                var computerMove = RandomNumberGenerator.GetInt32(args.Length);
                var hmac = sec.GenerateHMAC(key, args[computerMove]);

                Console.WriteLine("HMAC: " + hmac);

                Console.WriteLine("Available Moves:");
                for (int i = 0; i < args.Length; i++)
                {
                    Console.WriteLine(i + 1 + " - " + args[i]);
                }
                Console.WriteLine("0 - Exit");
                Console.WriteLine("? - Help");

                Console.Write("Enter your move: ");
                var ans = Console.ReadLine();

                if (ans == "?")
                {
                    a.Print();
                    Console.WriteLine("");
                    continue;
                }

                if (ans == "0")
                {
                    gameFinished = true;
                    Console.WriteLine("Goodby.Have a nice day!");
                    continue;
                }

                int playerMove = 0;

                if (!int.TryParse(ans, out playerMove) || playerMove <= 0 || playerMove > args.Length)
                {
                    Console.WriteLine("");
                    continue;
                }

                Console.WriteLine("Your move: " + args[playerMove - 1]);
                Console.WriteLine("Computer move: " + args[computerMove]);

                judge.MatchResult(computerMove, playerMove);

                Console.WriteLine("HMAC key: " + key +"\n");
             }
        }
    }
}