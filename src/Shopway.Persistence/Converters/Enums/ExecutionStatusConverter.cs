using Shopway.Infrastructure.Outbox;

namespace Shopway.Persistence.Converters.Enums;

[GenerateEnumConverter(EnumName = nameof(ExecutionStatus), EnumNamespace = ExecutionStatusNamespace)]
public sealed class GenerateExecutionStatusConverter
{
    public const string ExecutionStatusNamespace = "Shopway.Infrastructure.Outbox";
}
