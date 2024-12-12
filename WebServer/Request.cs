namespace WebServer;

public class Request
{
    public string Method { set; get; }
    public string URL { set; get; }
    public string EndPoint { set; get; }
    public Dictionary<string, string> Body;
    public Dictionary<string, string> Headers;

   
}