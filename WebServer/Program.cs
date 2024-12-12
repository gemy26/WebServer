using System.Reflection;
using System.Runtime.CompilerServices;

namespace WebServer;

class Program
{
    static void Main(string[] args)
    {
        Router.AutoRegisterRoutes();
        Server server = new Server(); 
        server.Run();
        
        
        /*

        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (var type in assembly.GetTypes())
            {
                if (type.GetCustomAttribute<ControllerAttribute>() != null)
                {
                    foreach (var method in type.GetMethods())
                    {
                        var routeAttribute = method.GetCustomAttribute<RouteAttribute>();
                        if (routeAttribute != null)
                        {
                            Console.WriteLine($"  Method: {method.Name}, Route: {routeAttribute.Route}");
                        }
                    }
                }
            }
        }
        */



    }
}