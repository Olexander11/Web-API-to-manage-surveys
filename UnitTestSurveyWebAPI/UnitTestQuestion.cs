using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SurveyWebAPI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using Xunit;

namespace UnitTestSurveyWebAPI.Tests
{
    public class UnitTestQuestion
    {

        HttpClient httpClient;


        public UnitTestQuestion()
        {
            string curDir = Directory.GetCurrentDirectory();
            var builder = new ConfigurationBuilder()
            .SetBasePath(curDir)
            .AddJsonFile("appsettings.json");
            var server = new TestServer(new WebHostBuilder().UseContentRoot(curDir).UseConfiguration(builder.Build()).UseStartup<SurveyWebAPI.Startup>());
            httpClient = server.CreateClient();

        }

              
        [Fact]
        public async void TestQuestion()
        {
            //Create question: [POST]/question
            Answer answer1 = new Answer()
            {
                Name = "Test answer1"
            };

            Answer answer2 = new Answer()
            {
                Name = "Test answer2"
            };

            Answer answer3 = new Answer()
            {
                Name = "Test answer3"
            };

            Question question1 = new Question()
            {
                Name = "Test question 1",
                Description = "Test question description1",
                QuestionAnswers = new List<Answer> { answer1, answer2 }
            };

            Question question2 = new Question()
            {
                Name = "Test question 2",
                Description = "Test question description2",
                QuestionAnswers = new List<Answer> { answer3, answer2 }
            };


            HttpContent content1 = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(question1), Encoding.UTF8, "application/json");
            HttpContent content2 = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(question2), Encoding.UTF8, "application/json");
            var request1 = await httpClient.PostAsync("api/question", content1);
            Assert.Equal(System.Net.HttpStatusCode.OK, request1.StatusCode);

            var request2 = await httpClient.PostAsync("api/question", content2);
            Assert.Equal(System.Net.HttpStatusCode.OK, request2.StatusCode);

            var responce1 = request1.Content.ReadAsStringAsync().Result; ;

            var responce2 = request2.Content.ReadAsStringAsync().Result;
            Assert.NotNull(responce1);
            Assert.NotNull(responce2);

            //List of questions: [GET]/questions
            var questions = await httpClient.GetAsync("api/questions");
            Assert.Equal(System.Net.HttpStatusCode.OK, questions.StatusCode);
            var content = questions.Content.ReadAsStringAsync().Result; 
            Assert.NotNull(content);
            Assert.Contains("Test question 2", content);
            Assert.Contains("Test question description1", content);


            List<Question> list =  JsonConvert.DeserializeObject<List<Question>>(content);
          
            foreach(Question item in list)
            {
                if (item.Name == "Test question 1") question1.Id = item.Id;
                if (item.Name == "Test question 2") question2.Id = item.Id;
            }

            //Edit question: [PUT]/question/{id}

            question1.Name = "Changed new name";
            question1.Description = "Changed new description";

            HttpContent content3 = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(question1), Encoding.UTF8, "application/json");
            var request3 = await httpClient.PutAsync("api/question/" + question1.Id, content3);
            Assert.Equal(System.Net.HttpStatusCode.OK, request3.StatusCode);

            //Get info for a single question: [GET]/question/{id}
            var request4 = await httpClient.GetAsync("api/question/" + question1.Id);
            Assert.Equal(System.Net.HttpStatusCode.OK, request4.StatusCode);
            var responce3 = request4.Content.ReadAsStringAsync().Result;
            Question question3 = JsonConvert.DeserializeObject<Question>(responce3);
            Assert.Equal(question1.Name, question3.Name);
            Assert.Equal(question1.Description, question3.Description);

            //Delete question: [DELETE]/question/{id}
            var request5 = await httpClient.DeleteAsync("api/question/" + question2.Id);
            Assert.Equal(System.Net.HttpStatusCode.OK, request5.StatusCode);

            var request6 = await httpClient.GetAsync("api/question/" + question2.Id);
            Assert.Equal(System.Net.HttpStatusCode.NotFound, request6.StatusCode);

            var request7 = await httpClient.DeleteAsync("api/question/" + question1.Id); //Delete all created items
        }

    }
}
