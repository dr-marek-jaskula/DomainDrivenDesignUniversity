using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shopway.Infrastructure.Outbox;

namespace Shopway.Persistence.Converters.Enums;

public sealed class ExecutionStatusConverter : ValueConverter<ExecutionStatus, string>
{
    public ExecutionStatusConverter() : base(status => status.ToString(), @string => (ExecutionStatus)Enum.Parse(typeof(ExecutionStatus), @string)) { }
}
