using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SurveyWebAPI.Models;

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

        // List all surveys: [GET]/surveys
        [HttpGet]
        public IEnumerable<string> GetSurveys()
        {
             return db.Surveys.ToList();
        }

        
        // Get info for a single survey: [GET]/survey/{id}
        [HttpGet("{id}")]
        public string GetSurvey(int id)
        {
            return "value";
        }

        // Create survey: [POST]/survey
        [HttpPost]
        public void PostSurvey([FromBody]string value)
        {
        }

        // Edit survey: [PUT]/survey/{id}
        [HttpPut("{id}")]
        public void PutSurvey(int id, [FromBody]string value)
        {
        }

        // Delete survey: [DELETE] ]/survey/{id}
        [HttpDelete("{id}")]
        public void DeleteSurvey(int id)
        {
        }
     }
}
