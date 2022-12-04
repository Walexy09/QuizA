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

            //Console.WriteLine($"File location is situated here: logFile: {Path.GetFullPath(logFile)}");

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
            else {
                sw.Write(userInfo);

            }

            sw.WriteLine("**********END****************", DateTime.Now);
            sw.Close();
        }

        //This will read the saved data
        public static string ReadUserLogs() 
        {



            return "";
        
        }
    }
}
