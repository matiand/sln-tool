using System;
using System.Collections.Generic;
using System.Text;

namespace SolutionFile.Document.Sections
{
    public class ProjectBodyHeader : IDocumentSection
    {
        public Guid Id { get; set; }
        public Guid ProjectType { get; set; }
        public string Name { get; set; }
        public string RelativePath { get; set; }

        public IEnumerable<string> ToRawLines()
        {
            var strBuilder = new StringBuilder();

            strBuilder.Append($@"Project(""{ProjectType.ToString("B").ToUpper()}"") = ");
            strBuilder.Append($@"""{Name}"", ");
            strBuilder.Append($@"""{RelativePath}"", ");
            strBuilder.Append($@"""{Id.ToString("B").ToUpper()}""");

            return new List<string> { strBuilder.ToString() };
        }
    }
}
