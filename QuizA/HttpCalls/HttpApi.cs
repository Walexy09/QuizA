using System;
using System.Net;
using System.Net.Http;
using QuizA.models;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;

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


        static public async Task RunAsync(String category, String type, String amount, String difficulty)
        {
            using (var client = new HttpClient())
            {
                String url = "https://opentdb.com/api.php";
                String url2 = "https://opentdb.com/api.php?amount=20&category=18&difficulty=easy&type=multiple";
                client.BaseAddress = new Uri(url);
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth","xyz");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Console.WriteLine("Get");
                /*HttpResponseMessage response = await client.GetAsync($"{url}?amount={amount}" +
                    $"&category={category}&" +
                    $"difficulty={difficulty}&" +
                    $"type={type}");*/
                HttpResponseMessage response = await client.GetAsync(url2);

                Helpers.ColoredMessage colored = new Helpers.ColoredMessage();

                //colored.printColoredMessages($"\n Response obtained from Http decoding :{response}", ConsoleColor.DarkYellow);

                if (response.IsSuccessStatusCode)
                {
                    //ReadAsAsync<Quiz>();
                    //Quiz quiz = await response.Content.ReadAsAsync<Quiz>();

                    QuizResponse quizResponse = await response.Content.ReadAsAsync<QuizResponse>();

                    List<Quiz> QuizesObtained = quizResponse.response_code == 0 ? quizResponse.results : new List<Quiz>();


                    Quizes = QuizesObtained.Count > 0 ? QuizesObtained : new List<Quiz>{};

                    Console.WriteLine($" Quizes length obtained from HttpApi class : {Quizes.Count}");
                    for (var i = 0; i < Quizes.Count; i++) {

                        colored.printColoredMessages($"\n ({i+1}) Questions: {Quizes[i].question}", ConsoleColor.DarkYellow);
                        colored.printColoredMessages($" Options ", ConsoleColor.DarkYellow);
                        for (var ques = 0; ques < Quizes[i].incorrect_answers.Length; ques++) {

                            colored.printColoredMessages($"{ques + 1} {Quizes[i].incorrect_answers[ques]}", ConsoleColor.White,false);

                        }

                    }

                }

               

            }
        }
    }
}
