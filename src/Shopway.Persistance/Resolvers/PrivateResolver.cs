using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Reflection;

namespace Shopway.Persistence.Resolvers;

public sealed class PrivateResolver : DefaultContractResolver
{
    protected override JsonProperty CreateProperty(
        MemberInfo member,
        MemberSerialization memberSerialization)
    {
        //We call the base CreateProperty method to get the json property back
        JsonProperty prop = base.CreateProperty(
            member,
            memberSerialization);

        if (!prop.Writable)
        {
            var property = member as PropertyInfo;

            //We get non-public setters
            bool hasPrivateSetter = property?.GetSetMethod(true) is not null;

            //Consider non-public setters if they exist
            prop.Writable = hasPrivateSetter;
        }

        return prop;
    }
}