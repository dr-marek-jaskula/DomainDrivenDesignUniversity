using Shopway.Application.Abstractions;
using Shopway.Domain.Common.DataProcessing;

namespace Shopway.Application.Features;

public sealed class FlatDataTransferObjectResponse : FlatDataTransferObject, IResponse
{
    private FlatDataTransferObjectResponse(FlatDataTransferObject dataTransferObject)
        : base(dataTransferObject)
    {
    }

    public static FlatDataTransferObjectResponse From(FlatDataTransferObject dataTransferObject)
    {
        return new FlatDataTransferObjectResponse(dataTransferObject);
    }
}