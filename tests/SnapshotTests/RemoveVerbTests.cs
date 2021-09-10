using System;
using Cli.CommandLineVerbs;
using Xunit;

namespace SnapshotTests
{
    public class RemoveVerbTests
    {
        [Fact]
        public void RemovesFilesFromGivenSolutionFolder()
        {
            var doc = Helpers.LoadSolutionFile("./input/example-one");
            var removeVerb = new RemoveVerb
            {
                SolutionFolder = "Solution Items",
                FilesToRemove = new[] { "file1", "file2", "file3" },
            };

            removeVerb.Run(doc);
            doc.SaveToFile("./out-RemoveVerbTests");

            Helpers.AssertFilesAreEqual("./out-RemoveVerbTests", "./snapshots/example-one-removing-files");
        }

        [Fact]
        public void IfFileDoesNotExist_ContinueAsNormal()
        {
            var doc = Helpers.LoadSolutionFile("./input/example-two");
            var removeVerb = new RemoveVerb
            {
                SolutionFolder = "Solution Items",
                FilesToRemove = new[] { "file1" },
            };

            removeVerb.Run(doc);
            doc.SaveToFile("./out-RemoveVerbTests");

            Helpers.AssertFilesAreEqual("./out-RemoveVerbTests", "./snapshots/example-two-removing-files");
        }

        [Fact]
        public void IfSolutionFolderDoesNotExist_ThrowsArgumentException()
        {
            var doc = Helpers.LoadSolutionFile("./input/example-one");
            var removeVerb = new RemoveVerb
            {
                SolutionFolder = "Test Folder",
                FilesToRemove = new[] { "file-one" },
            };

            Assert.Throws<ArgumentException>(() => removeVerb.Run(doc));
        }

        [Fact]
        public void RemovingFilesIsIdempotent()
        {
            var doc = Helpers.LoadSolutionFile("./input/example-one");
            var removeVerb = new RemoveVerb
            {
                SolutionFolder = "Solution Items",
                FilesToRemove = new[] { "file1", "file2", "file3" },
            };

            removeVerb.Run(doc);
            removeVerb.Run(doc);
            doc.SaveToFile("./out-RemoveVerbTests");

            Helpers.AssertFilesAreEqual("./out-RemoveVerbTests", "./snapshots/example-one-removing-files");
        }
    }
}
