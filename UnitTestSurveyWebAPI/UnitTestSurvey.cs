using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using SurveyWebAPI.Controllers;
using System;
using System.Net.Http;
using Xunit;

namespace UnitTestSurveyWebAPI
{
    public class UnitTestSurvey
    {

        HttpClient httpClient;

        public UnitTestSurvey()
        {
            var server = new TestServer(new WebHostBuilder().UseStartup<SurveyWebAPI.Startup>());
            httpClient = server.CreateClient();
        }

        //List all surveys: [GET]/surveys
        [Fact]
        public async void TestGET() 
        {

           var survey = await httpClient.GetAsync("api/Survey");
            survey.EnsureSuccessStatusCode();

            Assert.Equal(System.Net.HttpStatusCode.OK, survey.StatusCode);

            var string1 = survey.Content.ReadAsStringAsync();
            
        }
    }
}
