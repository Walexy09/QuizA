using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizA
{
    class Program
    {
        static void Main(string[] args)
        {
            //This prints out the quiz instruction before starting
            Instructions.QuizInstructions();

            Console.WriteLine($"\n Question Category: {Instructions.Category}" +
                $"\n Number of Questions Selected: {Instructions.Amount}" +
                $"\n Question Difficulty Selected: {Instructions.Difficulty}" +
                $"\n Type of Question Selected: {Instructions.Type}");

            Console.ReadKey();


        }

        
        
        
    }
}
