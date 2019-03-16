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
    public class SurveyController : Controller
    {
        SurveyContext db;

        public SurveyController(SurveyContext context)
        {
            this.db = context;
                     
        }
                
        // Get info for a single survey: [GET]/survey/{id}
        [HttpGet("{id}")]
        public IActionResult GetSurvey(int id)
        {
            Survey survey = db.Surveys.Include(s => s.Questions).FirstOrDefault(x => x.Id == id);
            if (survey == null) return NotFound();
            return new ObjectResult(survey);
        }

        // Create survey: [POST]/survey
        [HttpPost]
        public IActionResult PostSurvey([FromBody]Survey survey)
        {
            if (survey == null)
            {
                return BadRequest();
            }
            db.Surveys.Add(survey);
            db.SaveChanges();
            return Ok(survey);
        }

        // Create survey: [POST]/survey
        [HttpPost]
        public IActionResult PostSurvey(int id, [FromBody]Question question)
        {
            if (question == null)
            {
                return BadRequest();
            }
            if (!db.Surveys.Any(x => x.Id == id))
            {
                return NotFound();
            }

            Survey survey = db.Surveys.Include(s => s.Questions).FirstOrDefault(x => x.Id == id);
            
            survey.Questions.Add(question);
            db.SaveChanges();
            return Ok(survey);
        }

        // Edit survey: [PUT]/survey/{id}
        [HttpPut("{id}")]
        public IActionResult PutSurvey(Survey survey)
        {
            if (survey == null)
            {
                return BadRequest();
            }

            if (!db.Surveys.Any(x => x.Id == survey.Id))
            {
                return NotFound();
            }

            db.Update(survey);
            db.SaveChanges();
            return Ok(survey);

        }

        // Delete survey: [DELETE] ]/survey/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteSurvey(int id)
        {
            Survey survey = db.Surveys.FirstOrDefault(x => x.Id == id);
            if (survey == null) return NotFound();

            db.Surveys.Remove(survey);
            db.SaveChanges();
            return Ok(survey);
        }
     }

    [Route("api/[controller]")]
    public class SurveysController : Controller
    {
        SurveyContext db;

        public SurveysController(SurveyContext context)
        {
            this.db = context;

        }

        // List all surveys: [GET]/surveys
        [HttpGet]
        public IEnumerable<Survey> GetSurveys()
        {
            return db.Surveys.ToList();
        }
    }
}
