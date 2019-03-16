using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SurveyWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace SurveyWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class SurveyQuestionsController : Controller
    {
        SurveyContext db;

        public SurveyQuestionsController(SurveyContext context)
        {
            this.db = context;
        }

        // List all questions for a survey : [GET] /surveyquestions/{questionId}
        [HttpGet]
        public IEnumerable<Question> GetSurveyQuestions(int id)
        {
            Survey survey = db.Surveys.Include(s => s.Questions).FirstOrDefault(x => x.Id == id);
            return  survey.Questions.ToList();
            
        }

        // Add question to list of the questions in survey: [POST] /survey/{id}
        [HttpPost]
        public IActionResult PostSurveyQuestion(Survey survey, int id)
        {
            Question quest = db.Questions.FirstOrDefault(x => x.Id == id);
            survey.Questions.Add(quest);

            db.Update(survey);
            db.SaveChanges();
            return Ok(survey);

        }

         // Remove questions from question list in survey: [DELETE]/surveyquestions/{questionI
        [HttpDelete("{id}")]
        public IActionResult DeleteSurveyQuestion(Survey survey, int id)
        {
            Question quest = db.Questions.FirstOrDefault(x => x.Id == id);
            survey.Questions.Remove(quest);

            db.Update(survey);
            db.SaveChanges();
            return Ok(survey);

        }
    }
}
