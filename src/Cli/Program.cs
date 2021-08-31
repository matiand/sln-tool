using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SolutionFile.Parsing;
using SolutionFile.Parsing.Components;

namespace Cli
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // var absPath = Path.GetFullPath(@"../../../in/eShopOnWeb.sln");
            // var sln = SolutionFile.Parse(absPath);

            var cwd = Directory.GetCurrentDirectory();
            var slnPath = Path.Join(cwd, "../../../../../sln-tool.sln");

            var slnParserComponents = GetAllParserComponents();
            var slnParser = new SolutionFileParser(slnPath, slnParserComponents);

            var document = slnParser.Parse();
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
