using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SurveyWebAPI.Models;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using Xunit;

namespace UnitTestSurveyWebAPI.Tests
{
    public class UnitTestSurvey
    {

        HttpClient httpClient;
        

        public UnitTestSurvey()
        {
            string curDir = Directory.GetCurrentDirectory();
            var builder = new ConfigurationBuilder()
            .SetBasePath(curDir)
            .AddJsonFile("appsettings.json");
            var server = new TestServer(new WebHostBuilder().UseContentRoot(curDir).UseConfiguration(builder.Build()).UseStartup<SurveyWebAPI.Startup>());
            httpClient = server.CreateClient();

        }

        [Fact]
        public async void TestSurvey()
        {
            //List all surveys:  [POST]/survey
            Question question1 = new Question()
            {
                Name = "Test question 1",
                Description = "Description 1"
            };

            Question question2 = new Question()
            {
                Name = "Test question 2",
                Description = "Description 2"
            };

            Question question3 = new Question()
            {
                Name = "Test question 3",
                Description = "Description 3"
            };

            Survey survey1 = new Survey()
            {
                Name = "Test survey 1",
                Description = "Test survey description1",
                Questions = new List<Question> { question1, question3 }
            };

            Survey survey2 = new Survey()
            {
                Name = "Test survey 2",
                Description = "Test survey description2",
                Questions = new List<Question> { question2, question3 }
            };


            HttpContent content1 = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(survey1), Encoding.UTF8, "application/json");
            HttpContent content2 = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(survey2), Encoding.UTF8, "application/json");

            var request1 = await httpClient.PostAsync("api/survey", content1);
            Assert.Equal(System.Net.HttpStatusCode.OK, request1.StatusCode);

            var request2 = await httpClient.PostAsync("api/survey", content2);
            Assert.Equal(System.Net.HttpStatusCode.OK, request2.StatusCode);

            var responce1 = request1.Content.ReadAsStringAsync().Result; ;

            var responce2 = request2.Content.ReadAsStringAsync().Result;
            Assert.NotNull(responce1);
            Assert.NotNull(responce2);

            //List all surveys: [GET]/surveys
            var questions = await httpClient.GetAsync("api/surveys");
            Assert.Equal(System.Net.HttpStatusCode.OK, questions.StatusCode);
            var content = questions.Content.ReadAsStringAsync().Result;
            Assert.NotNull(content);
            Assert.Contains("Test survey 1", content);
            Assert.Contains("Test survey description2", content);


            List<Survey> list = JsonConvert.DeserializeObject<List<Survey>>(content);

            foreach (Survey item in list)
            {
                if (item.Name == "Test survey 1")
                    survey1.Id = item.Id;
                if (item.Name == "Test survey 2") survey2.Id = item.Id;
            }

            //Edit survey: [PUT]/survey/{id}

            survey1.Name = "Changed new survay name";
            survey2.Description = "Changed new survay description";

            HttpContent content3 = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(survey1), Encoding.UTF8, "application/json");
            var request3 = await httpClient.PutAsync("api/survey/" + survey1.Id, content3);
            Assert.Equal(System.Net.HttpStatusCode.OK, request3.StatusCode);

            //Get info for a single survey: [GET] / survey /{id}
            var request4 = await httpClient.GetAsync("api/survey/" + survey1.Id);
            Assert.Equal(System.Net.HttpStatusCode.OK, request4.StatusCode);
            var responce3 = request4.Content.ReadAsStringAsync().Result;
            Survey  survey3 = JsonConvert.DeserializeObject<Survey>(responce3);
            Assert.Equal(survey1.Name, survey3.Name);
            Assert.Equal(survey1.Description, survey3.Description);
            

            //  Delete survey: [DELETE] ]/survey/{id}
            var request5 = await httpClient.DeleteAsync("api/survey/" + survey2.Id);
            Assert.Equal(System.Net.HttpStatusCode.OK, request5.StatusCode);

            var request6 = await httpClient.GetAsync("api/survey/" + survey2.Id);
            Assert.Equal(System.Net.HttpStatusCode.NotFound, request6.StatusCode);

            var request31 = await httpClient.PutAsync("api/survey/" + survey2.Id, content3);
            Assert.Equal(System.Net.HttpStatusCode.NotFound, request31.StatusCode);

            //  Add question to list of the questions in survey: [POST] /survey/{id}
            Question question4 = new Question()
            {
                Name = "Test question 4",
                Description = "Test description 4"
            };
            survey1.Questions.Add(question4);
            HttpContent content5 = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(survey1), Encoding.UTF8, "application/json");
            var request7 = await httpClient.PutAsync("api/survey/" + survey1.Id, content5);
            Assert.Equal(System.Net.HttpStatusCode.OK, request7.StatusCode);
            var responce6 = request7.Content.ReadAsStringAsync().Result;
            Survey survey4 = JsonConvert.DeserializeObject<Survey>(responce6);
            Assert.NotNull(survey4.Questions.Find(x => x.Name == "Test question 4"));


            // Add question to list of the questions in survey: [POST] / surveyquestions /{ id}
            Question question5 = new Question()
            {
                Name = "Test question 5",
                Description = "Test description 5"
            };

            HttpContent content7 = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(question5), Encoding.UTF8, "application/json");
            var request8 = await httpClient.PostAsync("api/surveyquestions/" + survey1.Id, content7);
            Assert.Equal(System.Net.HttpStatusCode.OK, request8.StatusCode);


            //List all questions for a survey : [GET] /surveyquestions/{surveyId}
            var request9= await httpClient.GetAsync("api/surveyquestions/" + survey1.Id);
            Assert.Equal(System.Net.HttpStatusCode.OK, request9.StatusCode);
            var questions2 = request9.Content.ReadAsStringAsync().Result;

            Assert.Contains("Test question 5", questions2);
            Assert.Contains("Test description 5", questions2);


            List<Question> list2 = JsonConvert.DeserializeObject<List<Question>>(questions2);
            foreach (Question item in list2)
            {
                if (item.Name == "Test question 5") question5.Id = item.Id;

            }

            //Remove questions from question list in survey: [DELETE]/surveyquestions/{questionI

            var request10 = await httpClient.DeleteAsync("api/surveyquestions/" + question5.Id);
            Assert.Equal(System.Net.HttpStatusCode.OK, request10.StatusCode);

            var request12 = await httpClient.DeleteAsync("api/survey/" + survey1.Id); //Delete all created items
            Assert.Equal(System.Net.HttpStatusCode.OK, request12.StatusCode);

            var request11 = await httpClient.GetAsync("api/surveyquestions/" + survey1.Id);
            Assert.Equal(System.Net.HttpStatusCode.NotFound, request11.StatusCode);


           
        }

    }
}
