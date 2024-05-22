using Shopway.Application.Features;
using Shopway.Domain.Common.DataProcessing;

namespace Shopway.Application.Utilities;

public static class DataTransferObjectUtilities
{
    public static DataTransferObjectResponse ToResponse(this DataTransferObject dto)
    {
        return new DataTransferObjectResponse(dto);
    }
}
