using FastEndpoints;
using Microsoft.AspNetCore.Http;

namespace Shopway.Presentation.FastEndpoints.Users;

public sealed class UsersGroup : Group
{
    private const string Group = "Users";
    private const string Tag = $"{Group}.FastEndpoints";

    public UsersGroup()
    {
        Configure(Group, endpointDefinition =>
        {
            endpointDefinition
                .AllowAnonymous();

            endpointDefinition
                .Description(builder => builder
                    .WithTags(Tag));
        });
    }
}
