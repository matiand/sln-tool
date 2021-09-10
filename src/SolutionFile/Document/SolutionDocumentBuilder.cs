using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SolutionFile.Parsing;
using SolutionFile.Parsing.Components;

namespace SolutionFile.Document
{
    public class SolutionDocumentBuilder
    {
        private string _slnPath;

        public SolutionDocumentBuilder WithFilePattern(string filePattern)
        {
            _slnPath = FindSolutionFile(filePattern);
            return this;
        }

        public SolutionDocumentBuilder WithAbsolutePath(string filePath)
        {
            _slnPath = filePath;
            return this;
        }

        public SolutionDocument Build()
        {
            if (_slnPath is null) throw new InvalidOperationException("Cannot build solution document without slnPath");

            var slnParserComponents = GetAllParserComponents();
            using var slnParser = new SolutionFileParser(_slnPath, slnParserComponents);

            return slnParser.Parse();
        }

        private static string FindSolutionFile(string filePattern)
        {
            // If filePattern is empty we look for a *.sln file in current directory
            if (filePattern is null || filePattern.Length == 0)
            {
                var foundSlnFile = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.sln")
                    .FirstOrDefault();
                if (foundSlnFile is null)
                {
                    Console.WriteLine("Could not find solution file in this directory");
                    Environment.Exit(exitCode: 1);
                }

                return foundSlnFile;
            }

            var fullSlnName = filePattern.EndsWith(".sln") ? filePattern : $"{filePattern}.sln";
            var slnPath = Path.Combine(Directory.GetCurrentDirectory(), fullSlnName);

            if (!File.Exists(slnPath))
            {
                Console.WriteLine("Could not find a solution file with that name in this directory");
                Environment.Exit(exitCode: 1);
            }

            return slnPath;
        }

        private static IEnumerable<IParserComponent> GetAllParserComponents()
        {
            var desiredInterface = typeof(IParserComponent);
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var components = assemblies.SelectMany(a => a.GetTypes())
                .Where(t => desiredInterface.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract);

            return components.Select(type => (IParserComponent)Activator.CreateInstance(type));
        }
    }
}
