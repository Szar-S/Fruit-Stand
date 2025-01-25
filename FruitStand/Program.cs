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
        static string AskAndReceive(string question){
            Console.Write(question);
            return Console.ReadLine();
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

                string yn = AskAndReceive("Do you want to continue? Y/N: ").ToUpper();
                while (yn != "Y" && yn != "N")
                {
                    yn = AskAndReceive("Do you want to continue? Y/N: ").ToUpper();
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
            string fruit = AskAndReceive("Which fruit(orange, banana or apple): ").ToLower();
            while (fruit != "orange" && fruit != "banana" && fruit != "apple")
            {
                fruit = AskAndReceive("orange, banana, apple: ");
            }

            string p = AskAndReceive("Price of one fruit: ");
            while (!p.All(char.IsDigit) || p.Replace(" ","") == "")
            {
                p = AskAndReceive("Price of one fruit: ");
            }
            double price = Convert.ToDouble(p);

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
            string yn = AskAndReceive("Add new basket or get a existing basket(use add or get): ").ToLower();
            while (yn != "add" && yn != "get")
            {
                yn = AskAndReceive("Use add or get: ").ToLower();
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

                string bN = AskAndReceive("Which Basket(Use numbers): ");
                while (!bN.All(char.IsDigit) || bN.Replace(" ", "") == "")
                {
                    bN = AskAndReceive("Which Basket(Use numbers): ");
                }
                int baskNum = Convert.ToInt32(bN);
                if (baskNum - 1 < 0)
                {
                    while (baskNum - 1 < 0)
                    {
                        baskNum = Convert.ToInt32(AskAndReceive("The number can't be less than one: "));
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
                string q = AskAndReceive("How many fruits in the basket: ");
                while (!q.All(char.IsDigit) || q.Replace(" ", "") == "")
                {
                    q = AskAndReceive("How many fruits in the basket: ");
                }
                int quantity = Convert.ToInt32(q);

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