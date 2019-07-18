using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcmeCorp.Training.Services;

namespace Tester
{
    class Program
    {
        readonly static int numberOfTries = 1000000; //<--can be changed by Designer
        static bool displayFullMsgs = false; //<-- can be changed by Designer

        static int numberOfSuccess = 0;

        static void Main(string[] args)
        {
            var last = new LegacyObjectMetadataProvider.LatestVersionProvider();
            CodeExtractor codeExtractor = new CodeExtractor();
            codeExtractor.msgFromExtractor += OnMsgFromExtractor; //subscribe to msgFromExtractor event with OnMsgFromExtractor method

            for (int trial = 0; trial < numberOfTries; trial++)
            {
                OptionallyWriteLine("--------------------");
                string metadata = last.ProvideMetadata();
                string extractedCode = codeExtractor.GetCodeFromString(metadata);
                Validate(extractedCode, metadata);

                if (!displayFullMsgs)
                {
                    Console.Clear();
                    Console.WriteLine($"Try: {trial + 1} of {numberOfTries}\r\n" +
                                      $"Success: {numberOfSuccess} of {numberOfTries}");
                }
            }
            OptionallyWriteLine($"Success rate: {numberOfSuccess}/{numberOfTries}");
            Console.ReadKey(true);
        }

        static void Validate(string extractedCode, string metadata)
        {
            var validator = new ObjectCodeValidator();
            try
            {
                validator.AssertCodeIsValid(extractedCode, metadata);
            }
            catch (Exception error)
            {
                OptionallyWriteLine($"Error while extracting code from:\r\n\r\n" +
                                    $"{metadata}\r\n\r\n" +
                                    $" {error.Message}");
                return;
            }

            OptionallyWriteLine($"Metadata:\r\n{metadata}\r\n\r\n" +
                                $"Extracted Code: {extractedCode}");
            numberOfSuccess++;
        }

        private static void OnMsgFromExtractor(object sender, string msg)
        {
            OptionallyWriteLine(msg + "\r\n");
        }

        private static void OptionallyWriteLine(string text)
        {
            if (displayFullMsgs)
            {
                Console.WriteLine(text);
            }
        }
    }
}
