using System;
using System.IO;
using System.Linq;
using Cli.CommandLineVerbs;
using SolutionFile.Document;
using SolutionFile.Document.Sections;
using Xunit;

namespace SnapshotTests
{
    public class ListSolutionFoldersVerbTests
    {
        [Theory]
        [InlineData("example-one")]
        [InlineData("example-two")]
        public void PrintsEachSolutionFolderOnNewLine(string fileName)
        {
            var doc = Helpers.LoadSolutionFile($"./input/{fileName}");
            var listVerb = new ListSolutionFoldersVerb();
            using var output = new StringWriter();
            var expectedFolderCount = doc.Sections.Count(s =>
                s is ProjectBodyHeader pbh && pbh.ProjectType == SolutionDocument.FolderTypeId);

            Console.SetOut(output);
            listVerb.Run(doc);
            // Skip last entry that is empty
            var actualFolderList = output.ToString()
                .Split(Environment.NewLine)
                .SkipLast(count: 1)
                .ToList();

            Assert.Equal(expectedFolderCount, actualFolderList.Count);
            Assert.Contains(actualFolderList, item => item == "src");
            Assert.Contains(actualFolderList, item => item == "Solution Items");
        }
    }
}
