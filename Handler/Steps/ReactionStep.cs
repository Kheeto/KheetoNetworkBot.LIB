using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using KheetoNetworkBot.Handler.Steps.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KheetoNetworkBot.LIB.Handler.Steps
{
    public class ReactionStep : DialogueStepBase
    {
        private readonly Dictionary<DiscordEmoji, ReactionStepData> options;
        private DiscordEmoji selectedEmoji;

        public ReactionStep(string content, Dictionary<DiscordEmoji, ReactionStepData> options) : base(content)
        {
            this.options = options;
        }

        public override IDialogueStep NextStep => options[selectedEmoji].NextStep;

        public Action<DiscordEmoji> OnValidResult { get; set; } = delegate {

        };

        public override async Task<bool> ProcessStep(DiscordClient client, DiscordChannel channel, DiscordUser user)
        {
            var cancelEmoji = DiscordEmoji.FromName(client, ":x:");

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

            embed.AddField("Per cancellare il dialogo", "Premi sulla reazione della X");

            InteractivityExtension interactivity = client.GetInteractivity();

            while(true)
            {
                DiscordMessage sentEmbed = await channel.SendMessageAsync(embed).ConfigureAwait(false);

                OnMessageAdded(sentEmbed);

                foreach(DiscordEmoji emoji in options.Keys)
                {
                    await sentEmbed.CreateReactionAsync(emoji);
                };

                await sentEmbed.CreateReactionAsync(cancelEmoji);

                var reactionResult = await interactivity.WaitForReactionAsync(
                    x => options.ContainsKey(x.Emoji) || x.Emoji == cancelEmoji
                ).ConfigureAwait(false);

                if(reactionResult.Result.Emoji == cancelEmoji)
                {
                    return true;
                }

                selectedEmoji = reactionResult.Result.Emoji;

                OnValidResult(selectedEmoji);

                return false;
            }
        }
    }

    
}
