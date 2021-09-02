using System.Collections.Generic;
using CommandLine;
using SolutionFile.Document;

namespace Cli.CommandLineVerbs
{
    [Verb("remove", HelpText = "Remove files from solution folder")]
    public class RemoveVerb : ISolutionDocumentAction
    {
        [Value(index: 0, Min = 1, MetaName = "files",
            HelpText = "Files to remove. Paths need to be relative to sln file")]
        public IEnumerable<string> FilesToRemove { get; set; }

        [Option(shortName: 'f', "sln-folder",
            HelpText = "Name of solution folder that will store your files",
            Default = "Solution Items")]
        public string SolutionFolder { get; set; }

        [Option(shortName: 'n', "sln-name",
            HelpText = "Name of solution file (if empty will use first .sln file it founds)")]
        public string SolutionFileName { get; set; }

        public void Run(SolutionDocument document)
        {
            foreach (var file in FilesToRemove)
            {
                document.RemoveFileToFolder(SolutionFolder, file);
            }

            document.SaveToFile("new-ver.sln");
        }
    }
}
