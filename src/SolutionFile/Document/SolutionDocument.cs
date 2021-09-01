using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SolutionFile.Document.Sections;

namespace SolutionFile.Document
{
    public class SolutionDocument
    {
        private readonly string _lineEnding;

        private readonly bool _shouldEmitByteOrderMark;

        // private readonly string _byteOrderMark = new(new[] { '\xEF', '\xBB', '\xBF' });

        public SolutionDocument(string lineEnding, bool shouldEmitByteOrderMark)
        {
            _lineEnding = lineEnding;
            _shouldEmitByteOrderMark = shouldEmitByteOrderMark;
        }

        public List<IDocumentSection> Sections { get; } = new();

        public void SaveToFile(string filePath)
        {
            var rawLines = Sections.SelectMany(s => s.ToRawLines());

            var rawDocBuilder = new StringBuilder();
            rawDocBuilder.AppendJoin(_lineEnding, rawLines);
            rawDocBuilder.Append(_lineEnding);

            var encoding = new UTF8Encoding(_shouldEmitByteOrderMark);
            File.WriteAllText(filePath, rawDocBuilder.ToString(), encoding);
        }
    }
}
