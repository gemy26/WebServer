namespace WebServer;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]

public class RouteAttribute : Attribute
{
    public string Route;

    public RouteAttribute(string description)
    {
        this.Route = description;
    }
}