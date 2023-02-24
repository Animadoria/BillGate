using System.Net.Http.Json;
using BillGate.APIs.Garbaag.Entities;

namespace BillGate.APIs.Garbaag;

public static class GarbaagAPI
{
    private const string GarbaagEndpoint = "https://garbaag.animadoria.com";

    public static async Task<Riddle[]?> GetRiddle()
    {

        HttpClient httpClient = CreateHttpClient();
        try
        {
            return await httpClient.GetFromJsonAsync<Riddle[]>("riddle");
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            return null;
        }
    }

    public static async Task<string[]?> GetDadJoke()
    {

        HttpClient httpClient = CreateHttpClient();
        try
        {
            return await httpClient.GetFromJsonAsync<string[]>("dadjoke");
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            return null;
        }
    }

    private static HttpClient CreateHttpClient()
    {
        HttpClient httpClient = new()
        {
            BaseAddress = new Uri(GarbaagEndpoint)
        };
        httpClient.DefaultRequestHeaders.Add("X-API-Key", Program.Bot.Configuration.GarbaagAPIKey);

        return httpClient;
    }
}