using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace BillGate.Commands.Fun;

public class MarkovCommand : BaseCommandModule
{
    [Command]
    public async Task Markov(CommandContext ctx)
    {
        await ctx.RespondAsync("not yet!!!!!!1");
    }
}