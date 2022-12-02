using System;
using System.Net;
using System.Net.Http;
using QuizA.models;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using QuizA.Helpers;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.WebUtilities;

namespace QuizA.HttpCalls
{
    //https://stackoverflow.com/questions/22627296/how-to-call-rest-api-from-a-console-application
    //Topic: How to call REST API from a console application?
    //Answered by: Sandeep Kumar, answered on Oct 3, 2019 at 7:00

    //This command at the Nuget console window installed the Microsoft.AspNetWebApi.Client and Newtonsoft.Json

    //Comand is : install-package Microsoft.AspNet.WebApi.Client

    class HttpApi
    {
        static public List<Quiz> Quizes {get; set;}

        static Random rng = new Random(); //used to shuffule the order of the option list



        static public async Task RunAsync(String category, String type, String amount, String difficulty)
        {
            using (var client = new HttpClient())
            {
               
                String url2 = "https://opentdb.com/api.php";
                client.BaseAddress = new Uri(url2);
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth","xyz");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                Helpers.ColoredMessage colored = new Helpers.ColoredMessage();

                var query = new Dictionary<string, string>()
                {
                   ["category"] = category,
                   ["amount"] = amount,
                   ["difficulty"] = difficulty.ToLower(),
                   ["type"] = type.ToLower()
            };

                //Source of this:https://makolyte.com/csharp-sending-query-strings-with-httpclient/
                //QueryHelpers.AddQueryString() (from Microsoft.AspNetCore.WebUtilities

                var urlEndPoint = QueryHelpers.AddQueryString(url2, query);

                colored.printColoredMessages($"Url Endpoint obtained: {urlEndPoint}", ConsoleColor.DarkMagenta);

                HttpResponseMessage response = await client.GetAsync(urlEndPoint);

                //colored.printColoredMessages($"\n Response obtained from Http decoding :{response}", ConsoleColor.DarkYellow);

                if (response.IsSuccessStatusCode)
                {
                    //ReadAsAsync<Quiz>();
                    //Quiz quiz = await response.Content.ReadAsAsync<Quiz>();



                    QuizResponse quizResponse = await response.Content.ReadAsAsync<QuizResponse>();

                    List<Quiz> QuizesObtained = quizResponse.response_code == 0 ? quizResponse.results : new List<Quiz>();

                    Quizes = QuizesObtained.Count > 0 ? QuizesObtained : new List<Quiz>{};

                    int[] userAnswer = new int[Quizes.Count];

                    const int minChoiceAns = 1;
                    const int maxChoiceAns = 5;

                    //use this to keep track of all chosen answers, both correct and incorrect
                    List<String> quizTakerChosenAnswersToQuestions = new List<string>();

                    List<bool> correctAnwersTracker = new List<bool>();

                    int correctAns = 0;

                    bool quizSummary = false;

                    Console.WriteLine($"\n Number of Quizes found from HttpApi class : {Quizes.Count}");
                    for (var i = 0; i < Quizes.Count; i++) {

                        colored.printColoredMessages($"\n ({i+1}) Questions: {Quizes[i].question}", ConsoleColor.DarkYellow);
                        colored.printColoredMessages($" Options ", ConsoleColor.DarkYellow);

                        Quizes[i].incorrect_answers.Add(Quizes[i].correct_answer);

                        Quizes[i].incorrect_answers =
                        Quizes[i].incorrect_answers.OrderBy(j => Guid.NewGuid()).ToList(); //This shuffles the option

                        for (var ques = 0; ques < Quizes[i].incorrect_answers.Count; ques++) {

                            colored.printColoredMessages($"{ques + 1}) {Quizes[i].incorrect_answers[ques]}", ConsoleColor.White, true);

                        }

                        Console.Write($"\n What is the appropriate answer to the question {i + 1} ?: ");
                        String choice = Console.ReadLine();
                        int validatedChoice;


                        while (!(int.TryParse(choice, out validatedChoice))) 
                        {
                            colored.printColoredMessages($"{choice} is not a valid response, please select between 1 to 4: ", ConsoleColor.Red, false);
                            choice = Console.ReadLine();


                        }

                        if (validatedChoice >= minChoiceAns && validatedChoice < maxChoiceAns)
                        {
                            userAnswer[i] = validatedChoice;

                            //check where the chosen answers are same as the correct answer, if true, add true to the correcAnswerTracker List,else add false
                            if (Quizes[i].incorrect_answers[validatedChoice - 1] == Quizes[i].correct_answer)
                            {
                                correctAnwersTracker.Add(true);

                            }
                            else 
                            {
                                correctAnwersTracker.Add(false);

                            }

                            //store the actual chosen answers into the quizTakerChosenAnswersToQuestions list
                            quizTakerChosenAnswersToQuestions.Add(Quizes[i].incorrect_answers[validatedChoice - 1]);
                        }
                        else 
                        {
                            colored.printColoredMessages($"{validatedChoice} is not a valid response", ConsoleColor.Red);

                        }

                      
                    }
                    for (int ans = 0; ans < correctAnwersTracker.Count; ans++) 
                    {

                        if (correctAnwersTracker[ans] == true) 
                        {

                            correctAns++;
                        }
                        
                    }


                    if (correctAnwersTracker.Count > 0) 
                    {
                        colored.printColoredMessages($"\n *******************************************************************************************************", ConsoleColor.Blue, true);


                        string header = "Your Quiz Score:";

                        colored.printColoredMessages($"\t\t\t\t\t {header.ToUpper()}", ConsoleColor.White, true);

                        string quizMessage = $"Correct Answers : {correctAns} \n \t\t\t\t\t Wrong Answers: {correctAnwersTracker.Count - correctAns}";

                        string percentMessage = $"Your percentage score: {((double)correctAns / (double)correctAnwersTracker.Count) * (double)100} %";

                        colored.printColoredMessages($"\t\t\t\t\t {quizMessage.ToUpper()} ", ConsoleColor.DarkGreen, true);

                        double scoreEarned = ((double)correctAns / (double)correctAnwersTracker.Count) * (double)100;

                        ConsoleColor colorEarned = scoreEarned >= 50d ? ConsoleColor.Green : ConsoleColor.Red;

                        colored.printColoredMessages($"\t\t\t\t\t {percentMessage.ToUpper()} ", colorEarned, true);

                        colored.printColoredMessages($"\n *******************************************************************************************************", ConsoleColor.Blue, true);

                    }
                    colored.printColoredMessages($"\n Would you like a detailed view of your performance on the quiz? This also shows the correct answer for each question Press Y for Yes, or N for No: ", ConsoleColor.DarkMagenta, false);

                    if (Console.ReadLine().ToUpper() == "Y")
                    {
                        quizSummary = true;

                        if (quizSummary)
                        {
                            for (var i = 0; i < Quizes.Count; i++)
                            {

                                colored.printColoredMessages($"\n Question  ({i + 1}): {Quizes[i].question}", ConsoleColor.DarkGray, true);
                                colored.printColoredMessages($"---------------------------------------------------------------------------------------", ConsoleColor.White, true);

                                for (int ans = 0; ans < quizTakerChosenAnswersToQuestions.Count; ans++) 
                                {
                                    if (i == ans) 
                                    {
                                        colored.printColoredMessages($"Your chosen answer is: { quizTakerChosenAnswersToQuestions[i]} ", ConsoleColor.DarkYellow);
                                        colored.printColoredMessages($"Correct Answer is: { Quizes[i].correct_answer} ", ConsoleColor.DarkYellow);

                                        colored.printColoredMessages($"VERDICT: ", ConsoleColor.DarkYellow, false);

                                        if (quizTakerChosenAnswersToQuestions[i] == Quizes[i].correct_answer)
                                        {
                                            colored.printColoredMessages($"You are correct !!", ConsoleColor.Green, false);

                                        }
                                        else 
                                        {
                                            colored.printColoredMessages($"You are Wrong !!", ConsoleColor.Red, false);

                                        }

                                    }
                                }
                                

                            }

                            }

                    }
             
                    

                }

               

            }
        }
    }
}
