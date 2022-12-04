using QuizA.HttpCalls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace QuizA
{
    //This class contains the instruction for playing the quiz game
    //This function prints the instruction for the Quiz Game.

    /// <summary>
    /// https://opentdb.com/api.php?amount=10&category=22&difficulty=easy&type=multiple
    /// </summary>

    public class Instructions
    {
        static public string Category { get; set; }
        static public string Amount { get; set; }
        static public string Difficulty { get; set; }
        static public string Type { get; set; }
        static public string CategoryName { get; set; }

        //Added this method on 23/11/22: Used to validate input from users. If user supplies invalid entry, it will keep
        //prompting them to enter they enter the right value before they can proceed. 
        static public int ValidateUserInputs(
            String userInput,
            int minAllowedValue,
            String[] itemArray,
            String arrayName
            )
        
        {
            int validatedOutput;

            Helpers.ColoredMessage colored = new Helpers.ColoredMessage();

            //This checks if the input is not empty
            while ((!int.TryParse(userInput, out validatedOutput)) || (String.IsNullOrEmpty(userInput)))
            {    
                colored.printColoredMessages($"\n You supplied a non-existent {arrayName} number or invalid entry" +
                       $"\n Please select a valid {arrayName} number from the above listed and  try again  !!", ConsoleColor.Red, false);

                userInput = Console.ReadLine();

            }

            //This checks if the chosen user value is within the allowable range or not.
            //If not, it asks the user to rechoose his choice!
            while ((validatedOutput < minAllowedValue) || (validatedOutput > itemArray.Length))
            {
                colored.printColoredMessages("\n You supplied an out of range value, " +
                    $"please reselect your {arrayName} choice!:   ", ConsoleColor.Red, false);
                //this asks the user to re-enter his choice if its not within accepted range
                if (int.TryParse(Console.ReadLine(), out validatedOutput))

                {
                    //This code breaks out of the loop if an out of range value is selected and repeats the outer code.
                    if ((validatedOutput < minAllowedValue) && (validatedOutput > itemArray.Length))
                    {
                        string Mssg = $"ERROR!!!! You selected: {validatedOutput}, which is not a valid entry, choose again!!";

                        colored.printColoredMessages(Mssg, ConsoleColor.Red);

                        break;
                    }
                   

                }

            }

            return validatedOutput;


        }

        static public void QuizInstructions()
        {
            Helpers.ColoredMessage colored = new Helpers.ColoredMessage();

            

            String author = "Joshua Ajayi Adewale, N1048457";
            String version = "1.0.0";
            string appName = "Quiza Appplication";
            string requiresInternet = $" Nb:Requires internet connectivity to fetch quizes and answers!!";
            String message = $" Welcome to the {appName.ToUpper()}, version: {version}. Developed by {author.ToUpper()}." +
                $"\n\n***************************************************Instructions********************************************* " +
                $" \n\n You would be presented with some options to select from, these determines the type of quiz questions you get." +
                $" In turn, a number of question is returned to you after which you must type start to begin the question with timer." +
                $" You must answer within a specified time limit." +
                $"\n\n The user must select: \n (i) A Question Category \n (ii) Difficulty Level \n (iii) Question Type" +
                $" i.e: multiple choice or True/False based questions \n (iv) The number of questions to answer!       ";

            colored.printColoredMessages("\n***************************************************Instructions********************************************* ");
            colored.printColoredMessages(message);
            colored.printColoredMessages(requiresInternet.ToUpper(), ConsoleColor.DarkBlue);

            String[] category = { "General Knowledge", "Entertainment:Books", "Entertainment: Film", "Entertainment: Music",
                "Entertainment: Musicals & Theatres", "Entertainment: Television", "Entertainment: Video Games",
                "Entertainment: Board Games", "Science & Nature", "Science: Computers", "Science: Mathematics","Mythology",
                "Sports", "Geography", "History", "Politics", "Art", "Celebrities", "Animals", "Vehicles", "Entertainment: Comics",
                "Science: Gadgets", "Entertainment: Japanese Anime & Manga", "Entertainment: Cartoons & Animations"

            };

            //This will be the equivalent of the category chosen. This will be passed into the REST Api for category query
            int[] categoryNo = { 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 };


            String[] difficulties = { "Easy", "Medium", "Hard" };

            String[] questionTypes = { "Multiple Choice", "True/False", "Any" };

            const int minCategorySelected = 1;

            string categorySelected;
            int validatedCategoryNumber;
            int categoryNoValue = 0;
            int questionAmount = 10;
            const int minQuestionNo = 10;
            const int maxQuestionNo = 50;
            int difficultyLevelSelected;
            int questionTypeSelected;

            string catMssg = "These are the categories to select from. Please choose a category by selecting the appropriate number";

            colored.printColoredMessages(catMssg, ConsoleColor.Green);

            colored.printColoredMessages("*****************CATEGORIES****************************", ConsoleColor.Green);

            for (int cat = 0; cat < category.Length; cat++)
            {
                string indexMessage = $"({cat + 1}). {category[cat]}               ";

                colored.printColoredMessages(indexMessage, ConsoleColor.Yellow, false);

            }

            categorySelected = Console.ReadLine();

            //validate the chosen category to be within 1 and 24 and also its a valid entry. uing int.TryParse to validate
            //it must be a valid entry and must not be an empty character
            try
            {
              

                validatedCategoryNumber =  ValidateUserInputs(categorySelected, minCategorySelected, category, "category");


                if (validatedCategoryNumber >= minCategorySelected && validatedCategoryNumber <= category.Length)
                {
                    for (int cat = 0; cat < category.Length; cat++)
                    {
                        for (int catNo = 0; catNo < categoryNo.Length; catNo++)
                        {

                            if (cat == catNo)
                            {
                                categoryNoValue = categoryNo[validatedCategoryNumber - 1];
                            }
                        }

                    }

                }

                string successMssg = $"You selected: {category[validatedCategoryNumber - 1]} Equivalet to value number: {categoryNoValue}";

                //Used to send the name of the chosen category to the HttpApi class for loggin purpose
                CategoryName = category[validatedCategoryNumber - 1];

                colored.printColoredMessages(successMssg, ConsoleColor.Green);

                ///////////////////////////////////////////////////////////////////////////////////
                /////For Question Type selected//////////////////////////////////////////

                colored.printColoredMessages("\n Please select your question type from amongst the below !  ");

                for (int questionType = 0; questionType < questionTypes.Length; questionType++)
                {

                    string indexMessage = $"({questionType + 1}). {questionTypes[questionType]} : ";

                    colored.printColoredMessages(indexMessage, ConsoleColor.Yellow, false);

                }

                string selectedQuestionType = Console.ReadLine();

                questionTypeSelected = ValidateUserInputs(selectedQuestionType, minCategorySelected, questionTypes, "questionType");

                string successQuestionTypeMssg = $"You selected: {questionTypes[questionTypeSelected - 1]} Question Type!! ";

                colored.printColoredMessages(successQuestionTypeMssg, ConsoleColor.Green);

                ///////////////////////////////////////////////////////////////////////////////////
                /////For Question Difficulty level selected//////////////////////////////////////////
                ///

                colored.printColoredMessages("\n Please select the level of question difficulty you would like to answer, i.e," +
                    "\n how difficult should the quiz questions be?  :");

                for (int difficulty = 0; difficulty < difficulties.Length; difficulty++)
                {

                    string indexMessage = $"({difficulty + 1}). {difficulties[difficulty]} : ";

                    colored.printColoredMessages(indexMessage, ConsoleColor.Yellow, false);

                }
                string selectedQuestionDifficulty = Console.ReadLine();

                difficultyLevelSelected = ValidateUserInputs(selectedQuestionDifficulty, minCategorySelected, difficulties, "Difficulty Level");

                string successQuestionDifficultyMssg = $"You selected: {difficulties[difficultyLevelSelected - 1]} Difficulty Level!! ";

                colored.printColoredMessages(successQuestionDifficultyMssg, ConsoleColor.Green);


                ///////////////////////////////////////////////////////////////////////////////////
                /////For Question Number of Questions to answe selected, defaults to 20//////////////////////////////////////////
                ///


                colored.printColoredMessages("\n Please select the number of questions difficulty you would like to answer, i.e," +
                    "\n how many quiz questions be presented to you. Maximum is 50 while minimun should be 20?  :", ConsoleColor.White, false);

                int validatedAmountOfQuestions;

                string userSelectedQuizNo = Console.ReadLine();

                //while supplied input is not an interger or its empty, it will ask input to be supplied again!
                while ( !(int.TryParse(userSelectedQuizNo, out validatedAmountOfQuestions))
                    && String.IsNullOrEmpty(userSelectedQuizNo) || ( validatedAmountOfQuestions < minQuestionNo) 
                    || (validatedAmountOfQuestions > maxQuestionNo)
                    ) 
                {
                    string Mssg = $"ERROR!!!! You selected: {validatedAmountOfQuestions} questions to answer, which is not a valid entry, choose again!!";

                    string questionMaxMssg = $"You selected {validatedAmountOfQuestions} questions, {maxQuestionNo} is the maximum allowed!! ";

                    string questionMinMssg = $"You selected {validatedAmountOfQuestions} questions, {minQuestionNo} is the mimimum allowed!! ";

                    string selectedShownMssg;

                    if (validatedAmountOfQuestions < minQuestionNo)
                    {

                        selectedShownMssg = questionMinMssg;

                        colored.printColoredMessages(selectedShownMssg, ConsoleColor.Red,false);
                    }
                    else 
                    {
                        selectedShownMssg = questionMaxMssg;

                        colored.printColoredMessages(selectedShownMssg, ConsoleColor.Red, false);

                    }

                    colored.printColoredMessages(Mssg, ConsoleColor.Red, false);

                    userSelectedQuizNo = Console.ReadLine();

                }

                questionAmount = validatedAmountOfQuestions;

                string successQuestionAmountMssg = $"You selected: {questionAmount} Number of Questions to answer:  ";

                colored.printColoredMessages(successQuestionAmountMssg, ConsoleColor.Green, false);


                /*colored.printColoredMessages("\n Summary of your selections are as follows :", ConsoleColor.DarkBlue);*/



                /*colored.printColoredMessages($"(1).Question Category selected : {category[validatedCategoryNumber - 1]}", ConsoleColor.Green, false);
                colored.printColoredMessages($"(2).Question Type selected : {questionTypes[questionTypeSelected - 1]}", ConsoleColor.Green, false);
                colored.printColoredMessages($"(3).Question difficulty selected : {difficulties[difficultyLevelSelected - 1]}", ConsoleColor.Green, false);
                colored.printColoredMessages($"(4).Amount of questions to answer : {questionAmount}", ConsoleColor.Green, false);
*/
                //set all the properties of the instruction classes
                Category = categoryNoValue.ToString();
                //category[validatedCategoryNumber - 1];
                Type = questionTypes[questionTypeSelected - 1];
                Amount = questionAmount.ToString();
                Difficulty = difficulties[difficultyLevelSelected - 1];

            }
            catch (Exception error)
            {
                colored.printColoredMessages($"{error.Message}!", ConsoleColor.Red, false);
            }



            //Console.ReadKey();






        }



    }

}
