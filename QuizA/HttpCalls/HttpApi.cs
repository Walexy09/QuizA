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

                    Console.WriteLine($" Quizes length obtained from HttpApi class : {Quizes.Count}");
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
                        }
                        else 
                        {
                            colored.printColoredMessages($"{validatedChoice} is not a valid response", ConsoleColor.Red);

                        }




                        /*if (int.TryParse(choice, out validatedChoice))
                        {

                            if (validatedChoice >= minChoiceAns && validatedChoice < maxChoiceAns)
                            {
                                userAnswer[i] = validatedChoice;
                            }

                        }
                        else 
                        {
                            colored.printColoredMessages($"{choice} is not a valid response, please select between 1 to 4 ", ConsoleColor.Red);


                        }*/
                       
                        //Add to repo: The correct answer was added to the list of incorrect ansers to choose from amd shuffled

                    }


                }

               

            }
        }
    }
}
