using System.Net.Sockets;
using System.Text;

namespace WebServer;
public class RequestHandler
{
    private const int BUFFER_SIZE = 1024;

    public static Request HandleClient(TcpClient client)
    {
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[BUFFER_SIZE];
        StringBuilder requestData = new StringBuilder();
        
        // Read the first chunk which should contain headers
        int bytesRead = stream.Read(buffer, 0, buffer.Length);
        requestData.Append(Encoding.ASCII.GetString(buffer, 0, bytesRead));
        
        // Find the end of headers
        string request = requestData.ToString();
        int headerEnd = request.IndexOf("\r\n\r\n");
        
        if (headerEnd == -1)
        {
            throw new Exception("Invalid HTTP request format");
        }

        // Parse headers
        string[] headerLines = request.Substring(0, headerEnd).Split(new[] { "\r\n" }, StringSplitOptions.None);
        
        // Parse the request line
        string[] requestLine = headerLines[0].Split(' ');
        Request parsedRequest = new Request
        {
            Method = requestLine[0],
            URL = requestLine[1].TrimStart('/'),
            Headers = new Dictionary<string, string>(),
            Body = new Dictionary<string, string>()
        };

        // Parse headers
        for (int i = 1; i < headerLines.Length; i++)
        {
            string[] header = headerLines[i].Split(  ": " , 2);
            if (header.Length == 2)
            {
                parsedRequest.Headers[header[0]] = header[1];
            }
        }

        // Read body if Content-Length is present
        
        if (parsedRequest.Headers.ContainsKey("Content-Type") &&
            parsedRequest.Headers["Content-Type"].StartsWith("multipart/form-data"))
        {
            string existingBody = request.Substring(headerEnd + 4);
            string boundry = parsedRequest.Headers["Content-Type"].Split(new[] { "boundary=" }, StringSplitOptions.None)[1];
            boundry = "--" + boundry;
            parsedRequest.Body = ParseBody(existingBody, boundry);
        }
        
        return parsedRequest;
    }
    
    private static Dictionary<string, string> ParseBody(string body, string boundry)
    {
        Dictionary<string, string> Body = new Dictionary<string, string>();

        var lines = body.Split(boundry);
        
        foreach (var line in lines)
        {
            var sections = line.Split(new[] { "\r\n\r\n" }, 2, StringSplitOptions.None);
            if (sections.Length < 2) continue;
            
            string headers = sections[0];
            string value = sections[1].Trim();
            
            foreach (var header in headers.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (header.StartsWith("Content-Disposition"))
                {
                    string key = header.Split(new[] { "name=\"" }, StringSplitOptions.None)[1].Split('"')[0];
                    //Console.WriteLine($"Field Name: {name}");
                    //Console.WriteLine($"Field Value: {value}");
                    Body[key] = value;
                    break;
                }
            }
        }

        return Body;
    }
}
