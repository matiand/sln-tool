using CommandLine;
using SolutionFile.Document;

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
            // document.
        }
    }
}
