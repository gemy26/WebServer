using System.Reflection;

namespace WebServer;

public static class Router
{
    private static Dictionary<string, MethodInfo> routes = new();
    
    public static void AutoRegisterRoutes()
    {
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (var type in assembly.GetTypes())
            {
                if (type.GetCustomAttribute<ControllerAttribute>() != null)
                {
                    foreach (var method in type.GetMethods())
                    {
                        var routeAttr = method.GetCustomAttribute<RouteAttribute>();
                        if (routeAttr != null)
                        {
                            Console.WriteLine("I'm adding new route :" + routeAttr.Route + " for method :" + method);
                            routes[routeAttr.Route.Substring(1)] = method;
                        }
                    }
                }
            }
        }
    }

    public static string HandleRoute(string route, Dictionary<string, string> body)
    {
        //Console.WriteLine("I'm in HandleRoute");
        if (routes.TryGetValue(route, out MethodInfo method))
        {
            var parameters = method.GetParameters();
            object[] invokeArgs = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                string paramName = parameters[i].Name;
                if (body.TryGetValue(paramName, out string value))
                {
                    try
                    {
                        invokeArgs[i] = Convert.ChangeType(value, parameters[i].ParameterType);
                        Console.WriteLine("I'm handling the route");
                    }
                    catch
                    {
                        Console.WriteLine("Parameter conversion failed. Returning Bad Request.");
                        return "400 Bad Request";
                    }
                }
                else
                {
                    invokeArgs[i] = Type.Missing;
                }
            }

            try
            {
                Console.WriteLine("I Trying to Invok");

                object result = method.Invoke(null, invokeArgs);
                Console.WriteLine("I Invoked");
                return result?.ToString() ?? string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error invoking route method: {ex.Message}");
                return "500 Internal Server Error";
            }
        }
        else
        {
            return "404 Not Found";
        }
    }
}


