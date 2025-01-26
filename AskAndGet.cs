using System;

//Asks the user for input and returns the input
public static class AskAndGet
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
        while (!int.TryParse(input, out int value) || input.Replace(" ", "") == "")
        {
            Console.Write(question);
            input = Console.ReadLine();
        }
        result = Convert.ToInt32(input);
        return result;
    }
    public static double Double(string question)
    {
        Console.Write(question);
        string input = Console.ReadLine();
        double result;
        while (!double.TryParse(input, out double value) || input.Replace(" ", "") == "")
        {
            Console.Write(question);
            input = Console.ReadLine();
        }
        result = Convert.ToDouble(input);
        return result;
    }
}
