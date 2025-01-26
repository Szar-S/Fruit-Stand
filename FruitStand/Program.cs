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
            Console.WriteLine("*************************Welcome to the fruit stand**************************************************************");
            while (yes)
            {
                //Creates the file if it doesn't exist
                if (!File.Exists(FilePath))
                {
                    File.Create(FilePath).Close();
                }
                //Prints all the baskets in the file
                Console.WriteLine("-----------------------------------------------------------------------------------------------------------------");
                printBaskets(FilePath);
                Console.WriteLine("-----------------------------------------------------------------------------------------------------------------");
                //Asks the user if they want to add a new basket or get an existing basket
                Console.WriteLine("");
                Console.WriteLine("*****************************************************************************************************************");
                Console.WriteLine("Would you like to:");
                Console.WriteLine("1- Add a new basket");
                Console.WriteLine("2- Get an existing basket");
                Console.WriteLine("3- Remove a basket");
                string choice = AskAndGet.String("Choose one of them: ").ToLower();
                while (choice != "1" && choice != "2" && choice != "3")
                {
                    choice = AskAndGet.String("Choose one of them: ").ToLower();
                }
                Console.WriteLine("");
                Console.WriteLine("*****************************************************************************************************************");
                Basket basket = new Basket();
                switch (choice)
                {
                    case "1":
                        Fruit f = newFruit();
                        basket = BasketAction.Add(FilePath, f);
                        JsonAction.Save(basket, FilePath);
                        //Prints the current basket
                        Console.WriteLine("-----------------------------------------------------------------------------------------------------------------");
                        consolePrint(basket);
                        Console.WriteLine("-----------------------------------------------------------------------------------------------------------------");
                        break;
                    case "2":
                        basket = BasketAction.Get(FilePath);
                        //Prints the current basket
                        Console.WriteLine("-----------------------------------------------------------------------------------------------------------------");
                        consolePrint(basket);
                        Console.WriteLine("-----------------------------------------------------------------------------------------------------------------");
                        break;
                    case "3":
                        basket = BasketAction.Remove(FilePath);
                        break;
                }

                //Asks the user if they want to continue the program
                Console.WriteLine("");
                string yn = AskAndGet.String("Do you want to continue? Y/N: ").ToUpper();
                while (yn != "Y" && yn != "N")
                {
                    yn = AskAndGet.String("Do you want to continue? Y/N: ").ToUpper();
                }
                if (yn == "N")
                {
                    yes = false;
                    Console.WriteLine("");
                    Console.WriteLine("*************************Thank you for visiting!*****************************************************************");
                }
                else
                {
                    Console.WriteLine("");
                    Console.WriteLine("*****************************************************************************************************************");
                }
            }
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
            //for removing a basket from the file
            public static Basket Remove(string FilePath, int baskNum)
            {
                try
                {
                    string line = File.ReadAllText(FilePath);
                    if (line == "" || line == "{}" || line == "[]" || line == null)
                    {
                        Console.WriteLine("No baskets found");
                        return null;
                    }
                    List<Basket> bask = JsonSerializer.Deserialize<List<Basket>>(line);
                    if (baskNum > bask.Count)
                    {
                        Console.WriteLine("No baskets found");
                        return null;
                    }
                    else
                    {
                        bask.RemoveAt(baskNum - 1);
                        string json = JsonSerializer.Serialize(bask);
                        File.WriteAllText(FilePath, json);
                        Console.WriteLine("Basket removed");
                        return null;

                    }
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
                Basket basket = new Basket();
                if (baskNum - 1 < 0)
                {
                    while (baskNum - 1 < 0)
                    {
                        baskNum = AskAndGet.Int("The number can't be less than one: ");
                    }
                    basket = JsonAction.Load(FilePath, baskNum);
                    return basket;
                }
                else
                {
                    basket = JsonAction.Load(FilePath, baskNum);
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
            static public Basket Remove(string FilePath)
            {
                int baskNum = AskAndGet.Int("Which Basket(Use numbers): ");
                Basket basket = JsonAction.Remove(FilePath, baskNum);
                return basket;
            }
        }
        //Prints all the baskets in the file
        static public void printBaskets(string FilePath)
        {
            Console.WriteLine("Baskets:");
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
