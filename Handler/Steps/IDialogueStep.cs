using DSharpPlus;
using DSharpPlus.Entities;
using System;
using System.Threading.Tasks;

namespace KheetoNetworkBot.LIB.Handler.Steps
{
    public interface IDialogueStep
    {
        Action<DiscordMessage> OnMessageAdded { get; set; }
        IDialogueStep NextStep { get; }
        Task<bool> ProcessStep(DiscordClient client, DiscordChannel channel, DiscordUser user);

    }
}
