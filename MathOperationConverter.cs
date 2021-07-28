using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.Entities;
using KheetoNetworkBot.LIB.Enums;

namespace KheetoNetworkBot.LIB
{
    public class MathOperationConverter : IArgumentConverter<MathOperation>
    {
        public Task<Optional<MathOperation>> ConvertAsync(string value, CommandContext command)
        {
            return value switch
            {
                "+" => Task.FromResult(Optional.FromValue(MathOperation.ADD)),
                "-" => Task.FromResult(Optional.FromValue(MathOperation.SUBTRACT)),
                "*" => Task.FromResult(Optional.FromValue(MathOperation.MULTIPLY)),
                "/" => Task.FromResult(Optional.FromValue(MathOperation.DIVIDE)),
                "%" => Task.FromResult(Optional.FromValue(MathOperation.PERCENT)),
                _ => Task.FromResult(Optional.FromValue(MathOperation.ADD))
            };
        }
    }
}
