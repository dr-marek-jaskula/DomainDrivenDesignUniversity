namespace Shopway.App.Options;

public static class OptionsUtilities
{
    public static TOptions GetOptions<TOptions>(this IConfiguration configuration)
        where TOptions : new()
    {
        var configurationValue = new TOptions();
        configuration.GetSectionFor<TOptions>().Bind(configurationValue);
        return configurationValue;
    }

    public static IConfiguration GetSectionFor<TOptions>(this IConfiguration configuration)
    {
        var section = typeof(TOptions).Name;
        return configuration.GetSection(section);
    }
}
