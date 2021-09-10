using System.IO;
using System.Linq;
using SolutionFile.Document;
using Xunit;

namespace SnapshotTests
{
    public class Helpers
    {
        public static SolutionDocument LoadSolutionFile(string filePath)
        {
            var absPath = Path.Combine(Directory.GetCurrentDirectory(), filePath);
            return new SolutionDocumentBuilder()
                .WithAbsolutePath(absPath)
                .Build();
        }

        public static void AssertFilesAreEqual(string actual, string snapshot)
        {
            var actualBytes = File.ReadAllBytes(actual);
            var snapshotBytes = File.ReadAllBytes(snapshot);

            Assert.True(actualBytes.SequenceEqual(snapshotBytes), "Given file differs from snapshot");
        }
    }
}
