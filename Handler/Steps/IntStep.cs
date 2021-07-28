using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KheetoNetworkBot.LIB.Handler.Steps
{
    public class IntStep : DialogueStepBase
    {
        private IDialogueStep nextStep;
        private readonly int? minValue;
        private readonly int? maxValue;

        public IntStep(string content, IDialogueStep nextStep, int? minValue = null, int? maxValue = null) : base(content)
        {
            this.nextStep = nextStep;
            this.minValue = minValue;
            this.maxValue = maxValue;
        }

        public Action<int> OnValidResult { get; set; } = delegate { };

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

            if(minValue.HasValue)
            {
                embed.AddField("Numero Minimo", minValue.Value.ToString(), true);
            }
            if (maxValue.HasValue)
            {
                embed.AddField("Numero Massimo", maxValue.Value.ToString(), true);
            }

            InteractivityExtension interactivity = client.GetInteractivity();

            while(true)
            {
                DiscordMessage embedNuovo = await channel.SendMessageAsync(embed).ConfigureAwait(false);

                OnMessageAdded(embedNuovo);
                var messageResult = await interactivity.WaitForMessageAsync(x => x.ChannelId == channel.Id && x.Author.Id == user.Id).ConfigureAwait(false);

                OnMessageAdded(messageResult.Result);

                if(!int.TryParse(messageResult.Result.Content, out int inputValue))
                {
                    await TryAgainAsync(channel, "La risposta che mi hai dato non è un numero!").ConfigureAwait(false);
                    continue;
                }

                if(messageResult.Result.Content.ToLower().Equals("k!annulla"))
                {
                    return true;
                }

                if(minValue.HasValue)
                {
                    if(inputValue < minValue.Value)
                    {
                        await TryAgainAsync(channel, "Quel numero è troppo basso!");
                        continue;
                    }
                }

                if (maxValue.HasValue)
                {
                    if (inputValue > maxValue.Value)
                    {
                        await TryAgainAsync(channel, "Quel numero è troppo alto!");
                        continue;
                    }
                }

                OnValidResult(inputValue);
                return false;
            }
        }
    }
}
