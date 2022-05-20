using System.Text.Json.Serialization;

namespace RubyOnBrain.Domain
{
    public class UserCourse
    {
        //[ForeignKey("UserId")]
        public int UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }


        //[ForeignKey("CourseId")]
        public int CourseId { get; set; }
        [JsonIgnore]
        public Course Course { get; set; }
        public int Rating { get; set; }
    }
}