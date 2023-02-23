using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace BillGate.Commands.Test;

public class TTSCommand : ApplicationCommandModule
{
    [SlashCommand("tts", "Uhhh")]
    public async Task TTS(InteractionContext ctx,
                          [Choice("English US Female", "en_us_001")]
                          [Choice("English US Male 1", "en_us_006")]
                          [Choice("English US Male 2", "en_us_007")]
                          [Choice("English US Male 3", "en_us_009")]
                          [Choice("English US Male 4", "en_us_010")]
                          [Choice("English UK Male 1", "en_uk_001")]
                          [Choice("English UK Male 2", "en_uk_003")]
                          [Choice("English AU Female", "en_au_001")]
                          [Choice("English AU Male", "en_au_002")]
                          [Choice("French Male 1", "fr_001")]
                          [Choice("French Male 2", "fr_002")]
                          [Choice("German Female", "de_001")]
                          [Choice("German Male", "de_002")]
                          [Choice("Spanish Male", "es_002")]
                          [Choice("Spanish MX Male", "es_mx_002")]
                          [Choice("Portuguese BR Female 2", "br_003")]
                          [Choice("Portuguese BR Female 3", "br_004")]
                          [Choice("Portuguese BR Male", "br_005")]
                          [Choice("Indonesian Female", "id_001")]
                          [Choice("Japanese Female 1", "jp_001")]
                          [Choice("Japanese Female 2", "jp_003")]
                          [Choice("Japanese Female 3", "jp_005")]
                          [Choice("Japanese Male", "jp_006")]
                          [Option("voice", "The voice to use", false)]
                          string voice,
                          [Option("content", "Content to speak")]
                          string content)
    {

        await Execute(ctx, voice, content);
    }

    [SlashCommand("tts-singing", "TTS... but singing & character voices")]
    public async Task TTSSinging(InteractionContext ctx,
                                 [Choice("Characters Ghostface (Scream)", "en_us_ghostface")]
                                 [Choice("Characters Chewbacca (Star Wars)", "en_us_chewbacca")]
                                 [Choice("Characters C3PO (Star Wars)", "en_us_c3po")]
                                 [Choice("Characters Stitch (Lilo & Stitch)", "en_us_stitch")]
                                 [Choice("Characters Stormtrooper (Star Wars)", "en_us_stormtrooper")]
                                 [Choice("Characters Rocket (Guardians of the Galaxy)", "en_us_rocket")]
                                 [Choice("Singing Alto", "en_female_f08_salut_damour")]
                                 [Choice("Singing Tenor", "en_male_m03_lobby")]
                                 [Choice("Singing Sunshine Soon", "en_male_m03_sunshine_soon")]
                                 [Choice("Singing Warmy Breeze", "en_female_f08_warmy_breeze")]
                                 [Choice("Singing Glorious", "en_female_ht_f08_glorious")]
                                 [Choice("Singing It Goes Up", "en_male_sing_funny_it_goes_up")]
                                 [Choice("Singing Chipmunk", "en_male_m2_xhxs_m03_silly")]
                                 [Choice("Singing Dramatic", "en_female_ht_f08_wonderful_world")]
                                 [Option("voice", "The voice to use", false)]
                                 string voice,
                                 [Option("content", "Content to speak")]
                                 string content)
    {
        //await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

        await Execute(ctx, voice, content);
    }

    private async Task Execute(InteractionContext ctx, string voice, string content)
    {
        using var http = new HttpClient();

        var resp = await http.PostAsync("https://tiktok-tts.weilnet.workers.dev/api/generation",
                                        new StringContent(JsonSerializer.Serialize(new TTSRequest
                                        {
                                            Voice = voice,
                                            Text = content
                                        }), new MediaTypeHeaderValue("application/json")));

        if (!resp.IsSuccessStatusCode)
        {
            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Whoops failed :("));
            return;
        }

        var tr = await resp.Content.ReadFromJsonAsync<TTSResponse>();

        var bytes = Convert.FromBase64String(tr.Data);
        var ms = new MemoryStream(bytes);

        await ctx.CreateResponseAsync(new DiscordInteractionResponseBuilder()
                                         .AddFile("audio.mp3", ms));
    }
}

public class TTSRequest
{
    [JsonPropertyName("text")] public string Text { get; set; }

    [JsonPropertyName("voice")] public string Voice { get; set; }
}

public class TTSResponse
{
    [JsonPropertyName("success")] public bool Success { get; set; }

    [JsonPropertyName("data")] public string Data { get; set; }

    [JsonPropertyName("error")] public object Error { get; set; }


}
