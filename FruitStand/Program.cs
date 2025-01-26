using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.Json;

namespace FruitStand
{
    internal class Program
    {
        const string FilePath = "FruitStand.json";
        private static void Main(string[] args)
        {
            bool yes = true;
            Console.WriteLine("Welcome to the fruit stand!");
            while (yes)
            {
                //Creates the file if it doesn't exist
                if (!File.Exists(FilePath))
                {
                    File.Open(FilePath, FileMode.OpenOrCreate).Close();
                }
                //Prints all the baskets in the file
                printBaskets(FilePath);
                //Asks the user if they want to add a new basket or get an existing basket
                string choice = AskAndGet.String("Add new basket or get a existing basket(use add or get): ").ToLower();
                while (choice != "add" && choice != "get")
                {
                    choice = AskAndGet.String("Use add or get: ").ToLower();
                }
                Basket basket = new Basket();
                switch (choice)
                {
                    case "add":
                        Fruit f = newFruit();

                        basket = BasketAction.Add(FilePath, f);
                        JsonAction.Save(basket, FilePath);
                        break;
                    case "get":
                        basket = BasketAction.Get(FilePath);
                        break;
                }
                //Prints the current basket
                consolePrint(basket);
                //Asks the user if they want to continue the program
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
            Console.WriteLine("Thank you for visiting!");
        }
        //Json actions
        public class JsonAction
        {
            //for saving the current basket to the file
            public static void Save(Basket basket, string FilePath)
            {
                try
                {
                    string line = File.ReadAllText(FilePath);
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
                    File.WriteAllText(FilePath, json);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            //for loading a basket from the file
            public static Basket Load(string FilePath, int baskNum)
            {
                try
                {
                    string line = File.ReadAllText(FilePath);
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
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return null;
                }
            }
        }
        //Asks the user for the name and price of the fruit and returns the fruit
        static Fruit newFruit()
        {
            string fruit = AskAndGet.String("Which fruit(orange, banana or apple): ").ToLower();
            while (fruit != "orange" && fruit != "banana" && fruit != "apple")
            {
                fruit = AskAndGet.String("orange, banana, apple: ");
            }
            double price = AskAndGet.Double("Price of one fruit: ");

            Fruit f;
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
        //Actions for the basket class
        static public class BasketAction
        {
            static public Basket Get(string FilePath)
            {
                int baskNum = AskAndGet.Int("Which Basket(Use numbers): ");

                if (baskNum - 1 < 0)
                {
                    while (baskNum - 1 < 0)
                    {
                        baskNum = AskAndGet.Int("The number can't be less than one: ");
                    }
                    Basket basket = JsonAction.Load(FilePath, baskNum);
                    return basket;
                }
                else if (JsonAction.Load(FilePath, baskNum) == null)
                {
                    return null;
                }
                else
                {
                    Basket basket = JsonAction.Load(FilePath, baskNum);
                    return basket;
                }
            }
            static public Basket Add(string FilePath, Fruit fruit)
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
        //Prints all the baskets in the file
        static public void printBaskets(string FilePath)
        {
            string line = File.ReadAllText(FilePath);
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
        //Prints the current basket
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
