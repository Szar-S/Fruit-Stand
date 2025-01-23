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
        }
        public Basket LoadFromJson(string filePath)
        {
            Basket basket;
            using (StreamReader sr = new StreamReader(filePath))
            {
                basket = JsonSerializer.Deserialize<Basket>(sr.ReadLine());
            }
            return basket;
        }
        private static void Main(string[] args)
        {
            Program program = new Program();
            Basket basket = new Basket();
            basket = program.ask(filePath);
            Console.WriteLine("Total price of basket: " + basket.TotalPrice);
        }
        public Basket ask(string filePath)
        {
            Basket basket = new Basket();
            Console.Write("Y,N: ");
            string @ask = Console.ReadLine().ToUpper();
            while (@ask != "Y" && @ask != "N")
            {
                Console.Write("Y,N: ");
                @ask = Console.ReadLine().ToUpper();
            }
            switch (@ask)
            {
                case "Y":
                    Fruit apple = new Apple(35);
                    basket = new Basket(apple, 5);
                    basket.CalculateTotalPrice();
                    break;
                case "N":
                    break;
            }
            SaveJSon(basket, filePath);
            return basket;
        }
    }
}