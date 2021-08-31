using System.Collections.Specialized;

namespace SolutionFile.Document.Sections
{
    public class NestedProjects : IDocumentSection
    {
        public OrderedDictionary ParentIdsByChildId { get; set; } = new();
    }
}
