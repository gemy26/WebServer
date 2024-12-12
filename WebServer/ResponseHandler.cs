using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;

namespace WebServer;
public class ResponseHandler
{
    private readonly Request _request;
    private readonly TcpClient _client;

    public ResponseHandler(Request request, TcpClient client)
    {
        _request = request;
        _client = client;
    }

    private string ProcessEndPoint()
    {
        var responseBodyObject = (object) Router.HandleRoute(_request.URL, _request.Body) ?? new { message = "404 Not Found" };
        string responseBody = JsonConvert.SerializeObject(responseBodyObject);
        
        return $"HTTP/1.1 200 OK\r\n" +
               "Content-Type: application/json\r\n" +
               "Access-Control-Allow-Origin: *\r\n" +
               $"Content-Length: {Encoding.UTF8.GetByteCount(responseBody)}\r\n" +
               "\r\n" +
               responseBody;
    }

    public void SendResponse()
    {
        try
        {
            using NetworkStream stream = _client.GetStream();
            stream.WriteTimeout = 5000; // 5 second timeout

            string response = ProcessEndPoint();
            byte[] responseBytes = Encoding.UTF8.GetBytes(response);
            
            stream.Write(responseBytes, 0, responseBytes.Length);
            stream.Flush();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending response: {ex.Message}");
            SendErrorResponse(ex.Message);
        }
        finally
        {
            _client.Close();
        }
    }

    private void SendErrorResponse(string message)
    {
        try
        {
            using NetworkStream stream = _client.GetStream();
            string errorResponse = $"HTTP/1.1 500 Internal Server Error\r\n" +
                                 "Content-Type: application/json\r\n" +
                                 "Connection: close\r\n" +
                                 "\r\n" +
                                 JsonConvert.SerializeObject(new { error = message });

            byte[] errorBytes = Encoding.UTF8.GetBytes(errorResponse);
            stream.Write(errorBytes, 0, errorBytes.Length);
            stream.Flush();
        }
        catch
        {
            Console.WriteLine("Failed to send error response");
        }
    }
}


