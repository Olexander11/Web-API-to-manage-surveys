using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SurveyWebAPI.Models;

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

        public RedirectResult SubmitAddQuestion(int idSur, int IdQuest )
        {
            Survey survey = db.Surveys.FirstOrDefault(x => x.Id == idSur);
            Question question = db.Questions.FirstOrDefault(x => x.Id == IdQuest);
            survey.Questions.Add(question);

            db.Update(survey);
            db.SaveChanges();

            return RedirectPermanent("/Home/AddQuestion?id="+ idSur);
        }

        public IActionResult Survey(int id)
        {
            Survey survey = db.Surveys.FirstOrDefault(x => x.Id == id);
            List <Question> questions = survey.Questions; ;
            ViewData["SurveyName"] = survey.Name;

            return View(questions);
        }

        public IActionResult AddQuestion(int id)
        {
            Survey survey = db.Surveys.FirstOrDefault(x => x.Id == id);
            ViewData["SurveyName"] = survey.Name;
            ViewData["Id"] = survey.Id;
            List<Question> questions = db.Questions.ToList();
            if ( survey.Questions.ToList()!=null) {
                foreach (Question item in survey.Questions.ToList())
                {
                    questions.Remove(item);
                }
            }
                   
            
            return View(questions);
        }
    }
}