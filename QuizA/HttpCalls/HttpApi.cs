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
        static public Quiz Quizes {get; set;}


        static async Task RunAsync(String category, String type, String amount, String difficulty)
        {
            using (var client = new HttpClient())
            {
                String url = " https://opentdb.com/api.php";
                client.BaseAddress = new Uri(url);
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth","xyz");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                Console.WriteLine("Get");
                HttpResponseMessage response = await client.GetAsync($"{url}?amount={amount}" +
                    $"&category={category}&" +
                    $"difficulty={difficulty}&" +
                    $"type={type}");
                if (response.IsSuccessStatusCode)
                {
                    //ReadAsAsync<Quiz>();
                    Quiz quiz = await response.Content.ReadAsAsync<Quiz>();

                    Quizes = quiz != null ? quiz: new Quiz { category = "Expecting quizes...."};

                    Console.WriteLine($" Quizes obtained: {Quizes}");
                    /*Console.WriteLine("Category: " + quiz.category + "Question," + ": " + quiz.question);*/
                }

               

            }
        }
    }
}
