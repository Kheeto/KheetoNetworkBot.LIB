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

        public static async Task CreateMeme(string embedTitle, string memeURL, CommandContext command, bool? useImage = null, bool? anotherURLForImage = null, string? anotherImageURL = null)
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

            if ((bool)useImage) embed.ImageUrl = memeURL;
            else { 
                embed.Url = memeURL; 
                if((bool)anotherURLForImage)
                {
                    embed.ImageUrl = anotherImageURL;
                }
            }

            await command.Message.DeleteAsync();
            await command.Channel.SendMessageAsync(embed);
        }
    }
}
