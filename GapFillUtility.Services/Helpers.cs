using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GapFillUtility.Services
{
    public static class Helpers
    {
        #region "Display Helpers"
        public static void DisplayHeader(string headerText, bool isHighlightStep = false)
        {
            string strDivider = "#####################################################";

            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.Black;

            if (isHighlightStep)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.White;
            }


            var displayText = (headerText.Length < strDivider.Length - 5) ? headerText.PadRight(strDivider.Length - 2, ' ') : headerText;

            Console.WriteLine($"\n{strDivider}".Trim());
            Console.WriteLine($"# {displayText}");
            Console.WriteLine($"{strDivider}");
            Console.ResetColor();
            Console.WriteLine("\n\n");

            SetConsoleHeader(headerText);
        }

        public static void SetConsoleHeader(string headerText)
        {
            Console.Title = $"GapFillContent - {headerText}";
        }

        public static void HighlightText(string title, string content, bool isValid = true)
        {
            if (!isValid)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.ForegroundColor = ConsoleColor.Black;
            }

            Console.Write($"{title}:".PadRight(15, ' '));
            Console.ResetColor();
            Console.Write($" {content}\n");
        }

        public static void HighlightText(string content, bool isValid = true)
        {
            if (!isValid)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.ForegroundColor = ConsoleColor.Black;
            }

            Console.Write($"{content}\n");
            Console.ResetColor();
        }

        public static void DisplayHelp<T>(ParserResult<T> result, IEnumerable<Error> errs)
        {
            var helpText = HelpText.AutoBuild(result, h =>
            {
                h.AdditionalNewLineAfterOption = false;
                h.Heading = "-----------------------------------\nGapFillContent\n-----------------------------------";
                h.Copyright = "Wolters Kluwer Health"; //change copyright text
                return HelpText.DefaultParsingErrorsHandler(result, h);
            }, e => e);
            Console.WriteLine(helpText);
        }
        #endregion


        public static string ExtractContentSetIdFromFile(this string srcPath)
        {
            var fileName = srcPath.Substring(0, srcPath.LastIndexOf('_'));

            // subtracting 36 because that is the length of the guid
            return fileName.Substring(fileName.Length - 36);
        }

        public static string ExtractContentSetId(this string srcPath)
        {
            // split url to individual items after the fullharvest key
            var fileResource = srcPath.Substring(srcPath.IndexOf("fullharvest")).Split('/')[6];

            // the last index of the '_' in the url item is the timestamp. to avoid 
            // unncessary truncation, we'll use this instead of the first implementation
            // where we check for the second instance of the '_' which isn't necessarily the
            // end of the contentsetid
            return fileResource.Substring(0, fileResource.LastIndexOf('_'));
        }


        public static string GetAssetFileName(this Uri uriPath)
        {
            return uriPath.LocalPath.Substring(uriPath.LocalPath.LastIndexOf('/') + 1);
        }

        public static string GetAssetFileName(this string uriPath)
        {
            var xpath = uriPath.Split('?')[0];

            return xpath.Substring(xpath.LastIndexOf('/') + 1);
        }

        public static string GetDateStampAudit()
        {
            return DateTime.Now.ToString("yyyy_MMdd_HH-mm-ss");
        }

        public static string GetUniqueDateStampAudit()
        {
            return $"{GetDateStampAudit()}_{Guid.NewGuid().ToString().Substring(0, 5)}";
        }
    }

    public static class Retry
    {
        public static async Task DoAsync(Func<Task> action, int maxTries = 3, double delayFactor = 1.0d)
        {
            for (var tryCount = 1; tryCount <= maxTries; tryCount++)
            {
                try
                {
                    await action().ConfigureAwait(false);
                    break;
                }
                catch (Exception)
                {
                    if (tryCount == maxTries) throw;

                    await Task.Delay(TimeSpan.FromSeconds(tryCount * delayFactor)).ConfigureAwait(false);
                }
            }
        }

        public static async Task<T> DoAsync<T>(Func<Task<T>> action, int maxTries = 3, double delayFactor = 1.0d)
        {
            var result = default(T);
            for (var tryCount = 1; tryCount <= maxTries; tryCount++)
            {
                try
                {
                    result = await action().ConfigureAwait(false);
                    break;
                }
                catch (Exception)
                {
                    if (tryCount == maxTries) throw;

                    await Task.Delay(TimeSpan.FromSeconds(tryCount * delayFactor)).ConfigureAwait(false);
                }
            }
            return result;
        }

        public static void Do(Action action, int maxTries = 3, int delayFactor = 1)
        {
            for (var tryCount = 1; tryCount <= maxTries; tryCount++)
            {
                try
                {
                    action();
                    break;
                }
                catch (Exception)
                {
                    if (tryCount == maxTries) throw;

                    Thread.Sleep(tryCount * delayFactor * 1000);
                }
            }
        }
    }
}
