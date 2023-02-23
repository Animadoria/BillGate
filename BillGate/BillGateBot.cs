using System.Reflection;
using System.Text.Json;
using BillGate.Configuration;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.Logging;

namespace BillGate;

public class BillGateBot
{
    public Config Configuration = null!;
    public DiscordClient Discord = null!;
    public CommandsNextExtension Commands = null!;
    public SlashCommandsExtension SlashCommands = null!;

    public async Task StartAsync()
    {
        #region Configuration
        if (!File.Exists(Config.CONFIG_PATH))
        {
            Console.WriteLine("There isn't a config file for Bill Gate, creating one right now");

            await File.WriteAllTextAsync(Config.CONFIG_PATH, JsonSerializer.Serialize(new Config()));

            Environment.Exit(0);
            return;
        }

        this.Configuration = JsonSerializer.Deserialize<Config>(await File.ReadAllTextAsync(Config.CONFIG_PATH))!;
        #endregion

        DiscordConfiguration discordCfg = new()
        {
            Token = this.Configuration.Token,
            //MinimumLogLevel = LogLevel.Debug,
            Intents = DiscordIntents.All
        };

        this.Discord = new DiscordClient(discordCfg);

        CommandsNextConfiguration cmdsCfg = new()
        {
            IgnoreExtraArguments = true,
            StringPrefixes = this.Configuration.Prefixes,
            EnableMentionPrefix = true,
            EnableDefaultHelp = true
        };

        this.Commands = this.Discord.UseCommandsNext(cmdsCfg);
        this.SlashCommands = this.Discord.UseSlashCommands();

        this.Commands.RegisterCommands(Assembly.GetExecutingAssembly());
        this.SlashCommands.RegisterCommands(Assembly.GetExecutingAssembly());


        await this.Discord.ConnectAsync(new DiscordActivity("Windows", ActivityType.Watching));
    }
}