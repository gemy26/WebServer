namespace WebServer;
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ControllerAttribute : Attribute
{
    public string Description { get; }

    public ControllerAttribute(string description)
    {
        Description = description;
    }
}