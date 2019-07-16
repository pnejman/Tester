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

        private int CheckMetadataVersion(string metadata)
        {
            Match match = Regex.Match(metadata, "[0-9]+~[A-Za-z0-9_ ]+~[A-Z0-9]+_[A-Z0-9]+~[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+(\\..[a-zA-Z]+)+$");
            if (match.ToString() == metadata)
            {
                msgFromExtractor(this, "Version 1 detected.");
                return 1;
            }

            match = Regex.Match(metadata, "[0-9]+~[A-Za-z0-9_ ]+~[A-Z0-9]+_[A-Z0-9]+_[A-Z0-9]+~[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+(\\..[a-zA-Z]+)+$");
            if (match.ToString() == metadata)
            {
                msgFromExtractor(this, "Version 2 detected.");
                return 2;
            }

            match = Regex.Match(metadata, "[0-9]+~[A-Za-z0-9_ ]+~[A-Z0-9]+_[A-Z0-9]+_[A-Z0-9]+~[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+_.*?~.*?(\\..[a-zA-Z]+)+$");
            if (match.ToString() == metadata)
            {
                msgFromExtractor(this, "Version 3 detected.");
                return 3;
            }

            match = Regex.Match(metadata, "[0-9]+~[A-Za-z0-9_ ]+~[A-Z0-9]+_[A-Z0-9]+_v[0-9]+~[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+_.*?~.*?(\\..[a-zA-Z]+)+$");
            if (match.ToString() == metadata)
            {
                msgFromExtractor(this, "Version 4 (4a) detected.");
                return 41;
            }

            match = Regex.Match(metadata, "[0-9]+~[A-Za-z0-9_ ]+~[A-Z0-9]+_[A-Z0-9]+_v[0-9]+~[A-Z0-9]+~[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+_.*?~.*?(\\..[a-zA-Z]+)+$");
            if (match.ToString() == metadata)
            {
                msgFromExtractor(this, "Version 4 (4b) detected.");
                return 42;
            }

            match = Regex.Match(metadata, "<Object>[\\s\\S]+<Metadata>[\\s\\S]+</ *Metadata>[\\s\\S]+</ *Object>");
            if (match.ToString() == metadata)
            {
                msgFromExtractor(this, "Version 5 detected.");
                return 5;
            }

            msgFromExtractor(this, "Error: Metadata version not recognized.");
            return 0;
        }


        public string GetCodeFromString(string metadata)
        {
            string extractedCode = "";
            string match = "";

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
                default:
                    break;
            }

            return extractedCode;
        }
    }
}
