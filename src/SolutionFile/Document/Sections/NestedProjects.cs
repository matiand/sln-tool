using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace SolutionFile.Document.Sections
{
    public class NestedProjects : IDocumentSection
    {
        public OrderedDictionary ParentIdsByChildId { get; set; } = new();

        public IEnumerable<string> ToRawLines()
        {
            var lines = new List<string> { "\tGlobalSection(NestedProjects) = preSolution" };

            lines.AddRange(ParentIdsByChildId.Cast<DictionaryEntry>()
                .Select(entry =>
                {
                    // These ids are Guid in string form, we need to capitalize them
                    var childId = entry.Key.ToString()?.ToUpper();
                    var parentId = entry.Value?.ToString()?.ToUpper();
                    return $"\t\t{{{childId}}} = {{{parentId}}}";
                }));

            lines.Add("\tEndGlobalSection");

            return lines;
        }
    }
}
