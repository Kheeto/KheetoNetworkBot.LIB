using DSharpPlus;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KheetoNetworkBot.LIB.Handler.Steps
{
    public abstract class DialogueStepBase : IDialogueStep
    {
        protected readonly string content;

        public DialogueStepBase(string content)
        {
            this.content = content;
        }

        public Action<DiscordMessage> OnMessageAdded { get; set; } = delegate { };

        public abstract IDialogueStep NextStep { get; }

        public abstract Task<bool> ProcessStep(DiscordClient client, DiscordChannel channel, DiscordUser user);

        protected async Task TryAgainAsync(DiscordChannel channel, string problem)
        {
            DiscordEmbedBuilder.EmbedFooter footer = new DiscordEmbedBuilder.EmbedFooter
            {
                Text = "Kheeto Network - Supporto",
                IconUrl = "https://cdn.discordapp.com/icons/654083852838502413/9a50cb8c9806ea2c26e36a0ea0bb76ec.webp?size=128",
            };

            DiscordEmbedBuilder embed = new DiscordEmbedBuilder
            {
                Title = "C'è stato un Problema!",
                Description = problem + "\n***Tornando nella home del Supporto...***",
                Color = new DiscordColor("#CD0000"),
                Footer = footer,
            };

            DiscordMessage sentEmbed = await channel.SendMessageAsync(embed).ConfigureAwait(false);

            OnMessageAdded(sentEmbed);
        }
    }
}
