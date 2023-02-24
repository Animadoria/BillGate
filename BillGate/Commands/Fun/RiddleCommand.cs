using BillGate.APIs.Garbaag;
using BillGate.APIs.Garbaag.Entities;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace BillGate.Commands.Fun;

public class RiddleCommand : BaseCommandModule
{
    [Command]
    [Description("Get a Riddle. Powered by Garbaag™")]
    [Category(Constants.Help.FUN)]
    public async Task Riddle(CommandContext ctx)
    {
        Riddle[]? riddle = await GarbaagAPI.GetRiddle();
        if (riddle == null)
        {
            await ctx.RespondAsync("Couldn't get a riddle from the Garbaag API, please try again later");
            return;
        }

        DiscordEmbedBuilder embed = new()
        {
            Title = riddle[0].Title,
            Description = riddle[0].Question + "\n\n" + Formatter.Spoiler(riddle[0].Answer),
            Color = DiscordColor.Blurple
        };

        await ctx.RespondAsync(embed);
    }
}