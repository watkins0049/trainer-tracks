using System.IO;
using System.Text.RegularExpressions;

namespace TrainerTracks.Security
{
    public static class FileSanitizerExtensions
    {
        // Stolen from https://stackoverflow.com/questions/309485/c-sharp-sanitize-file-name
        public static string SanitizeFileName(this string fileName)
        {
            string invalidChars = Regex.Escape(new string(Path.GetInvalidFileNameChars()));
            string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

            return Regex.Replace(fileName, invalidRegStr, "");
        }

    }
}
