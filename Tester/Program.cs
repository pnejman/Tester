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
        static void Main(string[] args)
        {
            var v1 = new LegacyObjectMetadataProvider.V1();
            var v2 = new LegacyObjectMetadataProvider.V2();
            var v3 = new LegacyObjectMetadataProvider.V3();
            var v4 = new LegacyObjectMetadataProvider.V4();
            var v5 = new LegacyObjectMetadataProvider.V5();
            var v6 = new LegacyObjectMetadataProvider.V6();
            var v7 = new LegacyObjectMetadataProvider.V7();

            string metadata = v3.ProvideMetadata();

            CodeExtractor codeExtractor = new CodeExtractor();
            codeExtractor.msgFromExtractor += OnMsgFromExtractor; //subscribe to msgFromExtractor event with OnMsgFromExtractor method
            string extractedCode = codeExtractor.GetCodeFromString(metadata);

            Validate(extractedCode, metadata);
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
                Console.WriteLine($"Error while extracting code from:\r\n\r\n" +
                                  $"{metadata}\r\n\r\n" +
                                  $" {error.Message}");
                Console.ReadKey(true);
                return;
            }
            Console.WriteLine($"Metadata:\r\n{metadata}\r\n\r\n" +
                              $"Extracted Code: {extractedCode}");
            Console.ReadKey(true);
        }

        private static void OnMsgFromExtractor(object sender, string msg)
        {
            Console.WriteLine(msg+"\r\n");
        }
    }
}
