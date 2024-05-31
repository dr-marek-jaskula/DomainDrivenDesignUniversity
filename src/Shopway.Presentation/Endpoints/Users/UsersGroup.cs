using FastEndpoints;
using Microsoft.AspNetCore.Http;

namespace Shopway.Presentation.Endpoints.Users;

public sealed class UsersGroup : Group
{
    private const string Prefix = "Users";
    private const string Tag = $"{Prefix}.Endpoints";

    public UsersGroup()
    {
        Configure(Prefix, endpointDefinition =>
        {
            endpointDefinition
                .AllowAnonymous();

            endpointDefinition
                .Description(builder => builder
                    .WithTags(Tag));
        });
    }
}
