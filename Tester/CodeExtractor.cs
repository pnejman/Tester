using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Tester
{
    public class CodeExtractor
    {
        public event EventHandler<string> msgFromExtractor;
        private int detectedVersion = -1;

        private void TryAnotherVersion(string versionName, int numberToReturn, string metadata, string regexPattern)
        {
            Match match = Regex.Match(metadata, regexPattern);
            if (match.ToString() == metadata)
            {
                msgFromExtractor?.Invoke(this, $"{versionName} detected.");
                detectedVersion = numberToReturn;
            }
        }

        private int CheckMetadataVersion(string metadata)
        {
            TryAnotherVersion("Version 1", 1, metadata, "[0-9]+~[A-Za-z0-9_ ]+~[A-Z0-9]+_[A-Z0-9]+~[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+(\\..[a-zA-Z]+)+$");
            TryAnotherVersion("Version 2", 2, metadata, "[0-9]+~[A-Za-z0-9_ ]+~[A-Z0-9]+_[A-Z0-9]+_[A-Z0-9]+~[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+(\\..[a-zA-Z]+)+$");
            TryAnotherVersion("Version 3", 3, metadata, "[0-9]+~[A-Za-z0-9_ ]+~[A-Z0-9]+_[A-Z0-9]+_[A-Z0-9]+~[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+_.*?~.*?(\\..[a-zA-Z]+)+$");
            TryAnotherVersion("Version 4 (4a)", 41, metadata, "[0-9]+~[A-Za-z0-9_ ]+~[A-Z0-9]+_[A-Z0-9]+_v[0-9]+~[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+_.*?~.*?(\\..[a-zA-Z]+)+$");
            TryAnotherVersion("Version 4 (4b)", 42, metadata, "[0-9]+~[A-Za-z0-9_ ]+~[A-Z0-9]+_[A-Z0-9]+_v[0-9]+~[A-Z0-9]+~[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+_.*?~.*?(\\..[a-zA-Z]+)+$");
            TryAnotherVersion("Version 5", 5, metadata, "<Object>[\\s\\S]+<ItemType>[a-zA-Z0-9 ]+<Metadata>[\\s\\S]+</ *Metadata>[\\s\\S]+</ *Object>");
            TryAnotherVersion("Version 6", 6, metadata, "<Object>[\\s\\S]+<ItemType>[\\s\\S]+<Code>[\\s\\S]+<Market>(?!.*BG|.*PL|.*EL).*</Market>+[\\s\\S]+<Version>[\\s\\S]+<Metadata>[\\s\\S]+</ *Metadata>[\\s\\S]+</ *Object>");
            TryAnotherVersion("Version 7 (7a)", 71, metadata, "<Object>[\\s\\S]+<ItemType>[\\s\\S]+<Code>[\\s\\S]+<Market>(?!.*BG|.*PL|.*EL).*</Market>+[\\s\\S]+<Version>[\\s\\S]+<Metadata>[\\s\\S]+</ *Metadata>[\\s\\S]+</ *Object>");
            TryAnotherVersion("Version 7 (7b)", 72, metadata, "<Object>[\\s\\S]+<ItemType>[\\s\\S]+<Code>[\\s\\S]+<Market>(BG|PL|EL)</Market>+[\\s\\S]+<Version>[\\s\\S]+<Metadata>[\\s\\S]+</ *Metadata>[\\s\\S]+</ *Object>");
 
            if (detectedVersion == -1)
            {
                msgFromExtractor?.Invoke(this, "Error: Metadata version not recognized.");
            }
            return detectedVersion;
        }


        public string GetCodeFromString(string metadata)
        {
            string extractedCode = "";
            string match;

            switch (CheckMetadataVersion(metadata))
            {
                case 1:
                    match = Regex.Replace(metadata, "(^(.*?~){2}.*?_)|(~.*?$)", "");
                    extractedCode = match.ToString();
                    break;
                case 2:
                    match = Regex.Replace(metadata, "(^(.*?~){2}.*?_.*?_)|(~.*?$)", "");
                    extractedCode = match.ToString();
                    break;
                case 3:
                    match = Regex.Replace(metadata, "(^(.*?~){2}.*?_.*?_)|(~.*?$)", "");
                    extractedCode = match.ToString();
                    break;
                case 41:
                    match = Regex.Replace(metadata, "(^(.*?~){3}[a-z0-9-_]+)|(~.*?$)", "");
                    extractedCode = match.ToString();
                    break;
                case 42:
                    match = Regex.Replace(metadata, "(^(.*?~){3})|(~.*?$)", "");
                    extractedCode = match.ToString();
                    break;
                case 5:
                    match = Regex.Replace(metadata, "<Object>[\\s\\S]+<Metadata>(.*?~){2}|~.*?</ *Metadata>[\\s\\S]+</ *Object>", "");
                    extractedCode = match.ToString();
                    break;
                case 6:
                    match = Regex.Replace(metadata, "\\{[\\s\\S]+\"object\":[\\s\\S]+\"Metadata\": \".*?~.*?~|~[\\s\\S]+", "");
                    extractedCode = match.ToString();
                    break;
                case 71:
                    match = Regex.Replace(metadata, "<Object>[\\s\\S]+<ItemType>[\\s\\S]+<Code>|</Code>[\\s\\S]+", "");
                    extractedCode = match.ToString();
                    break;
                case 72:
                    match = Regex.Replace(metadata, "<Object>[\\s\\S]+<ItemType>[\\s\\S]+<Code>|[A-Z]{2}</Code>[\\s\\S]+", "");
                    extractedCode = match.ToString();
                    break;
                default:
                    break;
            }
            return extractedCode;
        }
    }
}
