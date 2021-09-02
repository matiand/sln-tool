using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SolutionFile.Document.Sections;
using SolutionFile.Extensions;

namespace SolutionFile.Document
{
    public class SolutionDocument
    {
        public static Guid FolderTypeId = new("2150E333-8FDC-42A3-9474-1A3956D46DE8");
        private readonly string _lineEnding;
        private readonly bool _shouldEmitByteOrderMark;

        public SolutionDocument(string lineEnding, bool shouldEmitByteOrderMark)
        {
            _lineEnding = lineEnding;
            _shouldEmitByteOrderMark = shouldEmitByteOrderMark;
        }

        public List<IDocumentSection> Sections { get; } = new();

        public void AddFileToFolder(string folder, string file)
        {
            if (!FolderExists(folder))
            {
                AddFolder(folder);
            }

            var itemsSection =
                (SolutionItems)Sections.First(s => s is SolutionItems sItems && sItems.FolderName == folder);
            // Silently ignore existing entries
            itemsSection.Elements.TryAdd(file, file);
        }

        public void RemoveFileToFolder(string folder, string file)
        {
            if (!FolderExists(folder)) throw new ArgumentException("Folder could not be found");

            var itemsSection =
                (SolutionItems)Sections.First(s => s is SolutionItems sItems && sItems.FolderName == folder);
            itemsSection.Elements.Remove(file);
        }

        public void SaveToFile(string filePath)
        {
            var rawLines = Sections.SelectMany(s => s.ToRawLines());

            var rawDocBuilder = new StringBuilder();
            rawDocBuilder.AppendJoin(_lineEnding, rawLines);
            rawDocBuilder.Append(_lineEnding);

            var encoding = new UTF8Encoding(_shouldEmitByteOrderMark);
            File.WriteAllText(filePath, rawDocBuilder.ToString(), encoding);
        }

        private bool FolderExists(string name)
        {
            return Sections.Any(s => s is ProjectBodyHeader project && project.Name == name);
        }

        private void AddFolder(string folderName)
        {
            if (Sections.Count == 0)
            {
                throw new InvalidOperationException("Cannot add folder to document, when document is empty");
            }

            // Empty folder block consists of: ProjectBodyHeader, SolutionItems, EndProjectBody sections
            var header = new ProjectBodyHeader
            {
                Id = Guid.NewGuid(),
                Name = folderName,
                // Folder is a virtual construct that groups related files, therefore path
                // is the same as its name
                // Maybe in the future we can build them outside of solution root
                RelativePath = folderName,
                ProjectType = FolderTypeId,
            };
            var folderBlock = new List<IDocumentSection>
            {
                header,
                new SolutionItems(folderName),
                new EndProjectBody(),
            };

            var insertIdx = Sections.FindLastIndex(s => s is GlobalBodyHeader);
            Sections.InsertRange(insertIdx, folderBlock);
        }
    }
}
