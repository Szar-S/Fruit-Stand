using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public abstract class Fruit
{
    public string Name;
    public float Value;
    public override string ToString()
    {
        return "";
    }
}
public class Orange : Fruit
{
    public Orange(float v)
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
    public Banana(int v)
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
    public Apple(int v)
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
    public Fruit Fruit;
    public int Quantity;
    public Basket()
    {
        Fruit f;
        Quantity = 0;
    }
    public Basket(Fruit f, int q)
    {
        this.Fruit = f;
        this.Price = f.Value;
        this.Quantity = q;
    }
    private float Price;
    public double TotalPrice;
    public double CalculateTotalPrice()
    {
        TotalPrice = Quantity * Price;
        return TotalPrice;
    }
    override public string ToString()
    {
        return this.Fruit.Name;
    }
}