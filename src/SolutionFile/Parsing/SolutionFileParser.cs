using System;
using System.Collections.Generic;
using System.IO;
using SolutionFile.Document;
using SolutionFile.Parsing.Components;

namespace SolutionFile.Parsing
{
    public class SolutionFileParser : IDisposable
    {
        private readonly IEnumerable<IParserComponent> _parserComponents;
        private readonly StreamReader _reader;

        public SolutionFileParser(string slnAbsPath, IEnumerable<IParserComponent> parserComponents)
        {
            _reader = new StreamReader(File.OpenRead(slnAbsPath));
            _parserComponents = parserComponents;
        }

        public void Dispose()
        {
            _reader?.Dispose();
        }

        public SolutionDocument Parse()
        {
            var document = new SolutionDocument();

            var nextLine = _reader.ReadLine();
            while (nextLine is not null)
            {
                if (nextLine.Trim().Length == 0)
                {
                    nextLine = _reader.ReadLine();
                    continue;
                }

                foreach (var component in _parserComponents)
                {
                    if (component.Matches(nextLine))
                    {
                        var documentSection = component.Parse(nextLine, _reader);
                        document.Sections.AddRange(documentSection);
                        break;
                    }
                }

                nextLine = _reader.ReadLine();
            }

            return document;
        }
    }
}
