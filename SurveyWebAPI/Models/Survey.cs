using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyWebAPI.Models
{
    public class Survey
    {
        public long Id { get; set; }
        public string SurveyName { get; set; }
        public string SurveyDescription { get; set; }
        public List<Question> SurveyQuestions { get; set; }
    }
}
