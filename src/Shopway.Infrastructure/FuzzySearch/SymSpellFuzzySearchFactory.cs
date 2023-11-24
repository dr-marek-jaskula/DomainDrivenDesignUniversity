using Shopway.Application.Abstractions;

namespace Shopway.Infrastructure.FuzzySearch;

public sealed class SymSpellFuzzySearchFactory : IFuzzySearchFactory
{
    private const int NumberOfWordsInEnDictionary = 82834;
    private const int ColumnIndexOrTermInDictionary = 0;
    private const int ColumnIndexOrTermFequencyInDictionary = 1;
    private const int DefaultMaxTermDistance = 2;
    private const int CountOfEqualyValuedWordsInDictionary = 1;
    private const string EnDictionaryFileName = "frequency_dictionary_en_82_765.txt";

    /// <summary>
    /// Create instance for frequency English directory
    /// </summary>
    public IFuzzySearch Create()
    {
        SymSpell symSpell = new(NumberOfWordsInEnDictionary, 2);

        var assembly = AssemblyReference.Assembly;
        var embeddedResourceName = $"{typeof(SymSpellFuzzySearchFactory).Namespace}.{EnDictionaryFileName}";

        using Stream stream = assembly.GetManifestResourceStream(embeddedResourceName)!;

        if (stream is null)
        {
            throw new FileNotFoundException("English frequency dictionary resource not found!");
        }

        symSpell.LoadDictionary(stream, ColumnIndexOrTermInDictionary, ColumnIndexOrTermFequencyInDictionary);
        return new SymSpellFuzzySearch(symSpell);
    }

    /// <summary>
    /// Create instance for corpus aimed by path input
    /// </summary>
    /// <param name="path">Path to corpus</param>
    /// <param name="initialCapacity">Approximated dictionary capacity. The expected number of words in dictionary. 
    /// Specifying an accurate initialCapacity is not essential. 
    /// However, it can help speed up processing by alleviating the need for data restructuring as the size grows.
    /// </param>
    /// <param name="maxDirectoryEditDistance">Maximal word distance</param>
    public IFuzzySearch Create(string path, int initialCapacity, int maxDirectoryEditDistance = DefaultMaxTermDistance)
    {
        SymSpell symSpell = new(initialCapacity, maxDirectoryEditDistance);

        if (symSpell.CreateDictionary(path) is false)
        {
            throw new DirectoryNotFoundException("Corpus dictionary not found!");
        }

        return new SymSpellFuzzySearch(symSpell);
    }

    /// <summary>
    /// Create instance for corpus from input list.
    /// </summary>
    /// <param name="approximateTo">A list of strings that words will approximate to</param>
    /// <param name="maxDirectoryEditDistance">Maximal word distance</param>
    /// <returns>SymSpell instance</returns>
    public IFuzzySearch Create(IList<string> approximateTo, int maxDirectoryEditDistance = DefaultMaxTermDistance)
    {
        int initialCapacity = approximateTo.Count;

        SymSpell symSpell = new(initialCapacity, maxDirectoryEditDistance);

        foreach (string word in approximateTo)
        {
            symSpell.CreateDictionaryEntry(word, CountOfEqualyValuedWordsInDictionary);
        }

        return new SymSpellFuzzySearch(symSpell);
    }
}