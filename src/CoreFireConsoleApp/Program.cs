using System;
using CoreFire;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length != 2)
        {
            Console.WriteLine("Please supply <url> <auth> in that order.");
            Console.WriteLine("  CoreFireConsoleApp.exe https://your-db.firebaseio.com/ spqiQHnlwA6uS6Ur8H3ZrJinHbX951DzDySazIA");
            Environment.Exit(1);
        }

        var url = args[0];
        var auth = args[1];

        var client = FireClientBuilder.Create()
            .WithUrl(url)
            .WithAuth(auth)
            .Build();

        Console.WriteLine(client.IsValid());
    }
}
