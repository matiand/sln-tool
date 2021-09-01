using System;
using System.Reflection;
using CommandLine;
using SolutionFile.Document;

namespace Cli.CommandLineVerbs
{
    [Verb("examples", HelpText = "Show usage examples")]
    public class ExamplesVerb : ISolutionDocumentAction
    {
        private readonly string _appName =
            Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>()!.Title;

        public void Run(SolutionDocument _)
        {
            Console.WriteLine("Add or remove file to solution");
            Console.WriteLine($"\t{_appName} add .gitignore");
            Console.WriteLine($"\t{_appName} remove .editorconfig --sln-name MySlnFile.sln");
            Console.WriteLine($"\t{_appName} add .dockerignore --sln-folder 'Docker config'\n");

            Console.WriteLine("List solution folders");
            Console.WriteLine($"\t{_appName} list --sln-name MySlnFile.sln\n");

            Console.WriteLine("You can add or remove multiple files");
            Console.WriteLine($"\t{_appName} add .gitignore .editorconfig .gitattributes");
            Console.WriteLine($"\t{_appName} remove .gitignore .editorconfig .gitattributes");
        }
    }
}
