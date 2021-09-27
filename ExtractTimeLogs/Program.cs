using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExtractTimeLogs
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var arguments = new Queue<string>(args);
            
            var fromPath = GetFromPath(arguments);

            var startTime = GetTime(arguments, "starting");
            var endTime = GetTime(arguments, "ending");

            var toPath = GetToPath(arguments, fromPath, startTime, endTime);

            await TimeRange.ExtractFolderAsync(fromPath, toPath, startTime, endTime);
        }

        private static string GetFromPath(Queue<string> arguments)
        {
            if (arguments.TryDequeue(out var argument))
            {
                return argument;
            }
            else
            {
                Console.Write("The path of folder? ");
                return Console.ReadLine();
            }
        }

        private static DateTime GetTime(Queue<string> arguments, string name)
        {
            if (arguments.TryDequeue(out var argumentText))
            {
                return DateTime.Parse(argumentText);
            }
            else
            {
                Console.Write($"The {name} time (yyyy/MM/dd HH:mm)? ");
                var readText = Console.ReadLine();

                return DateTime.Parse(readText);
            }
        }

        private static string GetToPath(Queue<string> arguments, string fromPath, DateTime fromTime, DateTime toTime)
        {
            if (arguments.TryDequeue(out var argument))
            {
                return argument;
            }
            else
            {
                return $"{fromPath} {fromTime:HHmm}-{toTime:HHmm}";
            }
        }
    }
}
