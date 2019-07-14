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
        private int CheckMetadataVersion(string metadata)
        {
            Match match = Regex.Match(metadata, "[0-9]+~[A-Za-z0-9_ ]+~[A-Z0-9]+_[A-Z0-9]+~[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+(..[a-zA-Z]+)+$");
            if (match.ToString() == metadata)
            {
                Console.WriteLine("Version 1 detected.");
                return 1;
            }

            match = Regex.Match(metadata, "[0-9]+~[A-Za-z0-9_ ]+~[A-Z0-9]+_[A-Z0-9]+_[A-Z0-9]+~[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+-[a-z0-9]+(..[a-zA-Z]+)+$");
            if (match.ToString() == metadata)
            {
                Console.WriteLine("Version 2 detected.");
                return 1;
            }

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
                default:
                    break;
            }


            return extractedCode;
        }
    }
}
