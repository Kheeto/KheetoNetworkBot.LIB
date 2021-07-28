using DSharpPlus;
using DSharpPlus.Entities;
using KheetoNetworkBot.Handler.Steps;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KheetoNetworkBot.Handler
{
    public class DialogueHandler
    {
        private readonly DiscordClient client;
        private readonly DiscordChannel channel;
        private readonly DiscordUser user;
        private IDialogueStep currentStep;

        public DialogueHandler(
            DiscordClient client,
            DiscordChannel channel,
            DiscordUser user,
            IDialogueStep currentStep
        )
        {
            this.client = client;
            this.channel = channel;
            this.user = user;
            this.currentStep = currentStep;
        }

        private readonly List<DiscordMessage> messages = new List<DiscordMessage>();

        public async Task<bool> ProcessDialogue()
        {
            while(currentStep != null)
            {
                currentStep.OnMessageAdded += (message) => messages.Add(message);

                bool cancelled = await currentStep.ProcessStep(client, channel, user);

                if(cancelled)
                {
                    await DeleteMessagesAsync().ConfigureAwait(false);

                    return false;
                }

                currentStep = currentStep.NextStep;
            }

            await DeleteMessagesAsync().ConfigureAwait(false);

            return true;
        }

        private async Task DeleteMessagesAsync()
        {
            if (channel.IsPrivate) return;

            foreach(DiscordMessage msg in messages)
            {
                await msg.DeleteAsync();
            }
        }
    }
}
