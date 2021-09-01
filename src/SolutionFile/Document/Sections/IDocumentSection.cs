using System.Collections.Generic;

namespace SolutionFile.Document.Sections
{
    public interface IDocumentSection
    {
        // We are not using ToString, because each class already has ToString implemented
        // Using custom method gets rid of bugs if we were to forget to override ToString
        public IEnumerable<string> ToRawLines();
    }
}
