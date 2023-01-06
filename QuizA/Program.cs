﻿using QuizA.HttpCalls;
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
            bool retakeQuiz = true;
            String userResponse = "Y";

            while (retakeQuiz && userResponse == "Y")
            {

                //This prints out the quiz instruction before starting
                Instructions.QuizInstructions();

                Helpers.ColoredMessage colored = new Helpers.ColoredMessage();

                colored.printColoredMessages("\n \t\t Summary of your selections are as follows :", ConsoleColor.White);

                Console.WriteLine($"\n \t\t Question Category: {Instructions.CategoryName}" +
                    $"\n \t\t Number of Questions Selected: {Instructions.Amount}" +
                    $"\n \t\t Question Difficulty Selected: {Instructions.Difficulty}" +
                    $"\n \t\t Type of Question Selected: {Instructions.Type}");

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
                else
                {

                    type = "";

                }


                //use the HttpApi class and pass in the values the user supplied to make the APi call to the remote end'
                //colored.printColoredMessages($"\n Please Wait Fetching Data................. :", ConsoleColor.Green);

                String loadingMessage = HttpApi.Quizes == null ? "Please Wait Fetching Data................. :" : "Fetched data!";

                colored.printColoredMessages($"\n {loadingMessage.ToUpper()}", ConsoleColor.Green);

                HttpApi.RunAsync(category, type, amount, difficulty).GetAwaiter().GetResult();


                if (HttpApi.Quizes.Count > 0)
                {
                    //colored.printColoredMessages($"\n Quizes Fetched length: {HttpApi.Quizes.Count}", ConsoleColor.Green);
                }
                else
                {
                    colored.printColoredMessages($"\n \t\t\t\t  No quizes found, please rechoose your options or\n\n \t\t\t\t  reduce the number of questions requested!", ConsoleColor.Red);

                }


                colored.printColoredMessages($"\n \t\t\t\t  Would you like to retake the quiz? , [Y/N] Y for Yes, N for No:  ", ConsoleColor.White, false);



                userResponse = Console.ReadLine().ToUpper();

                if (userResponse == "Y")
                {
                    retakeQuiz = true;
                    TimeCounter.resetTimer();

                }
                else
                {
                    retakeQuiz = false;
                    colored.printColoredMessages($"\n \t\t\t\t Thank you for your time taking the Quiz." +
                        $"\n\n \t\t\t\t Quitting Quiz Application now, Bye!!!", ConsoleColor.DarkYellow);
                    Console.ReadKey();
                    break;
                }




            }





        }  //main function ends




    }
}
