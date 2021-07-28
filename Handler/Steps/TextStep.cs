using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KheetoNetworkBot.Handler.Steps
{
    public class TextStep : DialogueStepBase
    {
        private IDialogueStep nextStep;
        private readonly int? minLength;
        private readonly int? maxLength;

        public TextStep(string content, IDialogueStep nextStep, int? minLength = null, int? maxLength = null) : base(content)
        {
            this.nextStep = nextStep;
            this.minLength = minLength;
            this.maxLength = maxLength;
        }

        public Action<string> OnValidResult { get; set; } = delegate { };

        public override IDialogueStep NextStep => nextStep;

        public void SetNextStep(IDialogueStep nextStep)
        {
            this.nextStep = nextStep;
        }

        public override async Task<bool> ProcessStep(DiscordClient client, DiscordChannel channel, DiscordUser user)
        {
            DiscordEmbedBuilder.EmbedFooter footer = new DiscordEmbedBuilder.EmbedFooter
            {
                Text = "Kheeto Network - Supporto",
                IconUrl = "https://cdn.discordapp.com/icons/654083852838502413/9a50cb8c9806ea2c26e36a0ea0bb76ec.webp?size=128",
            };

            DiscordEmbedBuilder embed = new DiscordEmbedBuilder
            {
                Description = content,
                Color = new DiscordColor("#CD0000"),
                Footer = footer,
            };

            embed.AddField("Per cancellare il dialogo", "Scrivi k!annulla");

            if(minLength.HasValue)
            {
                embed.AddField("Lunghezza Minima Risposta", minLength.Value.ToString(), true);
            }
            if (maxLength.HasValue)
            {
                embed.AddField("Lunghezza Massima Risposta", maxLength.Value.ToString(), true);
            }

            InteractivityExtension interactivity = client.GetInteractivity();

            while(true)
            {
                DiscordMessage embedNuovo = await channel.SendMessageAsync(embed).ConfigureAwait(false);

                OnMessageAdded(embedNuovo);
                var messageResult = await interactivity.WaitForMessageAsync(x => x.ChannelId == channel.Id && x.Author.Id == user.Id).ConfigureAwait(false);

                OnMessageAdded(messageResult.Result);

                if(messageResult.Result.Content.ToLower().Equals("k!annulla"))
                {
                    return true;
                }

                if(minLength.HasValue)
                {
                    if(messageResult.Result.Content.Length < minLength.Value)
                    {
                        await TryAgainAsync(channel, "La risposta che mi hai dato è troppo corta!");
                        continue;
                    }
                }

                if (maxLength.HasValue)
                {
                    if (messageResult.Result.Content.Length > maxLength.Value)
                    {
                        await TryAgainAsync(channel, "La risposta che mi hai dato è troppo lunga!");
                        continue;
                    }
                }

                OnValidResult(messageResult.Result.Content);
                return false;
            }
        }
    }
}
