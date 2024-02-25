using Shopway.Application.Abstractions;
using Shopway.Domain.Common.DataProcessing;

namespace Shopway.Application.Features;

public sealed class DataTransferObjectResponse : DataTransferObject, IResponse
{
    private DataTransferObjectResponse(DataTransferObject dataTransferObject)
        : base(dataTransferObject)
    {
    }

    public static DataTransferObjectResponse From(DataTransferObject dataTransferObject)
    {
        return new DataTransferObjectResponse(dataTransferObject);
    }
}