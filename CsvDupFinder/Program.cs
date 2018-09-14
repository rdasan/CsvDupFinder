using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CsvDupFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter File Name (Provide complete File Path): ");
            var fileName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(fileName) || !File.Exists(fileName))
            {
                Console.WriteLine("File does not exist.");
                return;
            }

            Console.Write("Enter Column name to check for duplicates: ");
            var columnName = Console.ReadLine();

            Tuple<int, string[]> linesTuple = ReadCsvFile(fileName, columnName);

            if (linesTuple == null)
            {
                Console.WriteLine("Could not load file or Incorrect column name");
                return;
            }

            var duplicateRows = FindDuplicateRows(linesTuple);

            Console.WriteLine("Duplicate Entries: ");
            duplicateRows.ForEach(Console.WriteLine);
            Console.Read();
        }

        private static List<string> FindDuplicateRows(Tuple<int, string[]> linesTuple)
        {
            var columnIndex = linesTuple.Item1;
            var dataRows = linesTuple.Item2;
            var groupings = dataRows.GroupBy(line => line.Split(',')[columnIndex]).ToArray();
            var duplicateRows = groupings.Where(grp => grp.Count() > 1).SelectMany(grp => grp).ToList();
            return duplicateRows;
        }

        private static Tuple<int, string[]> ReadCsvFile(string fileName, string columnName)
        {
            var allLines = File.ReadAllLines(fileName);
            var colHeaderLines = allLines[0].Split(',').ToList();
            var colIndex = colHeaderLines.FindIndex(x => x.Equals(columnName));

            if (colIndex < 0)
                return null;

            allLines = allLines.Where(x => x != allLines[0]).ToArray();
            return new Tuple<int, string[]>(colIndex, allLines);
        }
    }
}
