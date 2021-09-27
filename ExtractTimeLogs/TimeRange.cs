using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ExtractTimeLogs
{
    public static class TimeRange
    {
        public static async Task ExtractFolderAsync(string fromPath, string toPath, DateTime startTime, DateTime endTime)
        {
            Directory.CreateDirectory(toPath);

            var files = Directory.GetFiles(fromPath);
            foreach (var file in files)
            {
                var lines = await GetLinesAsync(file, startTime, endTime);

                var fileName = Path.GetFileName(file);

                if (!lines.Any(t => !string.IsNullOrWhiteSpace(t)))
                {
                    Console.WriteLine($"Skip {fileName}");
                    continue;
                }

                var outputPath = Path.Combine(toPath, fileName);
                await File.WriteAllLinesAsync(outputPath, lines);
            }
        }

        private static async Task<string[]> GetLinesAsync(string file, DateTime startTime, DateTime endTime)
        {
            var lines = await File.ReadAllLinesAsync(file);

            var timeTexts = lines.Select(t => {
                var match = Regex.Match(t, @"\[(?<time>\d{4}/\d\d/\d\d \d\d:\d\d):\d\d.\d{3}\]");
                if (!match.Success)
                {
                    return (Time: default, Text: t);
                }

                var time = DateTime.Parse(match.Groups["time"].Value);
                return (Time: time, Text: t);
            });

            var timedTexts = timeTexts.Run((s, t) =>
                t.Time != default
                    ? (t.Time, t)
                    : (s, (Time: s, t.Text)),
                default(DateTime));

            return timedTexts
                .Where(t => t.Time >= startTime && t.Time <= endTime)
                .Select(t => t.Text)
                .ToArray();
        }
    }
}
