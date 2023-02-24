using BillGate.APIs.Garbaag;
using BillGate.APIs.Garbaag.Entities;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace BillGate.Commands.Fun;

public class DadJokeCommand : BaseCommandModule
{
    [Command]
    [Description("Get a dad joke. Powered by Garbaag™")]
    [Category(Constants.Help.FUN)]
    public async Task DadJoke(CommandContext ctx)
    {
        string[]? joke = await GarbaagAPI.GetDadJoke();
        if (joke == null)
        {
            await ctx.RespondAsync("Couldn't get a dad joke from the Garbaag API, please try again later");
            return;
        }

        DiscordEmbedBuilder embed = new()
        {
            Description = joke[0],
            Color = DiscordColor.Blurple
        };

        await ctx.RespondAsync(embed);
    }
}