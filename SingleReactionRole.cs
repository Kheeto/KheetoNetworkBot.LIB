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
    public class SingleReactionRole
    {
        CommandContext command;
        string title;
        string description;
        string color;
        DiscordEmoji emoji;

        public SingleReactionRole(CommandContext command, string title, string description, string color, DiscordEmoji emoji)
        {
            this.command = command;
            this.title = title;
            this.description = description;
            this.color = color;
            this.emoji = emoji;
        }

        public async Task RunAsync()
        {

            DiscordEmbedBuilder embed = new DiscordEmbedBuilder
            {
                Color = new DiscordColor(color),
                Title = title,
                Description = description,
            };

            DiscordMessage sentEmbed = await command.Channel.SendMessageAsync(embed);
            await command.Message.DeleteAsync();

            InteractivityExtension interactivity = command.Client.GetInteractivity();

            await sentEmbed.CreateReactionAsync(emoji);

            await interactivity.WaitForReactionAsync(x => x.Message == sentEmbed && x.Emoji == emoji);
        }
    }
}
