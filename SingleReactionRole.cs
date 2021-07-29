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
        DiscordRole role;

        public SingleReactionRole(CommandContext command, string title, string description, string color, DiscordEmoji emoji, DiscordRole role)
        {
            this.command = command;
            this.title = title;
            this.description = description;
            this.color = color;
            this.emoji = emoji;
            this.role = role;
        }

        public async Task RunAsync()
        {
            await command.Message.DeleteAsync();

            DiscordEmbedBuilder embed = new DiscordEmbedBuilder
            {
                Color = new DiscordColor(color),
                Title = title,
                Description = description,
            };

            DiscordMessage sentEmbed = await command.Channel.SendMessageAsync(embed);

            InteractivityExtension interactivity = command.Client.GetInteractivity();

            await sentEmbed.CreateReactionAsync(emoji);

            var reaction = await interactivity.WaitForReactionAsync(x => x.Message == sentEmbed && x.Emoji == emoji);

            if(reaction.Result.Emoji == emoji)
            {
                await (reaction.Result.User as DiscordMember).GrantRoleAsync(role);
            }
        }
    }
}
