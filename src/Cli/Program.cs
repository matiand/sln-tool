using System;
using Cli.CommandLineVerbs;
using CommandLine;
using SolutionFile.Document;

namespace Cli
{
    internal class Program
    {
        private static readonly Parser CliParser = Parser.Default;

        private static void Main(string[] args)
        {
            try
            {
                HandleInput(args);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failure: {e.Message}");
                Environment.Exit(exitCode: 1);
            }
        }

        private static void HandleInput(string[] args)
        {
            CliParser.ParseArguments<ExamplesVerb, AddVerb, RemoveVerb,
                    ListSolutionFoldersVerb>(args)
                .WithParsed<AddVerb>(verb =>
                {
                    var document = new SolutionDocumentBuilder()
                        .WithFilePattern(verb.SolutionFileName)
                        .Build();
                    verb.Run(document);
                    document.SaveToFile(document.SolutionPath);
                })
                .WithParsed<RemoveVerb>(verb =>
                {
                    var document = new SolutionDocumentBuilder()
                        .WithFilePattern(verb.SolutionFileName)
                        .Build();
                    verb.Run(document);
                    document.SaveToFile(document.SolutionPath);
                })
                .WithParsed<ListSolutionFoldersVerb>(verb =>
                {
                    var document = new SolutionDocumentBuilder()
                        .WithFilePattern(verb.SolutionFileName)
                        .Build();
                    verb.Run(document);
                })
                .WithParsed<ExamplesVerb>(verb => { verb.Run(_: null); })
                .WithNotParsed(_ => { Environment.Exit(exitCode: 1); });
        }
    }
}
