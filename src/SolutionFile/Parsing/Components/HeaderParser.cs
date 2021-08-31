using System.Collections.Generic;
using System.IO;
using System.Linq;
using SolutionFile.Document.Sections;

namespace SolutionFile.Parsing.Components
{
    public class HeaderParser : IParserComponent
    {
        public bool Matches(string line)
        {
            return line.StartsWith("Microsoft Visual Studio Solution File, Format Version");
        }

        // According to this https://docs.microsoft.com/en-us/visualstudio/extensibility/internals/solution-dot-sln-file?view=vs-2019#file-header
        // Header consists of 4 lines
        public IEnumerable<IDocumentSection> Parse(string firstLine, StreamReader reader)
        {
            // Remember to eager load this collection so reader is called
            var lines = Enumerable.Range(start: 0, count: 3).Select(_ => reader.ReadLine()).Prepend(firstLine).ToList();

            return new List<IDocumentSection> { new RawSection(lines) };
        }
    }
}
