namespace BillGate.Configuration;

public class Config
{
    public static string CONFIG_PATH => Directory.GetCurrentDirectory() + "/config.json";

    public string Token { get; set; } = "CHANGEME";

    public string[] Prefixes { get; set; } = { "b!" };
}