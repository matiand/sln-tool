using System;

namespace SolutionFile.Document.Sections
{
    public class ProjectBodyHeader : IDocumentSection
    {
        public Guid Id { get; set; }
        public Guid ProjectType { get; set; }
        public string Name { get; set; }
        public string RelativePath { get; set; }
    }
}
