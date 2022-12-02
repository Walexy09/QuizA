using QuizA.HttpCalls;
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

            Helpers.ColoredMessage colored = new Helpers.ColoredMessage();

            colored.printColoredMessages("\n Summary of your selections are as follows :", ConsoleColor.DarkBlue);

            Console.WriteLine($"\n Question Category: {Instructions.Category}" +
                $"\n Number of Questions Selected: {Instructions.Amount}" +
                $"\n Question Difficulty Selected: {Instructions.Difficulty}" +
                $"\n Type of Question Selected: {Instructions.Type}");

            String category = Instructions.Category;
            String amount = Instructions.Amount;

            String difficulty = Instructions.Difficulty;

            String type = Instructions.Type;
            if (type == "Multiple Choice")
            {
                type = "multiple";
            }
            else if (type == "True/False")
            {
                type = "boolean";
            }
            else {

                type = "";
            
            }


                //use the HttpApi class and pass in the values the user supplied to make the APi call to the remote end'
                //colored.printColoredMessages($"\n Please Wait Fetching Data................. :", ConsoleColor.Green);

                String loadingMessage = HttpApi.Quizes == null ? "Please Wait Fetching Data................. :" : "Fetched data!";

            colored.printColoredMessages($"\n {loadingMessage.ToUpper()}", ConsoleColor.Green);

            HttpApi.RunAsync(category, type, amount, difficulty).GetAwaiter().GetResult();

          
            if (HttpApi.Quizes.Count > 0)
            {
                colored.printColoredMessages($"\n Quizes Fected length: {HttpApi.Quizes.Count}", ConsoleColor.Green);
            }
            else
            {
                colored.printColoredMessages($"\n Quizes Fetched is empty and does not contain anything", ConsoleColor.Red);

            }


            Console.ReadKey();


        }




    }
}
