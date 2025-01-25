using System;
public class Fruit
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
    }
    public Basket(Fruit f)
    {
        this.Fruit = f;
        this.Quantity = 1;
    }
    public Basket(Fruit f, int q)
    {
        this.Fruit = f;
        this.Quantity = q;
    }
    public double TotalPrice
    {
        get
        {
            return Quantity * Fruit.Value;
        }
    }

    override public string ToString()
    {
        return $"This basket contains {this.Quantity} of {this.Fruit.Name}. The price for one piece is {this.Fruit.Value}, making the total cost {TotalPrice}. ";
    }
}