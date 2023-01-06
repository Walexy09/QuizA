using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizA.Helpers
{
    public class Logger
    {

        public static void WriteUserLogs(String userInfo, bool newLine = true)
        {
            // Include logic for saving User Quiz information taken, like subject, date taken, scores, etc
            // Get the absolute path to the saved file, which is saved in the Log folder

            string logFile = @"QuizRecord.txt";

            logFile = Path.GetFullPath(logFile);

            //Path.GetFullPath(logFile);
            //Source from https://learn.microsoft.com/en-us/dotnet/api/system.io.path.getfullpath?view=netframework-4.8//
            //Topic:Path.GetFullPath Method 
            Helpers.ColoredMessage colored = new Helpers.ColoredMessage();

            // Open the log file for append and write the log
            StreamWriter sw = new StreamWriter(logFile, true);

            sw.WriteLine("**********User Quiz Taken On {0} *************", DateTime.Now);

            if (newLine)
            {
                sw.WriteLine(userInfo);

            }
            else
            {
                sw.Write(userInfo);

            }

            sw.WriteLine("**********END****************", DateTime.Now);
            sw.Close();
        }

        public static bool pathExist()
        {

            string logFile = @"QuizRecord.txt";

            logFile = Path.GetFullPath(logFile);

            if (!File.Exists(logFile))
            {
                // File does not exist, return an empty list
                return false;
            }
            else
            {

                return true;
            }


        }

        //This will read the saved data
        public static List<string> SearchFileForText(string searchText)
        {
            string logFile = @"QuizRecord.txt";

            logFile = Path.GetFullPath(logFile);

            if (!File.Exists(logFile))
            {
                // File does not exist, return an empty list
                return new List<string>();
            }

            List<string> lines = new List<string>();

            using (StreamReader reader = new StreamReader(logFile))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains(searchText))
                    {
                        lines.Add(line);
                        while ((line = reader.ReadLine()) != null && line != "****************")
                        {
                            lines.Add(line);
                        }
                    }
                }
            }

            return lines;
        }


        public static void DisplayLines(String searchText)
        {

            List<string> lines = SearchFileForText(searchText);

            if (lines.Count > 0)
            {
                Console.WriteLine("  \n  \t\t\t\t\t Congrats, Your quiz results were found on these days:");
                foreach (string line in lines)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($" \n  \t\t\t\t\t{line}");
                    Console.ResetColor();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(" \n  \t\t\t\t\t No Quiz history were found in the file!!.");
                Console.ResetColor();
            }

        }

    }
}
