// See https://aka.ms/new-console-template for more information

namespace BillGate;

public static class Program
{
    public static BillGateBot Bot = new();

    public static async Task Main(string[] args)
    {
        await Bot.StartAsync();

        // never stahp!
        await Task.Delay(-1);
    }
}