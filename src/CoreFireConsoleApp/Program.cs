using System;
using CoreFire;

class Program
{
    static void Main(string[] args)
    {
        var client = new CoreFire.FireClient(null);
        Console.WriteLine(client.IsValid());
    }
}
