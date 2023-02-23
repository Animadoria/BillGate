using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace BillGate.Commands.Fun;

public class RateCommand : BaseCommandModule
{
    [Command]
    public async Task Rate(CommandContext ctx, [RemainingText] string ratee)
    {
        var rnd = new Random(ratee.GetHashCode());

        await ctx.RespondAsync($"I rate {ratee} a {rnd.Next(11)}/10");
    }
}