using System;
using Microsoft.VisualBasic.FileIO;
using System.IO;
using System.Linq;

interface ISerialNumberGenerator
{
    string GenerateSerialNumber(string input);
}

class HashSerialNumberGenerator : ISerialNumberGenerator
{
    public string GenerateSerialNumber(string input)
    {
        int hash = input.GetHashCode();
        return hash.ToString("X8"); // Convert to hexadecimal
    }
}

class Program
{
    static void Main()
    {
        string path = "books.csv";
        string outputPath = "hashed_books.csv"; // Output file

        ISerialNumberGenerator serialNumberGenerator = new HashSerialNumberGenerator();

        using (TextFieldParser parser = new TextFieldParser(path))
        using (StreamWriter sw = new StreamWriter(outputPath))
        {
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(",");
            bool isFirstLine = true;
            while (!parser.EndOfData)
            {
                string[] fields = parser.ReadFields();
                string line = String.Join(",", fields);

                if (isFirstLine)
                {
                    // Write the header line to the new CSV file
                    sw.WriteLine(line);
                    isFirstLine = false;
                }
                else
                {
                    string serialNumber = serialNumberGenerator.GenerateSerialNumber(line);

                    // Add quotes back to each field and write the hashed line to the new CSV file
                    string quotedLine = String.Join(",", fields.Select(field => $"\"{field}\""));
                    sw.WriteLine($"{serialNumber},{quotedLine}");
                }
            }
        }
    }
}
