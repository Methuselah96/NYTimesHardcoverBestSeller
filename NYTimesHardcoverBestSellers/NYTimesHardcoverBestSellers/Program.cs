using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using Path = System.IO.Path;

namespace NYTimesHardcoverBestSellers
{
    class Result
    {
        public BestSellerList FictionList { get; set; }
        public BestSellerList NonFictionList { get; set; }
    }

    class Info
    {
        public int Points { get; set; }
        public int Count { get; set; }
        public DateTime Last { get; set; }
    }

    class Program
    {
        private static readonly DateTime FirstDateTime = new DateTime(1931, 10, 12);
        private static readonly HttpClient Client = new HttpClient();
        private static readonly Dictionary<string, Info> Info = new Dictionary<string, Info>();
        private const int MaxPoints = 16;

        private static readonly List<DateTime> SkipDates = new List<DateTime>
        {
            new DateTime(1931, 10, 19),
            new DateTime(1931, 10, 26),
            new DateTime(1931, 11, 2),
            new DateTime(1931, 11, 9),
            new DateTime(1931, 11, 23),
        };

        public static readonly Dictionary<string, string> Aliases = new Dictionary<string, string>
        {
            {"EPIC OF AMERICA, by James Truslow Adams.", "THE EPIC OF AMERICA, by James Truslow Adams."},
            {"THE GOOD EARTH, by Pearl Buck.", "THE GOOD EARTH, by Pearl S. Buck."},
            {"BROOME STAGES, by Clemence Dane.", "BROOME STAGES, by Clemance Dane."},
            {"ONLY YESTERDAY, by Frederick Lewis Allen.", "ONLY YESTERDAY, by F. L. Allen."},
            {"MOURNING BECOMES ELECTRA, by Eugene O'Neill.", "MOURNING BECOMES ELECTRA, by Eugene O’Neill."}
        };

        static async Task Main()
        {
            var loopDateTime = FirstDateTime;
            var dateTimes = new List<DateTime>();
            while (loopDateTime <= DateTime.Today)
            {
                if (loopDateTime >= new DateTime(1978, 8, 13) && loopDateTime <= new DateTime(1978, 11, 5))
                {
                    loopDateTime = loopDateTime.AddDays(7);
                    continue;
                }
                dateTimes.Add(loopDateTime);

                if (loopDateTime == new DateTime(1943, 11, 22))
                {
                    loopDateTime = loopDateTime.AddDays(6);
                }
                else
                {
                    loopDateTime = loopDateTime.AddDays(7);
                }
            }

            foreach (var dateTime in dateTimes)
            {
                if (SkipDates.Contains(dateTime))
                {
                    continue;
                }
                await Download(dateTime);
            }

            Dictionary<string, int> lastWeek = null;
            foreach (var dateTime in dateTimes.Where(dateTime => dateTime < new DateTime(1960, 1, 1)))
            {
                if (SkipDates.Contains(dateTime))
                {
                    lastWeek = new Dictionary<string, int>();
                    continue;
                }
                var result = MakeRequest(dateTime);
                var newLastWeek = new Dictionary<string, int>();
                ProcessAndValidateBestSellerList(result.FictionList, lastWeek, newLastWeek, dateTime);
                ProcessAndValidateBestSellerList(result.NonFictionList, lastWeek, newLastWeek, dateTime);
                lastWeek = newLastWeek;
                // Console.WriteLine(dateTime);
            }

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Title,Points,Count");
            foreach (var (key, info) in Info)
            {
                var escapedKey = key.Contains(",") ? $"\"{key}\"" : key;
                stringBuilder.AppendLine($"{escapedKey},{info.Points},{info.Count}");
            }

            var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "nyt-hardcover.csv");
            await File.WriteAllTextAsync(filePath, stringBuilder.ToString());
        }

        public static string SubPath(DateTime dateTime)
        {
            return $"{dateTime.Year}-{dateTime.Month:D2}-{dateTime.Day:D2}.pdf";
        }

        public static async Task Download(DateTime dateTime)
        {
            var directoryPath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "NYTimesPdfs");
            Directory.CreateDirectory(directoryPath);
            var path = Path.Join(directoryPath, SubPath(dateTime));
            if (File.Exists(path))
            {
                return;
            }
            var url = $"http://www.hawes.com/{dateTime.Year}/{SubPath(dateTime)}";
            var response = await Client.GetAsync(url);

            var j = 1;
            while (!response.IsSuccessStatusCode)
            {
                Thread.Sleep(j * 1000);
                response = await Client.GetAsync(url);
                j *= 2;
            }
            Console.WriteLine(dateTime);

            var responseBody = await response.Content.ReadAsByteArrayAsync();
            await File.WriteAllBytesAsync(path, responseBody);
        }

        public static readonly List<(DateTime, string)> LastWeekRankExceptions = new List<(DateTime, string)>
        {
            (new DateTime(1932, 6, 6), "THE BLACK SWAN, by Rafael Sabatini."),
            (new DateTime(1934, 6, 4), "ON OUR WAY, by Franklin D. Roosevelt."),
            (new DateTime(1935, 1, 7), "WINE FROM THESE GRAPES, by Edna St. Vincent Millay."),
            (new DateTime(1936, 7, 20), "STORIES OF THREE DECADES, by Thomas Mann."),
            (new DateTime(1936, 9, 7), "THEY WALK IN THE CITY, by J. B. Priestley."),
            (new DateTime(1937, 4, 26), "GONE WITH THE WIND, by Margaret Mitchell."),
            (new DateTime(1938, 7, 18), "THE DARK RIVER, by Charles Nordloff and James Norman Hall."),
            (new DateTime(1938, 9, 12), "THE DARK RIVER, by Charles Nordloff and James Norman Hall."),
            (new DateTime(1940, 1, 15), "LIFE OF GREECE, by Will Durant."),
            (new DateTime(1944, 8, 6), "LEAVE HER TO HEAVEN, by Ben Ames Williams."),
            (new DateTime(1944, 8, 6), "PRESIDENTIAL AGENT, by Upton Sinclair."),
            (new DateTime(1945, 10, 28), "THE WHITE TOWER, by James Ramsey Ullman."),
            (new DateTime(1945, 10, 28), "SO WELL REMEMBERED, by James Hilton."),
            (new DateTime(1945, 11, 11), "SO WELL REMEMBERED, by James Hilton."),
            (new DateTime(1945, 11, 11), "THE PEACOCK SHEDS HIS TAIL, by Alice Tisdale Hobart."),
            (new DateTime(1945, 11, 18), "CAPTAIN FROM CASTILLE, by Samuel Shellaburger."),
            (new DateTime(1945, 11, 18), "FOREVER AMBER, by Kathleen Winsor."),
            (new DateTime(1946, 3, 3), "YOUR INCOME TAX, by J. K. Lasser."),
            (new DateTime(1946, 3, 31), "LOVELY IS THE LEE, by Robert Gibbings."),
            (new DateTime(1946, 4, 21), "THE CIANO DIARIES, by Galeazzo Ciano."),
            (new DateTime(1946, 4, 21), "ON THE EDGE OF EVENING, by Cornelius Weygandt."),
            (new DateTime(1946, 8, 18), "THE SCARLET TREE, by Osbert Sitwell."),
            (new DateTime(1947, 7, 13), "THE SLING AND THE ARROW, by Stuart David Engstrand."),
            (new DateTime(1949, 1, 30), "THE ROOSEVELT MYTH, by John T. Flynn."),
            (new DateTime(1950, 1, 29), "THE BIG FISHERMAN, by Lloyd C. Douglas."),
        };

        public static readonly List<(DateTime, string)> FirstWeeksOnList = new List<(DateTime, string)>
        {
            (new DateTime(1938, 9, 5), "DESIGNING WOMEN, by Margarette Byers with Consuelo Kamholz."),
            (new DateTime(1950, 1, 22), "ROOSEVELT AND THE RUSSIANS, by Edward R. Stettinius Jr."),
        };

        public static readonly List<(DateTime, string)> MultiWeeksOnList = new List<(DateTime, string)>
        {
            (new DateTime(1940, 2, 19), "INSIDE EUROPE, by John Gunther."),
            (new DateTime(1940, 2, 26), "YOUR INCOME TAX, by J. K. Lasser."),
            (new DateTime(1940, 7, 22), "MEIN KAMPF, by Adolph Hitler."),
            (new DateTime(1946, 1, 27), "YOUR INCOME TAX, by J. K. Lasser."),
            (new DateTime(1947, 11, 2), "PETTICOAT SURGEON, by Bertha Van Hoosen."),
            (new DateTime(1947, 11, 9), "PETTICOAT SURGEON, by Bertha Van Hoosen."),
            (new DateTime(1948, 2, 22), "THREE CAME HOME, by Agnes Newton Keith."),
            (new DateTime(1950, 1, 1), "THE SEVEN STOREY MOUNTAIN, by Thomas Merton."),
            (new DateTime(1950, 1, 8), "FATHER OF THE BRIDE, by Edward Streeter."),
            (new DateTime(1950, 2, 5), "THE ROAD AHEAD, by John T. Flynn."),
            (new DateTime(1950, 3, 12), "A RAGE TO LIVE, by John O'Hara."),
            (new DateTime(1950, 3, 12), "MY THREE YEARS IN MOSCOW, by Walter Bedell Smith."),
            (new DateTime(1950, 3, 19), "A RAGE TO LIVE, by John O'Hara."),
            (new DateTime(1950, 3, 19), "MY THREE YEARS IN MOSCOW, by Walter Bedell Smith."),
        };

        public static void ProcessAndValidateBestSellerList(BestSellerList result, Dictionary<string, int> lastWeek,
            Dictionary<string, int> newLastWeek, DateTime dateTime)
        {
            var entries = result.Entries;
            if (entries.Count > MaxPoints)
            {
                throw new Exception($"Max book titles is at least {entries.Count}");
            }

            if (entries.Count == 0)
            {
                throw new Exception("That's odd.");
            }

            for (var i = 0; i < entries.Count; i++)
            {
                var entry = entries[i];
                var points = MaxPoints - i;
                var rank = i + 1;
                var aliasTitle = Aliases.ContainsKey(entry.Title) ? Aliases[entry.Title] : entry.Title;
                newLastWeek[aliasTitle] = rank;
                if (Info.ContainsKey(aliasTitle))
                {
                    if (lastWeek.ContainsKey(aliasTitle))
                    {
                        if (lastWeek[aliasTitle] != entry.LastWeek &&
                            !LastWeekRankExceptions.Contains((dateTime, aliasTitle)))
                        {
                            throw new Exception("Did not match last week's rank.");
                        }
                    }

                    var oldInfo = Info[aliasTitle];

                    if (oldInfo.Count + 1 != entry.WeeksOnList)
                    {
                        if (!MultiWeeksOnList.Contains((dateTime, entry.Title)))
                        {
                            throw new Exception("Weeks on list is incorrect.");
                        }

                        Console.WriteLine(entry.Title);
                        Console.WriteLine(oldInfo.Last);
                        Console.WriteLine(dateTime);
                        Console.WriteLine();
                    }

                    oldInfo.Count = entry.WeeksOnList;
                    oldInfo.Points += points;
                    oldInfo.Last = dateTime;
                }
                else
                {
                    if (entry.WeeksOnList != 1 && !FirstWeeksOnList.Contains((dateTime, entry.Title)))
                    {
                        throw new Exception("Weeks on list is incorrect.");
                    }
                    if (entry.LastWeek.HasValue) throw new Exception("Weeks on list is incorrect.");
                    Info[aliasTitle] = new Info
                    {
                        Count = entry.WeeksOnList,
                        Points = points,
                        Last = dateTime,
                    };
                }
            }
        }

        public static readonly Dictionary<(DateTime dateTime, int pageNum), string> ReplacementText = new Dictionary<(DateTime dateTime, int pageNum), string>
        {
            {(new DateTime(1938, 4, 18), 1), @" 
Uif!Ofx!Zpsl!Ujnft!Cftu!Tfmmfs!Mjtu!
This April 18, 1938 Last Weeks 
Week Fiction Week On List 
    
1 THE CITADEL, by A. J. Cronin. (Little, Brown.) 1 31 
    
2 THE YEARLING, by Marjorie Kinnan Rawlings. (Scribner.) 8 2 
    
3 ACTION AT AQUILA, by Hervey Allen. (Farrar & Rinehart.) 2 6 
    
4 HOPE OF HEAVEN, by John O'Hara. (Harcourt, Brace.) 6 3 
    
5 THE MORTAL STORM, by Phyllis Bottome. (Little, Brown.) -- 1 
    
6 NORTHWEST PASSAGE, by Kenneth Roberts. (Doubleday, Doran.) 3 41 
    
7 THIS PROUD HEART, by Pearl S. Buck. (Reynal & Hitchcock.) 5 8 
    
8 THE RAINS CAME, by Louis Bromfield. (Harper.) 7 25 
 
Hawes Publications  www.hawes.com "},

            {(new DateTime(1943, 5, 17), 2), @" 
Uif!Ofx!Zpsl!Ujnft!Cftu!Tfmmfs!Mjtu!
This May 17, 1943 Last Weeks 
Week Non-Fiction Week On List 
    
1 ONE WORLD, by Wendell L. Willkie. (Simon & Schuster.) 1 5 
    
BETWEEN THE THUNDER AND THE SUN, by Vincent Sheean. (Random 
2 2 7 
House.) 
    
3 ON BEING A REAL PERSON, by Harry Emerson Fosdick. (Harper.) 3 9 
    
4 JOURNEY AMONG WARRIORS, by Eve Curie. (Doubleday, Doran.) -- 1 
    
5 GEORGE WASHINGTON CARVER, by Rackham Holt. (Doubleday, Doran.) 5 3 
 
Hawes Publications  www.hawes.com "},

            {(new DateTime(1946, 12, 29), 2), @" 
Uif!Ofx!Zpsl!Ujnft!Cftu!Tfmmfs!Mjtu!
This December 29, 1946 Last Weeks 
Week Non-Fiction Week On List 
    
1 PEACE OF MIND, by Joshua Loth Liebman. (Simon & Schuster.) 1 38 
    
2 THE EGG AND I, by Betty MacDonald. (Lippincott.) 2 62 
    
3 THE PLOTTERS, by John Roy Carlson. (Dutton.) 3 4 
    
4 THE ROOSEVELT I KNEW, by Frances Perkins. (Viking.) 4 6 
    
5 AS HE SAW IT, by Elliott Roosevelt. (Duell, Sloan & Pearce.) 5 11 
    
6 NOT SO WILD A DREAM, by Eric Sevareid. (Knopf.) 7 9 
    
7 BALZAC, by Stephan Zweig. (Viking.) 8 2 
    
8 HAPPY THE LAND, by Louise Dickinson Rich. (Lippincott.) 12 3 
    
9 DAHL'S BOSTON, by Francis W. Dahl and Charles W. Morton. (Little, Brown.) -- 2 
    
10 SO THIS IS PEACE, by Bob Hope. (Simon & Schuster.) 6 7 
    
THE LOWELLS AND THEIR SEVEN WORLDS, by Ferris Greenslet. (Houghton 
11 -- 10 
Mifflin.) 
    
THUNDER OUT OF CHINA, by Theodore H. White and Annalee Jacoby. 
12 9 6 
(Sloane.) 
    
13 THE HAPPY PROFESSION, by Ellery Sedgwick. (Little, Brown.) 11 10 
    
14 HIROSHIMA, by John Hershey. (Knopf.) 13 5 
    
15 I CHOSE FREEDOM, by Victor A. Kravchenko. (Scribner.) 16 33 
    
16 WHERE ARE WE HEADING? by Sumner Welles. (Harper.) -- 8 
 
Hawes Publications  www.hawes.com "}
        };

        public static Result MakeRequest(DateTime dateTime)
        {
            using var pdfReader = new PdfReader(Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "NYTimesPdfs", SubPath(dateTime)));
            using var pdfDocument = new PdfDocument(pdfReader);

            BestSellerList fictionList = null;
            BestSellerList nonFictionList = null;
            if (pdfDocument.GetNumberOfPages() != 2)
            {
                throw new Exception("More than 2 pages!");
            }
            for (var pageNum = 1; pageNum <= pdfDocument.GetNumberOfPages(); pageNum++)
            {
                var pageText = ReplacementText.ContainsKey((dateTime, pageNum))
                    ? ReplacementText[(dateTime, pageNum)]
                    : PdfTextExtractor.GetTextFromPage(pdfDocument.GetPage(pageNum));
                var bestSellerList = TextParser.Parse(pageText);
                switch (bestSellerList.Genre)
                {
                    case Genre.Fiction when fictionList != null:
                        throw new Exception("Fiction list should be null.");
                    case Genre.Fiction:
                        fictionList = bestSellerList;
                        break;
                    case Genre.NonFiction when nonFictionList != null:
                        throw new Exception("Non-fiction list should be null.");
                    case Genre.NonFiction:
                        nonFictionList = bestSellerList;
                        break;
                    default:
                        throw new Exception("Gotta be one or the other.");
                }
            }

            return new Result
            {
                FictionList = fictionList,
                NonFictionList = nonFictionList,
            };
        }
    }
}
