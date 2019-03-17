using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurveyWebAPI.Models;

namespace SurveyWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class QuestionController : Controller
    {
        SurveyContext db;

        public QuestionController(SurveyContext context)
        {
            this.db = context;
        }

      
        // Get info for a single question: [GET]/question/{id}
        [HttpGet("{id}")]
        public IActionResult GetQuestion(int id)
        {
            Question quest = db.Questions.Include(x =>x.QuestionAnswers).FirstOrDefault(x => x.Id == id);
            if (quest == null)
                return NotFound();
            return new ObjectResult(quest);
        }

        // Create question: [POST]/question
        [HttpPost]
        public IActionResult PostQuestion([FromBody]Question question)
        {
            if (question == null)
            {
                return BadRequest();
            }

            db.Questions.Add(question);
            db.SaveChanges();
            return Ok(question);
        }


        // Edit question: [PUT]/question/{id}
        [HttpPut("{id}")]
        public IActionResult PutQuestion(int id, [FromBody]Question question)
        {
            if (question == null)
            {
                return BadRequest();
            }
            if (!db.Questions.Any(x => x.Id == id))
            {
                return NotFound();
            }
            question.Id = id;
            db.Update(question);
            db.SaveChanges();
            return Ok(question);
        }


        // Delete question: [DELETE] ]/question/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteQuestion(int id)
        {
            Question quest = db.Questions.FirstOrDefault(x => x.Id == id);
            if (quest == null) return NotFound();
            db.Questions.Remove(quest);
            db.SaveChanges();
            return Ok(quest);
        }
    }

    [Route("api/[controller]")]
    public class QuestionsController : Controller
    {
        SurveyContext db;

        public QuestionsController(SurveyContext context)
        {
            this.db = context;
        }

        // List all questions: [GET]/questions
        [HttpGet]
        public IEnumerable<Question> GetQuestions()
        {
            return db.Questions.ToList();
        }
    }
}
