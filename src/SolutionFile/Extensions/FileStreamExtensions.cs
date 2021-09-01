using System;
using System.IO;

namespace SolutionFile.Extensions
{
    public static class FileStreamExtensions
    {
        public static string GetLineEnding(this FileStream file)
        {
            // We have a memory leak here, because we do not close this reader
            // This allows us to use that file stream again
            // The memory leak is not a problem, this method will only be called once
            // in this program's lifetime
            var reader = new StreamReader(file);
            var lineEnding = string.Empty;
            const char lf = '\x0A';
            const char cr = '\x0D';

            while (reader.Peek() != -1 && lineEnding.Length == 0)
                switch (reader.Read())
                {
                    case lf:
                    {
                        lineEnding = lf.ToString();
                        break;
                    }
                    case cr:
                    {
                        if (reader.Read() == lf)
                        {
                            lineEnding = new string(new[] { cr, lf });
                        }

                        break;
                    }
                }

            if (lineEnding.Length == 0)
            {
                throw new ArgumentException("Failed to read line ending of given file");
            }

            file.Position = 0;
            return lineEnding;
        }

        public static bool HasByteOrderMark(this FileStream file)
        {
            var bits = new byte[3];
            file.Read(bits, offset: 0, count: 3);
            file.Position = 0;

            return bits[0] == 0xEF && bits[1] == 0XBB && bits[2] == 0xBF;
        }
    }
}
