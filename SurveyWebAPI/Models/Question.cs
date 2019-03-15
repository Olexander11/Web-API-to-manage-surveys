using System.Collections.Generic;

namespace SurveyWebAPI.Models
{
    public class Question
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Answer> QuestionAnswers { get; set; }
    }
}
