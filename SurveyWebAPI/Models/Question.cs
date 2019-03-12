using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyWebAPI.Models
{
    public class Question
    {
        public long Id { get; set; }
        public string QuestionName { get; set; }
        public string QuestionDescription { get; set; }
        public List<string> QuestionAnswears { get; set; }
    }
}
