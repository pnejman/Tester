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
            string metadata = v1.ProvideMetadata();

            CodeExtractor codeExtractor = new CodeExtractor();
            string extractedCode = codeExtractor.GetCodeFromString(metadata);

            var validator = new ObjectCodeValidator();
            try
            {
                validator.AssertCodeIsValid(extractedCode, metadata);
            }
            catch(Exception error)
            {
                Console.WriteLine($"Error while extracting code from:\r\n" +
                                  $"{metadata}\r\n" +
                                  $" {error.Message}");
                Console.ReadKey(true);
                return;
            }
            Console.WriteLine($"Metadata: {metadata}\r\n" +
                              $"Extracted Code: {extractedCode}");
            Console.ReadKey(true);
        }
    }
}
