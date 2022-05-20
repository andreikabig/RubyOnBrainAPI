namespace RubyOnBrain.API.Classes.Responses
{
    public class Error
    {
        public int error_code { get; set; }
        public string error_msg { get; set; }
        public RequestParams request_params { get; set; }
    }

}
