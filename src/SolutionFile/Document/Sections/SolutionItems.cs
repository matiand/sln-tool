using System.Collections.Specialized;

namespace SolutionFile.Document.Sections
{
    public class SolutionItems : IDocumentSection
    {
        public OrderedDictionary Elements { get; set; } = new();
    }
}
