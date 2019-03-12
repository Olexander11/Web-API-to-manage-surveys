using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

         // List all questions: [GET]/questions
        [HttpGet]
        public IEnumerable<string> GetQuestions()
        {
            return new string[] { "value1", "value2" };
        }
                
        // Get info for a single question: [GET]/question/{id}
        [HttpGet("{id}")]
        public string GetQuestion(int id)
        {
            return "value";
        }
                
        // Create question: [POST]/question
        [HttpPost]
        public void PostQuestion([FromBody]string value)
        {
        }

              
        // Edit question: [PUT]/question/{id}
        [HttpPut("{id}")]
        public void PutQuestion(int id, [FromBody]string value)
        {
        }

        
        // Delete question: [DELETE] ]/question/{id}
        [HttpDelete("{id}")]
        public void DeleteQuestion(int id)
        {
        }
     }
}
