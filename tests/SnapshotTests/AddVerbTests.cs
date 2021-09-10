using System.Linq;
using Cli.CommandLineVerbs;
using SolutionFile.Document.Sections;
using Xunit;

namespace SnapshotTests
{
    public class AddVerbTests
    {
        [Theory]
        [InlineData("example-one")]
        [InlineData("example-two")]
        public void AddsFilesToGivenSolutionFolder(string fileName)
        {
            var doc = Helpers.LoadSolutionFile($"./input/{fileName}");
            var addVerb = new AddVerb
            {
                SolutionFolder = "Solution Items",
                FilesToAdd = new[] { "file-one", "file-two", "file-three" },
            };

            addVerb.Run(doc);
            doc.SaveToFile("./out-AddVerbTests");

            Helpers.AssertFilesAreEqual("./out-AddVerbTests", $"./snapshots/{fileName}-adding-files");
        }

        [Fact]
        public void IfSolutionFolderDoesNotExist_CreatesIt()
        {
            var doc = Helpers.LoadSolutionFile("./input/example-one");
            var addVerb = new AddVerb
            {
                SolutionFolder = "Test Folder",
                FilesToAdd = new[] { "file-one" },
            };

            addVerb.Run(doc);

            Assert.True(doc.Sections.Any(s => s is ProjectBodyHeader { Name: "Test Folder" }),
                "New solution folder was not found");
        }

        [Fact]
        public void AddingFilesIsIdempotent()
        {
            var doc = Helpers.LoadSolutionFile("./input/example-one");
            var addVerb = new AddVerb
            {
                SolutionFolder = "Solution Items",
                FilesToAdd = new[] { "file-one", "file-two", "file-three" },
            };

            addVerb.Run(doc);
            addVerb.Run(doc);
            doc.SaveToFile("./out-AddVerbTests");

            Helpers.AssertFilesAreEqual("./out-AddVerbTests", "./snapshots/example-one-adding-files");
        }
    }
}
