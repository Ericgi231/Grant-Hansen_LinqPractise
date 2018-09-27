using System;
using System.Collections.Generic;
using System.Linq;

namespace Demo_LINQ_Intro
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> listOfNumbers = new List<int>();

            Random myRandom = new Random();

            for (int i = 0; i < 20; i++)
            {
                listOfNumbers.Add(myRandom.Next(1, 100));
            }

            Console.Write("Evens by foreach: ");
            foreach (int n in GetEvenNumbers(listOfNumbers))
            {
                Console.Write(n + " ");
            }
            Console.WriteLine();

            Console.Write("Evens by LINQ:    ");
            foreach (int n in GetEvenNumbersLINQ(listOfNumbers))
            {
                Console.Write(n + " ");
            }
            Console.WriteLine();

            Console.ReadKey();
        }

        public static List<int> GetEvenNumbers(List<int> listOfNumbers)
        {
            List<int> listOfEvens = new List<int>();

            foreach (int n in listOfNumbers)
            {
                if (n % 2 == 0)
                {
                    listOfEvens.Add(n);
                }
            }

            listOfEvens.Sort();

            return listOfEvens;
        }

        public static List<int> GetEvenNumbersLINQ(List<int> listOfNumbers)
        {
            var listOfEvens = (
                from n in listOfNumbers
                where n % 2 == 0
                orderby n
                select n
                )
                .ToList();

            //var listOfEvens = listOfNumbers.Where(n => n % 2 == 0).OrderBy(n => n).ToList();

            return listOfEvens;
        }
    }
}
