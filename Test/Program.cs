﻿
public class Error
{ 
    public int error_code { get; set; }
    public string error_msg { get; set; }
    public List<RequestParam> request_params { get; set; }
}

public class RequestParam
{ 
    public string key { get; set; }
    public string value { get; set; }
}