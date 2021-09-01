using SolutionFile.Document;

namespace Cli
{
    public interface ISolutionDocumentAction
    {
        public void Run(SolutionDocument document);
    }
}
