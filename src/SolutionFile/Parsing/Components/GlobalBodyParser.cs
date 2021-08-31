using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using SolutionFile.Document.Sections;

namespace SolutionFile.Parsing.Components
{
    public class GlobalBodyParser : IParserComponent
    {
        public bool Matches(string line)
        {
            return line == "Global";
        }

        public IEnumerable<IDocumentSection> Parse(string firstLine, StreamReader reader)
        {
            var sections = new List<IDocumentSection> { new GlobalBodyHeader() };

            var nextLine = reader.ReadLine();
            while (nextLine is not null)
            {
                if (nextLine.Contains("GlobalSection(NestedProjects) = preSolution"))
                {
                    sections.Add(ParseNestedProjects(reader));
                }
                else if (nextLine == "EndGlobal")
                {
                    sections.Add(new EndGlobalBody());
                }
                else
                {
                    sections.Add(ParseRawSection(nextLine, reader));
                }

                nextLine = reader.ReadLine();
            }

            return sections;
        }

        private IDocumentSection ParseNestedProjects(TextReader reader)
        {
            var nestedProjects = new NestedProjects();

            var nextLine = reader.ReadLine();
            while (nextLine is not null)
            {
                if (nextLine.Contains("EndGlobalSection")) break;

                var (childId, parentId) = ParseNestedProjectsLine(nextLine);
                nestedProjects.ParentIdsByChildId.Add(childId, parentId);

                nextLine = reader.ReadLine();
            }

            return nestedProjects;
        }

        private (Guid, Guid) ParseNestedProjectsLine(string line)
        {
            const string pattern = @"{(.+?)}.+?{(.+?)}";
            var capturedGroups = Regex.Match(line, pattern).Groups;

            return (Guid.Parse(capturedGroups[groupnum: 1].Value), Guid.Parse(capturedGroups[groupnum: 2].Value));
        }

        private IDocumentSection ParseRawSection(string firstLine, TextReader reader)
        {
            var lines = new List<string> { firstLine };

            var nextLine = reader.ReadLine();
            while (nextLine is not null)
            {
                lines.Add(nextLine);
                if (nextLine.Contains("EndGlobalSection")) break;

                nextLine = reader.ReadLine();
            }

            return new RawSection(lines);
        }
    }
}
