using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SurveyWebAPI.Models;

namespace SurveyWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class SurQuestionController : Controller
    {
        SurveyContext db;

        public SurQuestionController(SurveyContext context)
        {
            this.db = context;
        }
               
        // List all questions for a survey : [GET] /surveyquestions/{questionId}
        [HttpGet]
        public IEnumerable<string> GetSurveyQuestions()
        {
            return new string[] { "value1", "value2" };
        }

        // Add question to list of the questions in survey: [POST] /survey/{id}
        [HttpPost]
        public void PostSurveyQuestion([FromBody]string value)
        {
        }

         // Remove questions from question list in survey: [DELETE]/surveyquestions/{questionI
        [HttpDelete("{id}")]
        public void DeleteSurveyQuestion(int id)
        {
        }
    }
}
