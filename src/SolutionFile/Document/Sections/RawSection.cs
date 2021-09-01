using System.Collections.Generic;

namespace SolutionFile.Document.Sections
{
    public class RawSection : IDocumentSection
    {
        private readonly IEnumerable<string> _lines;

        public RawSection(IEnumerable<string> lines)
        {
            _lines = lines;
        }

        public RawSection()
        {
            _lines = new List<string> { "" };
        }

        public IEnumerable<string> ToRawLines()
        {
            return _lines;
        }
    }
}
