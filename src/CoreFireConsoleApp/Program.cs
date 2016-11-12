using System;
using CoreFire;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length != 3)
        {
            Console.WriteLine("Please supply <uri> <auth> <name> in that order.");
            Console.WriteLine("  CoreFireConsoleApp.exe https://your-db.firebaseio.com/ spqiQHnlwA6uS6Ur8H3ZrJinHbX951DzDySazIA YourFirstName");
            Environment.Exit(1);
        }

        var uri = new Uri(args[0]);
        if (uri.Scheme != "https")
        {
            Console.WriteLine("Uri's scheme must be https");
            Environment.Exit(2);
        }

        var auth = args[1];

        var client = FireClientBuilder.Create()
            .WithUri(uri)
            .WithAuth(auth)
            .Build();

        var name = args[2];
        var pushResponse = client.PushSync("/names", name);
        Console.WriteLine(pushResponse/*.AbsolutePath*/);
    }
}
