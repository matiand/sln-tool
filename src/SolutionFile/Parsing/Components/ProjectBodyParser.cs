using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using SolutionFile.Document.Sections;

namespace SolutionFile.Parsing.Components
{
    public class ProjectBodyParser : IParserComponent
    {
        private string _projectName;

        public bool Matches(string line)
        {
            return line.StartsWith("Project(");
        }

        public IEnumerable<IDocumentSection> Parse(string firstLine, StreamReader reader)
        {
            var sections = new List<IDocumentSection> { ParseProjectHeader(firstLine) };

            var nextLine = reader.ReadLine();
            while (nextLine is not null)
            {
                if (nextLine.Contains("ProjectSection(SolutionItems) = preProject"))
                {
                    sections.Add(ParseSolutionItems(reader));
                }
                else if (nextLine == "EndProject")
                {
                    sections.Add(new EndProjectBody());
                    break;
                }
                else
                {
                    sections.Add(ParseRawSection(nextLine, reader));
                }

                nextLine = reader.ReadLine();
            }

            return sections;
        }

        private IDocumentSection ParseProjectHeader(string firstLine)
        {
            const string pattern = @"""{(?<type>.+?)}"".+?""(?<name>.+?)"".+?""(?<path>.+?)"".+""{(?<id>.+?)}""";
            var capturedGroups = Regex.Match(firstLine, pattern).Groups;

            _projectName = capturedGroups["name"].Value;

            return new ProjectBodyHeader
            {
                ProjectType = Guid.Parse(capturedGroups["type"].Value),
                Name = _projectName,
                RelativePath = capturedGroups["path"].Value,
                Id = Guid.Parse(capturedGroups["id"].Value),
            };
        }

        private IDocumentSection ParseRawSection(string firstLine, TextReader reader)
        {
            var lines = new List<string> { firstLine };

            var nextLine = reader.ReadLine();
            while (nextLine is not null)
            {
                lines.Add(nextLine);
                if (nextLine.Contains("EndProjectSection")) break;

                nextLine = reader.ReadLine();
            }

            return new RawSection(lines);
        }

        private IDocumentSection ParseSolutionItems(TextReader reader)
        {
            var solutionItems = new SolutionItems(_projectName);

            var nextLine = reader.ReadLine();
            while (nextLine is not null)
            {
                if (nextLine.Contains("EndProjectSection")) break;

                var chunks = nextLine.Split("=");
                solutionItems.Elements.Add(chunks[0].Trim(), chunks[1].Trim());

                nextLine = reader.ReadLine();
            }

            return solutionItems;
        }
    }
}
