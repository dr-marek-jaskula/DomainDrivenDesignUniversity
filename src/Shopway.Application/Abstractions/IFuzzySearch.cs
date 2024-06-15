using Shopway.Domain.Common.Results;

namespace Shopway.Application.Abstractions;

public interface IFuzzySearch
{
    Result<string> FindBestSuggestion(string stringToApproximate, int maxEditDistance = 2);
    Result<IList<string>> FindMultipleBestSuggestions(string stringToApproximate, int count, int maxEditDistance = 2);
    Result<string> WordSegmentation(string inputTerm, int maxEditDistance = 0);
}
