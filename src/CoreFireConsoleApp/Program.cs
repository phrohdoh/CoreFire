using System;
using CoreFire;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length != 2)
        {
            Console.WriteLine("Please supply <uri> <name> in that order.");
            Console.WriteLine("  CoreFireConsoleApp.exe https://your-db.firebaseio.com/ YourFirstName");
            Environment.Exit(1);
        }

        var uri = new Uri(args[0]);
        if (uri.Scheme != "https")
        {
            Console.WriteLine("Uri's scheme must be https");
            Environment.Exit(2);
        }

        var client = FireClientBuilder.Create()
            .WithUri(uri)
            .Build();

        var name = args[1];
        var pushResponse = client.PushSync("/names", new[] { name, "test" });
        Console.WriteLine(pushResponse);

        var setResponse = client.SetSync("/names", new[] { "test1", "test2" });
        Console.WriteLine(setResponse);

        var getResponse = client.GetSync<string[]>("/names");
        Console.WriteLine(string.Join(", ", getResponse));
    }
}
