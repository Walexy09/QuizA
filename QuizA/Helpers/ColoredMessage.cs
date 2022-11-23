using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizA.Helpers
{
    //This class has a method that helps to write colored messages to the screen. Ut takes 3 arguments: color to use,
    //message to print and newLine which means the message should be printed on a new line or not.
    public class ColoredMessage
    {


        public void printColoredMessages(String message, ConsoleColor color = ConsoleColor.White, bool newLine = true)
        {

            Console.ForegroundColor = color;

            if (newLine)
            {
                Console.WriteLine($"\n {message}");
            }
            else
            {
                Console.Write($"\n {message}");

            }

            Console.ResetColor();

        }
    }

    
}
