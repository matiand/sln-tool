using System.Collections.Generic;

namespace SolutionFile.Document.Sections
{
    public class GlobalBodyHeader : IDocumentSection
    {
        public IEnumerable<string> ToRawLines()
        {
            return new List<string> { "Global" };
        }
    }
}
