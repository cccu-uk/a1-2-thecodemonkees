﻿using System.Text.Json;

namespace QuizzApp
{
    public class QuizEngine
    {
        public static List<Questions.Result> roots = new List<Questions.Result>();
        public static List<Questions.TriviaCategory> CategoriesList = new List<Questions.TriviaCategory>();

        public static async Task Main(string seed)
        {
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync("https://opentdb.com/api.php?" + seed);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                // Above three lines can be replaced with new helper method below
                // string responseBody = await client.GetStringAsync(uri);
                Questions.Root myDeserializedClass = JsonSerializer.Deserialize<Questions.Root>(responseBody);
                foreach (var x in myDeserializedClass.results)
                {
                    roots.Add(x);
                    Console.WriteLine(x.question);
                    Console.WriteLine(x.type.GetType());
                    Console.WriteLine(x.correct_answer);
                    Console.WriteLine(x.incorrect_answers);
                    Console.WriteLine(x.incorrect_answers[0]);
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
        }

        public static async Task GetCategoriesTask() // background worker/await msdn
        {
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            HttpClient client = new HttpClient();
            string responseBody = await client.GetStringAsync("https://opentdb.com/api_category.php");
            Questions.Root myDeserializedClass = JsonSerializer.Deserialize<Questions.Root>(responseBody);
            foreach (var x in myDeserializedClass.trivia_categories)
            {
                CategoriesList.Add(x);
            }
        }
    }
}
