using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KheetoNetworkBot.LIB
{
    public class PollMaker
    {
        CommandContext command;
        TimeSpan duration;
        DiscordEmoji[] emojis;

        public PollMaker(CommandContext command, TimeSpan duration, DiscordEmoji[] emojis)
        {
            this.command = command;
            this.duration = duration;
            this.emojis = emojis;
        }

        public async Task RunAsync() {
            InteractivityExtension interactivity = command.Client.GetInteractivity();

            DiscordEmbedBuilder descrizioneEmbed = new DiscordEmbedBuilder
            {
                Color = new DiscordColor("#FF0000"),
                Description = "Ora scrivi la descrizione del sondaggio",
            };

            DiscordMessage sentDescrizioneEmbed = await command.Channel.SendMessageAsync(descrizioneEmbed).ConfigureAwait(false);

            var descrizione = await interactivity.WaitForMessageAsync(x => x.Channel == command.Channel && x.Author == command.User);

            if(descrizione.TimedOut)
            {
                await command.Message.RespondAsync("Non hai scritto niente in 60 secondi, comando cancellato.").ConfigureAwait(false);
                await sentDescrizioneEmbed.DeleteAsync();
                return;
            } else
            {
                await command.Message.DeleteAsync().ConfigureAwait(false);
                await sentDescrizioneEmbed.DeleteAsync().ConfigureAwait(false);
                await descrizione.Result.DeleteAsync().ConfigureAwait(false);
            }

            DiscordEmbedBuilder.EmbedFooter footer = new DiscordEmbedBuilder.EmbedFooter
            {
                Text = "Kheeto Network - Sondaggi",
                IconUrl = "https://cdn.discordapp.com/icons/654083852838502413/9a50cb8c9806ea2c26e36a0ea0bb76ec.webp?size=128",
            };

            DiscordEmbedBuilder embed = new DiscordEmbedBuilder
            {
                Color = new DiscordColor("#FF0000"),
                Title = "Sondaggio",
                Description = descrizione.Result.Content,
                Footer = footer,
            };

            DiscordMessage sentEmbed = await command.Channel.SendMessageAsync(embed).ConfigureAwait(false);

            foreach(DiscordEmoji emoji in emojis)
            {
                await sentEmbed.CreateReactionAsync(emoji);
            }
        }

    }
}
