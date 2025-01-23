using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
using System.Text.Json;

namespace FruitStand
{
    internal class Program
    {
        const string filePath = "FruitStand.json";
        public static void SaveJSon(Basket basket, string filePath)
        {
            File.Open(filePath, FileMode.OpenOrCreate).Close();
            string json = JsonSerializer.Serialize(basket);
            File.AppendAllText(filePath, json);
        }
        public static Basket[] LoadFromJson(string filePath)
        {
            Basket[] bask;

            using (StreamReader sr = new StreamReader(filePath))
            {
                bask = JsonSerializer.Deserialize<Basket[]>(sr.ReadToEnd());
            }
            return bask;
        }
        private static void Main(string[] args)
        {
            Basket basket = new Basket();

            basket = whichAction(filePath);

            basket.CalculateTotalPrice();

            consolePrint(basket);

        }
        static string AskAndReceive(string question)
        {
            Console.Write(question);
            return Console.ReadLine();
        }
        static Fruit newFruit()
        {
            Fruit f;
            string fruit = AskAndReceive("orange, banana, apple: ").ToLower();

            string p = AskAndReceive("Price: ");
            while (!p.All(char.IsDigit))
            {
                p = AskAndReceive("Price: ");
            }
            double price = Convert.ToDouble(p);
            while (fruit != "orange" && fruit != "banana" && fruit != "apple")
            {
                fruit = AskAndReceive("orange, banana, apple: ");
            }
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
                    f = new Apple(price);
                    break;
            }
            return f;
        }
        static public Basket whichAction(string filePath)
        {

            string yn = AskAndReceive("Y,N: ").ToUpper();
            while (yn != "Y" && yn != "N")
            {
                yn = AskAndReceive("Y,N: ").ToUpper();
            }
            Basket basket = new Basket();
            switch (yn)
            {
                case "Y":
                    Fruit f = newFruit();

                    basket = newBasket(filePath, f);
                    break;
                case "N":
                    basket = getBasket(filePath);
                    break;

            }
            SaveJSon(basket, filePath);
            return basket;
        }
        static public Basket getBasket(string filePath)
        {

            string bN = AskAndReceive("basketNum: ");
            while (!bN.All(char.IsDigit))
            {
                bN = AskAndReceive("basketNum: ");
            }
            int baskNum = Convert.ToInt32(bN);
            if (baskNum - 1 < 0)
            {
                while (baskNum - 1 < 0)
                {
                    baskNum = Convert.ToInt32(AskAndReceive("BasketNum can't be less than one: "));
                }
                Basket basket = LoadFromJson(filePath)[baskNum - 1];
                return basket;
            }
            else if (File.ReadAllLines(filePath) == null)
            {
                Console.WriteLine("No baskets found");
                return null;
            }
            else if (baskNum > LoadFromJson(filePath).Length)
            {
                while (baskNum > LoadFromJson(filePath).Length)
                {
                    baskNum = Convert.ToInt32(AskAndReceive("BasketNum can't be more than the number of baskets: "));
                }
                Basket basket = LoadFromJson(filePath)[baskNum - 1];
                return basket;

            }
            else
            {
                Basket basket = LoadFromJson(filePath)[baskNum - 1];
                return basket;
            }
        }
        static public Basket newBasket(string filePath, Fruit fruit)
        {
            string q = AskAndReceive("Quantity: ");
            while (!q.All(char.IsDigit))
            {
                q = AskAndReceive("Quantity: ");
            }
            int quantity = Convert.ToInt32(q);

            Basket basket = new Basket(fruit, quantity);
            SaveJSon(basket, filePath);
            return basket;
        }
        static public void printBaskets(string filePath)
        {
            Basket[] baskets = LoadFromJson(filePath);
            for (int i = 0; i < baskets.Length; i++)
            {
                Console.WriteLine(baskets[i].ToString());
            }
        }
        static public void consolePrint(Basket basket)
        {
            Console.WriteLine(basket.ToString());
            Console.WriteLine("Fruit: " + basket.Fruit.Name);
            Console.WriteLine("Quantity: " + basket.Quantity);
            Console.WriteLine("Price: " + basket.Fruit.Value);
            Console.WriteLine("Total price of basket: " + basket.TotalPrice);
        }
    }
}