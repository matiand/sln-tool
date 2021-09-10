using Xunit;

namespace SnapshotTests
{
    public class SaveToFileTests
    {
        [Theory]
        [InlineData("crlf")]
        [InlineData("lf")]
        public void SavingPreservesLineEndings(string lineEnding)
        {
            var doc = Helpers.LoadSolutionFile($"./input/example-with-{lineEnding}");

            // Use different output name in different test classes, because xunit runs
            // tests from the same class in sequence, but from different classes in parallel
            doc.SaveToFile("./out-SaveToFileTests");

            Helpers.AssertFilesAreEqual("./out-SaveToFileTests", $"./snapshots/example-with-{lineEnding}");
        }

        [Fact]
        public void IfFileHasBom_SavingPreservesIt()
        {
            var doc = Helpers.LoadSolutionFile("./input/example-with-bom");

            doc.SaveToFile("./out-SaveToFileTests");

            Helpers.AssertFilesAreEqual("./out-SaveToFileTests", "./snapshots/example-with-bom");
        }

        [Fact]
        public void IfFileDoesNotHaveBom_SavingDoesNotAddIt()
        {
            var doc = Helpers.LoadSolutionFile("./input/example-with-no-bom");

            doc.SaveToFile("./out-SaveToFileTests");

            Helpers.AssertFilesAreEqual("./out-SaveToFileTests", "./snapshots/example-with-no-bom");
        }

        [Theory]
        [InlineData("example-one")]
        [InlineData("example-two")]
        public void GivenNoModification_SavedFileEqualsLoadedFile(string filePath)
        {
            var docOne = Helpers.LoadSolutionFile($"./input/{filePath}");

            docOne.SaveToFile("./out-SaveToFileTests");

            Helpers.AssertFilesAreEqual("./out-SaveToFileTests", $"./snapshots/{filePath}");
        }
    }
}
