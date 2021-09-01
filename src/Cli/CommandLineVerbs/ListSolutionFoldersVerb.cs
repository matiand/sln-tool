using System;
using CommandLine;
using SolutionFile.Document;
using SolutionFile.Document.Sections;

namespace Cli.CommandLineVerbs
{
    [Verb("list", HelpText = "List solution folders")]
    public class ListSolutionFoldersVerb : ISolutionDocumentAction
    {
        [Option(shortName: 'n', "sln-name",
            HelpText = "Name of solution file (if empty will use first .sln file it founds)")]
        public string SolutionFileName { get; set; }

        public void Run(SolutionDocument document)
        {
            foreach (var section in document.Sections)
            {
                if (section is ProjectBodyHeader project && project.ProjectType == SolutionDocument.FolderTypeId)
                {
                    Console.WriteLine(project.Name);
                }
            }
        }
    }
}
