using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using SurveyWebAPI.Controllers;
using System;
using System.Net.Http;
using Xunit;

namespace UnitTestSurveyWebAPI
{
    public class UnitTest1
    {

        HttpClient httpClient;

        public UnitTest1()
        {
            var server = new TestServer(new WebHostBuilder().UseStartup<SurveyWebAPI.Startup>());
            httpClient = server.CreateClient();
        }

        [Fact]
        public async void Test1()
        {

           var survey = await httpClient.GetAsync("api/Survey");
            survey.EnsureSuccessStatusCode();

            Assert.Equal(System.Net.HttpStatusCode.OK, survey.StatusCode);

            var string1 = survey.Content.ReadAsStringAsync();


        }
    }
}
