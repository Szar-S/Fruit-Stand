using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Json;

namespace FruitStand
{
    internal class Program
    {
        const string filePath = "FruitStand.json";
        static class AskAndGet
        {
            public static string String(string question)
            {
                Console.Write(question);
                return Console.ReadLine();
            }
            public static int Int(string question)
            {
                Console.Write(question);
                string input = Console.ReadLine();
                int result;
                while (!input.All(char.IsDigit) || input.Replace(" ", "") == "")
                {
                    Console.Write(question);
                    input = Console.ReadLine();
                }
                result = Convert.ToInt32(input);
                return result;
            }
        }
        private static void Main(string[] args)
        {
            bool yes = true;
            while (yes)
            {
                File.Open(filePath, FileMode.OpenOrCreate).Close();

                printBaskets(filePath);

                Basket basket = whichAction(filePath);
                
                consolePrint(basket);

                string yn = AskAndGet.String("Do you want to continue? Y/N: ").ToUpper();
                while (yn != "Y" && yn != "N")
                {
                    yn = AskAndGet.String("Do you want to continue? Y/N: ").ToUpper();
                }
                if (yn == "N")
                {
                    yes = false;
                }
            }

        }
        public class JsonAction
        {
            public static void Save(Basket basket, string filePath)
            {
                string line = File.ReadAllText(filePath);
                List<Basket> bask = new List<Basket>();
                if (line == "" || line == "{}" || line == null || line == "[]")
                {
                    bask.Add(basket);
                }
                else
                {
                    bask = JsonSerializer.Deserialize<List<Basket>>(line);
                    bask.Add(basket);
                }
                string json = JsonSerializer.Serialize(bask);
                File.WriteAllText(filePath, json);
            }
            public static Basket Load(string filePath, int baskNum)
            {
                string line = File.ReadAllText(filePath);
                if (line == "" || line == "{}" || line == "[]" || line == null)
                {
                    return null;
                }

                List<Basket> bask = JsonSerializer.Deserialize<List<Basket>>(line);
                if (baskNum > bask.Count)
                {
                    return null;
                }
                return bask[baskNum - 1];
            }
        }
        static Fruit newFruit()
        {
            Fruit f;
            string fruit = AskAndGet.String("Which fruit(orange, banana or apple): ").ToLower();
            while (fruit != "orange" && fruit != "banana" && fruit != "apple")
            {
                fruit = AskAndGet.String("orange, banana, apple: ");
            }

            double price = AskAndGet.Int("Price of one fruit: ");

            switch (fruit)
            {
                case "orange":
                    f = new Orange(price);
                    break;
                case "banana":
                    f = new Banana(price);
                    break;
                case "apple":
                    f = new Apple(price);
                    break;
                default:
                    f = new Fruit();
                    break;
            }
            return f;
        }
        static public Basket whichAction(string filePath)
        {
            string yn = AskAndGet.String("Add new basket or get a existing basket(use add or get): ").ToLower();
            while (yn != "add" && yn != "get")
            {
                yn = AskAndGet.String("Use add or get: ").ToLower();
            }

            Basket basket = new Basket();
            switch (yn)
            {
                case "add":
                    Fruit f = newFruit();

                    basket = BasketAction.Add(filePath, f);
                    JsonAction.Save(basket, filePath);
                    break;
                case "get":
                    basket = BasketAction.Get(filePath);
                    break;
            }

            return basket;
        }
        static public class BasketAction
        {
            static public Basket Get(string filePath)
            {
                int baskNum = AskAndGet.Int("Which Basket(Use numbers): ");

                if (baskNum - 1 < 0)
                {
                    while (baskNum - 1 < 0)
                    {
                        baskNum = AskAndGet.Int("The number can't be less than one: ");
                    }
                    Basket basket = JsonAction.Load(filePath, baskNum);
                    return basket;
                }
                else if (JsonAction.Load(filePath, baskNum) == null)
                {
                    return null;
                }
                else
                {
                    Basket basket = JsonAction.Load(filePath, baskNum);
                    return basket;
                }
            }
            static public Basket Add(string filePath, Fruit fruit)
            {
                int quantity = AskAndGet.Int("How many fruits in the basket: ");
                while (quantity < 1)
                {
                    quantity = AskAndGet.Int("The quantity can't be less than one: ");
                }
                Basket basket = new Basket(fruit, quantity);
                return basket;
            }
        }
        static public void printBaskets(string filePath)
        {
            string line = File.ReadAllText(filePath);
            if (line == "" || line == "{}" || line == "[]" || line == null)
            {
                Console.WriteLine("No baskets found");
                return;
            }
            else
            {
                List<Basket> bask = JsonSerializer.Deserialize<List<Basket>>(line);
                for (int i = 0; i < bask.Count; i++)
                {
                    Console.WriteLine((i + 1) + ": " + bask[i].ToString());
                }
            }
        }
        static public void consolePrint(Basket basket)
        {
            if (basket == null)
            {
                Console.WriteLine("No baskets found");
                return;
            }
            else
            {
                Console.WriteLine(basket.ToString());
                /*
                Console.WriteLine("Fruit: " + basket.Fruit.Name);
                Console.WriteLine("Quantity: " + basket.Quantity);
                Console.WriteLine("Price: " + basket.Fruit.Value);
                Console.WriteLine("Total price of basket: " + basket.TotalPrice.ToString());
                */
            }
        }
    }
}