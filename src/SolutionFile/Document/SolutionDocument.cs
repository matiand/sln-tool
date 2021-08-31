using System.Collections.Generic;
using SolutionFile.Document.Sections;

namespace SolutionFile.Document
{
    public class SolutionDocument
    {
        public List<IDocumentSection> Sections { get; } = new();
    }
}
