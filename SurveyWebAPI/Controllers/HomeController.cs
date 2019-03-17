using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SurveyWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace SurveyWebAPI.Controllers
{
    public class HomeController : Controller
    {
        SurveyContext db;

        public HomeController(SurveyContext context)
        {
            this.db = context;
        }

        public IActionResult Index()
        {
            List<Survey> surveys = db.Surveys.ToList();



            return View(surveys);
        }

        public IActionResult CreateSur()
        {
          return View();
        }

        public RedirectResult SubmitCreateSur(string name, string description)
        {
            Survey survey = new Survey { Name = name, Description = description };
            db.Surveys.Add(survey);
            db.SaveChanges();
                        
            return RedirectPermanent("/Home/Index");
        }

        public RedirectResult SubmitCreateQuestion(int idSur, string name, string description, List<string> items)
        {
            Survey survey = db.Surveys.Include(s => s.Questions).FirstOrDefault(x => x.Id == idSur);
           
            List<Answer> answears = new List<Answer>();
            foreach(string it in items)
            {
                if (it == null) continue;
                Answer answear = new Answer {Name = it };
                answears.Add(answear);
               
            }

            Question question = new Question { Name = name, Description = description, QuestionAnswers = answears };
                     
           
            survey.Questions.Add(question);                    
           
            db.SaveChanges();

            return RedirectPermanent("/Home/CreateQuestion?id=" + idSur);
        }

        public IActionResult Survey(int id)
        {
            Survey survey = db.Surveys.Include(s =>s.Questions).FirstOrDefault(x => x.Id == id);
            List<Question> questions = survey.Questions;
            if (questions != null)
            {
                foreach(Question question in questions)
                {
                    db.Entry(question).Collection(x => x.QuestionAnswers).Load();
                }
            }
            ViewData["SurveyName"] = survey.Name;
            ViewData["SurveId"] = survey.Id;
            return View(questions);
        }

        public IActionResult CreateQuestion(int id)
        {
            Survey survey = db.Surveys.FirstOrDefault(x => x.Id == id);
            ViewData["SurveyName"] = survey.Name;
            ViewData["Id"] = survey.Id;
                     
            
            return View();
        }
    }
}