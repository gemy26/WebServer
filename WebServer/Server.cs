using System.Net;
using System.Net.Sockets;

namespace WebServer;

public class Server
{
    private static IPAddress _localAddress = IPAddress.Parse("127.0.0.1");
    private TcpListener tcpListener = new TcpListener(_localAddress, 8080);

    public void Run()
    {
        try
        {
            tcpListener.Start();
            Console.WriteLine("Server started on http://127.0.0.1:8080");
        
            while (true)
            {
                Console.WriteLine("Waiting for client connection...");
                TcpClient client = tcpListener.AcceptTcpClient();
                Console.WriteLine("Client connected!");
            
                // Handle each client in a separate task
                Task.Run(() =>
                {
                    try
                    {
                        Request request = RequestHandler.HandleClient(client);
                        Console.WriteLine("Request Body");
                        foreach (var i in request.Body)
                        {
                            Console.WriteLine(i.Key +" "+ i.Value);
                        }
                        ResponseHandler responseHandler = new ResponseHandler(request, client);
                        responseHandler.SendResponse();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error handling client: {ex.Message}");
                    }
                });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Server error: {ex.Message}");
        }
    }
}