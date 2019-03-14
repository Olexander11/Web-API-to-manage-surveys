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

        public IActionResult Survey(int id)
        {
            Survey survey = db.Surveys.FirstOrDefault(x => x.Id == id);
            List <Question> questions = survey.Questions; ;
            ViewData["SurveyName"] = survey.Name;

            return View(questions);
        }

        public IActionResult AddQuestion()
        {
            return View();
        }
    }
}