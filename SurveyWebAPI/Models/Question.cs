using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SurveyWebAPI.Models
{
    public class Question
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Answer> QuestionAnswers { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public Survey Parent { get; set; }
    }
}
