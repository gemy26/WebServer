namespace WebServer;

[Controller("")]
public class Controllers
{
    [Route("/api/sayHello")]
    public static string SayHello()
    {
        return "Hello, World!";
    }

    [Route("/api/greet")]
    public static string Greet(string name, int age)
    {
        return $"Hello, {name}. You are {age} years old.";
    }
}