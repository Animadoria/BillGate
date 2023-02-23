using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace BillGate.Commands.Admin;

public class EvalCommand : BaseCommandModule
{
    public async Task<ScriptState<object>> EvaluateAsync(CommandContext ctx, string code)
    {
        ScriptOptions? sopts = ScriptOptions.Default.WithReferences(
            AppDomain.CurrentDomain.GetAssemblies()
                     .Where(assembly => !assembly.IsDynamic && !string.IsNullOrEmpty(assembly.Location)));

        Eval globals = new Eval
        {
            Context = ctx,
            CommandsNext = ctx.Client.GetCommandsNext(),
            Instance = ctx.Client,
            User = ctx.User,
            Guild = ctx.Guild,
            Channel = ctx.Channel,
            Bot = Program.Bot
        };

        Script<object>? script = CSharpScript.Create(Admin.Eval.DefaultUsings + "\n" + code, sopts, typeof(Eval));
        script.Compile();
        ScriptState<object>? result = await script.RunAsync(globals);
        return result;
    }

    [Command]
    [RequireOwner]
    public async Task Eval(CommandContext ctx, [RemainingText] string code)
    {
        await ctx.TriggerTypingAsync();
        DiscordEmbedBuilder embed = new DiscordEmbedBuilder()
        {
            Title = "Evaluation",
            Color = DiscordColor.Green,
            Footer = new DiscordEmbedBuilder.EmbedFooter
            {
                Text = $"Bill Gate Bot · version {Constants.VERSION}"
            },
            Timestamp = DateTime.Now
        };

        try
        {
            // Check if code string starts with code block
            if (code.Split("\n")[0].StartsWith("```"))
            {
                code = string.Join("\n", code.Split("\n")[1..])[..^3];
            }


            ScriptState<object> result = await this.EvaluateAsync(ctx, code);

            if (result is { ReturnValue: { } } && !string.IsNullOrWhiteSpace(result.ReturnValue.ToString()))
            {
                await ctx.RespondAsync(embed.AddField("Evaluation Result",
                                                      Formatter.BlockCode(result.ReturnValue.ToString(), "csharp")));
            }
            else
                await ctx.RespondAsync(embed.AddField("Evaluation Result",
                                                      Formatter.BlockCode("No result was returned.", "csharp")));
        }
        catch (Exception ex)
        {
            await ctx.RespondAsync(embed.WithColor(DiscordColor.Red).AddField("Error!",
                                                                              Formatter.BlockCode(
                                                                                  ex.GetType() + ": " +
                                                                                  ex.Message, "csharp")));
        }
    }
}

public class Eval
{
    internal static string DefaultUsings => """
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Threading.Tasks;

using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using BillGate;
""";

    public static Dictionary<string, dynamic> DynStore = new();

    public CommandContext Context { get; init; } = null!;

    public CommandsNextExtension CommandsNext { get; init; } = null!;

    public DiscordClient Instance { get; init; } = null!;

    public DiscordUser User { get; init; } = null!;

    public DiscordGuild Guild { get; init; } = null!;

    public DiscordChannel Channel { get; init; } = null!;

    public BillGateBot Bot { get; init; } = null!;
}