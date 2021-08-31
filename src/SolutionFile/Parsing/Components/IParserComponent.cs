using System.Collections.Generic;
using System.IO;
using SolutionFile.Document.Sections;

namespace SolutionFile.Parsing.Components
{
    public interface IParserComponent
    {
        public bool Matches(string line);
        public IEnumerable<IDocumentSection> Parse(string firstLine, StreamReader reader);
    }
}
