using System.Collections.Generic;

namespace SolutionFile.Document.Sections
{
    public class EndGlobalBody : IDocumentSection
    {
        public IEnumerable<string> ToRawLines()
        {
            return new List<string> { "EndGlobal" };
        }
    }
}
