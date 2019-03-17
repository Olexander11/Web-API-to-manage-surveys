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
        [HttpGet("{id}")]
        public IActionResult GetSurveyQuestions(int id)
        {
            Survey survey = db.Surveys.Include(s => s.Questions).FirstOrDefault(x => x.Id == id);
            if (survey == null) return NotFound();

            return new ObjectResult(survey.Questions.ToList());

        }

        // Add question to list of the questions in survey: [POST] /survey/{id}
        [HttpPost("{id}")]
        public IActionResult PostSurveyQuestion([FromBody]Question question, int id)
        {
            Survey survey = db.Surveys.Include(x=>x.Questions).FirstOrDefault(x => x.Id == id);
            if (survey == null) return NotFound();

            survey.Questions.Add(question);

            db.Update(survey);
            db.SaveChanges();
            return Ok(survey);

        }

         // Remove questions from question list in survey: [DELETE]/surveyquestions/{questionI
        [HttpDelete("{id}")]
        public IActionResult DeleteSurveyQuestion(int id)
        {
            Question quest = db.Questions.FirstOrDefault(x => x.Id == id);
            if (quest == null) return NotFound();

            db.Remove(quest);
            db.SaveChanges();
            return Ok(quest);

        }
    }
}
