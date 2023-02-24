using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Jint;
using Jint.Native;
using Jint.Runtime;

namespace BillGate.Commands.Admin;

public class JSEvalCommand : BaseCommandModule
{
    [Command]
    public async Task JSEval(CommandContext ctx, [RemainingText] string code)
    {
        await ctx.TriggerTypingAsync();
        DiscordEmbedBuilder embed = new()
        {
            Title = "JS Evaluation",
            Color = DiscordColor.Green,
            Footer = new DiscordEmbedBuilder.EmbedFooter
            {
                Text = $"Bill Gate Bot · version {Constants.VERSION}"
            },
            Timestamp = DateTime.Now
        };

        try
        {
            JsValue result = new Engine(cfg => cfg.AllowClr())
                            .SetValue("context", ctx)
                            .SetValue("commandsNext", ctx.Client.GetCommandsNext())
                            .SetValue("instance", ctx.Client)
                            .SetValue("user", ctx.User)
                            .SetValue("guild", ctx.Guild)
                            .SetValue("channel", ctx.Channel)
                            .SetValue("bot", Program.Bot)
                            .Evaluate(code);

            if (result.Type != Types.Undefined && !string.IsNullOrWhiteSpace(result.ToString()))
            {
                await ctx.RespondAsync(embed.AddField("Evaluation Result",
                                                      Formatter.BlockCode(result.ToString(), "js")));
            }
            else
                await ctx.RespondAsync(embed.AddField("Evaluation Result",
                                                      Formatter.BlockCode("No result was returned.", "js")));
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