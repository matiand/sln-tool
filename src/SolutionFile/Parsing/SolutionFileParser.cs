using System;
using System.Collections.Generic;
using System.IO;
using SolutionFile.Document;
using SolutionFile.Document.Sections;
using SolutionFile.Extensions;
using SolutionFile.Parsing.Components;

namespace SolutionFile.Parsing
{
    public class SolutionFileParser : IDisposable
    {
        private readonly SolutionDocument _document;
        private readonly IEnumerable<IParserComponent> _parserComponents;
        private readonly StreamReader _reader;

        public SolutionFileParser(string slnPath, IEnumerable<IParserComponent> parserComponents)
        {
            var file = File.OpenRead(slnPath);
            _document = new SolutionDocument(slnPath, file.GetLineEnding(), file.HasByteOrderMark());
            _reader = new StreamReader(file);

            _parserComponents = parserComponents;
        }

        public void Dispose()
        {
            _reader?.Dispose();
        }

        public SolutionDocument Parse()
        {
            var nextLine = _reader.ReadLine();

            while (nextLine is not null)
            {
                if (nextLine.Trim().Length == 0)
                {
                    _document.Sections.Add(new RawSection());
                    nextLine = _reader.ReadLine();
                    continue;
                }

                foreach (var component in _parserComponents)
                {
                    if (component.Matches(nextLine))
                    {
                        var documentSection = component.Parse(nextLine, _reader);
                        _document.Sections.AddRange(documentSection);
                        break;
                    }
                }

                nextLine = _reader.ReadLine();
            }

            return _document;
        }
    }
}
