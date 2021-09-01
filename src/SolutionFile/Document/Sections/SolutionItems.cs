using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace SolutionFile.Document.Sections
{
    public class SolutionItems : IDocumentSection
    {
        public SolutionItems(string folderName)
        {
            FolderName = folderName;
        }

        public string FolderName { get; }
        public OrderedDictionary Elements { get; set; } = new();

        public IEnumerable<string> ToRawLines()
        {
            var lines = new List<string>();

            lines.Add("\tProjectSection(SolutionItems) = preProject");
            lines.AddRange(Elements.Cast<DictionaryEntry>().Select(de => $"\t\t{de.Key} = {de.Value}"));
            lines.Add("\tEndProjectSection");

            return lines;
        }
    }
}
