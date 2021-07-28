using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KheetoNetworkBot.LIB
{
    public class MemeMaker
    {
        public string embedTitle;
        public string memeURL;
        public bool useImage;
        public bool anotherURLForImage;
        public string anotherImageURL;
        CommandContext command;

        public MemeMaker(string embedTitle, string memeURL, CommandContext command, bool? useImage = null, bool?anotherURLForImage = null, string? anotherImageURL = null)
        {
            if (useImage == null) useImage = true;
            if (anotherURLForImage == null) anotherURLForImage = false;
            if (anotherURLForImage == true) this.anotherImageURL = anotherImageURL;
            this.embedTitle = embedTitle;
            this.memeURL = memeURL;
            this.command = command;
            this.useImage = (bool)useImage;
            this.anotherURLForImage = (bool)anotherURLForImage;
        }

        public async Task RunAsync()
        {
            DiscordEmbedBuilder.EmbedFooter footer = new DiscordEmbedBuilder.EmbedFooter
            {
                Text = "Meme mandata da " + command.Member.DisplayName,
                IconUrl = command.User.AvatarUrl
            };

            DiscordEmbedBuilder embed = new DiscordEmbedBuilder
            {
                Title = embedTitle,
                Color = new DiscordColor("#CD0000"),
                Footer = footer,
            };

            if (useImage) embed.ImageUrl = memeURL;
            else { 
                embed.Url = memeURL; 
                if(anotherURLForImage)
                {
                    embed.ImageUrl = anotherImageURL;
                }
            }

            await command.Message.DeleteAsync();
            await command.Channel.SendMessageAsync(embed);
        }
    }
}
