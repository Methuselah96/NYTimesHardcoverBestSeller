using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NYTimesHardcoverBestSellers
{
    public enum Genre
    {
        Fiction,
        NonFiction,
    }

    public class Entry
    {
        public string Title { get; set; }
        public int? LastWeek { get; set; }
        public int WeeksOnList { get; set; }
    }

    public class BestSellerList
    {
        public Genre? Genre { get; set; }
        public List<Entry> Entries { get; set; } = new List<Entry>();
    }

    public static class TextParser
    {
        public static BestSellerList Parse(string text)
        {
            var bestSellerList = new BestSellerList();
            using var stringReader = new StringReader(text);
            var line = stringReader.ReadLine();
            while (line != null && !line.Contains("Fiction") && !line.Contains("Non-Fiction"))
            {
                line = stringReader.ReadLine();
            }

            if (line != null && line.Contains("Non-Fiction"))
            {
                bestSellerList.Genre = Genre.NonFiction;
            }
            else if (line != null && line.Contains("Fiction"))
            {
                bestSellerList.Genre = Genre.Fiction;
            }

            line = stringReader.ReadLine();
            if (!string.IsNullOrWhiteSpace(line))
            {
                throw new Exception("Expected a blank line!");
            }

            while (line != null && string.IsNullOrWhiteSpace(line))
            {
                line = stringReader.ReadLine();
            }

            var count = 1;
            while (line != null && !line.Contains("Hawes Publications"))
            {
                var stringBuilder = new StringBuilder();
                string data = null;
                if (line.StartsWith($"{count}"))
                {
                    if (char.IsDigit(line.Trim().Last()))
                    {
                        var (dataTemp, title) = SplitIntoParts(line);
                        data = dataTemp;
                        stringBuilder.Append(title);

                        line = stringReader.ReadLine();
                    }
                    else
                    {
                        var nextLine = stringReader.ReadLine();
                        var (dataTemp, title) = SplitTwoLinesIntoParts(line, nextLine);
                        data = dataTemp;
                        stringBuilder.Append(title);

                        line = stringReader.ReadLine();
                    }
                }
                else
                {
                    stringBuilder.Append(line);
                    line = stringReader.ReadLine();
                }

                if (!string.IsNullOrWhiteSpace(line))
                {
                    if (data == null)
                    {
                        if (!line.StartsWith($"{count}"))
                        {
                            throw new Exception("Assumed line would start with count.");
                        }

                        var (dataTemp, title) = SplitIntoParts(line);
                        data = dataTemp;
                        stringBuilder.Append(title);
                    }
                    else
                    {
                        stringBuilder.Append(line);
                    }

                    line = stringReader.ReadLine();
                    while (!string.IsNullOrWhiteSpace(line))
                    {
                        stringBuilder.Append(line);
                        line = stringReader.ReadLine();
                    }
                }

                if (data == null) throw new Exception("You done messed up.");
                bestSellerList.Entries.Add(ParseEntry(data, stringBuilder.ToString()));
                count++;
                while (line != null && string.IsNullOrWhiteSpace(line))
                {
                    line = stringReader.ReadLine();
                }
            }

            return bestSellerList;
        }

        private const string DataPattern = @"(\d+) ([-\dNF]+) ([\d]+)";

        public static Entry ParseEntry(string data, string title)
        {
            var dataRegex = new Regex(DataPattern);
            var dataMatch = dataRegex.Match(data);
            var match3Text = dataMatch.Groups[2].Value;
            match3Text = match3Text.EndsWith("F")
                ? match3Text.EndsWith("NF")
                    ? match3Text.Remove(match3Text.Length - 2)
                    : match3Text.Remove(match3Text.Length - 1)
                : match3Text;
            var lastWeek = match3Text == "--" ? default(int?) : int.Parse(match3Text);
            var weeksOnList = int.Parse(dataMatch.Groups[3].Value);

            var parenthesesIndex = title.IndexOf('(');
            var titleString = title.Substring(0, parenthesesIndex).Trim();

            if (string.IsNullOrWhiteSpace(titleString)) throw new Exception("No title.");

            return new Entry
            {
                Title = titleString,
                LastWeek = lastWeek,
                WeeksOnList = weeksOnList,
            };
        }

        public static (string Data, string Title) SplitIntoParts(string line)
        {
            var parts = line.Split(' ');
            var rightParts = new[] { parts[^3], parts[^2], parts[^1] };
            var rightData = string.Join(' ', rightParts);
            var fullDataParts = new[] { parts[0], rightData };
            var data = string.Join(' ', fullDataParts);
            var length = line.Length - rightData.Length - parts[0].Length - 1;
            var title = line.Substring(parts[0].Length + 1, length);
            return (data, title);
        }

        public static (string Data, string Title) SplitTwoLinesIntoParts(string line1, string line2)
        {
            var parts1 = line1.Split(' ');
            var fullDataParts = new[] { parts1[0], line2 };
            var data = string.Join(' ', fullDataParts);
            var length = line1.Length - parts1[0].Length - 1;
            var title = line1.Substring(parts1[0].Length + 1, length);
            return (data, title);
        }
    }
}
