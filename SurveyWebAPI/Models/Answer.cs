using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace SurveyWebAPI.Models
{
    public class Answer
    {
        public long Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public Question Parent { get; set; }
    }
}
