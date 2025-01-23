using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public abstract class Fruit
{
    public string Name { get; set; }
    public double Value { get; set; }
    public override string ToString()
    {
        return "";
    }
}
public class Orange : Fruit
{
    public Orange(double v)
    {
        this.Name = "Orange";
        this.Value = v;
    }
    public override string ToString()
    {
        return Name;
    }

}
public class Banana : Fruit
{
    public Banana(double v)
    {
        this.Name = "Banana";
        this.Value = v;
    }
    public override string ToString()
    {
        return Name;
    }
}
public class Apple : Fruit
{
    public Apple(double v)
    {
        this.Name = "Apple";
        this.Value = v;
    }
    public override string ToString()
    {
        return Name;
    }
}
[Serializable]

public class Basket
{
    public Fruit Fruit { get; set; }
    public int Quantity { get; set; }
    public Basket()
    {
        Quantity = 0;
    }
    public Basket(Fruit f, int q)
    {
        this.Fruit = f;
        this.Price = f.Value;
        this.Quantity = q;
    }
    private double Price;
    public double TotalPrice;
    public double CalculateTotalPrice()
    {
        TotalPrice = Quantity * Price;
        return TotalPrice;
    }
    override public string ToString()
    {
        return $"This basket has {this.Quantity} of {this.Fruit.Name} and the price of one of them is {this.Price} ";
    }
}