namespace Shopway.Application.Abstractions;

public interface IFuzzySearchFactory
{
    abstract IFuzzySearch Create();
    abstract IFuzzySearch Create(string path, int initialCapacity, int maxDirectoryEditDistance = 2);
    abstract IFuzzySearch Create(IList<string> approximateTo, int maxDirectoryEditDistance = 2);
}
