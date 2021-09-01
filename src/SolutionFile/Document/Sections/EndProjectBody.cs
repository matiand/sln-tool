using System.Collections.Generic;

namespace SolutionFile.Document.Sections
{
    public class EndProjectBody : IDocumentSection
    {
        public IEnumerable<string> ToRawLines()
        {
            return new List<string> { "EndProject" };
        }
    }
}
