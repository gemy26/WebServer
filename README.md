# Project: C# Web Server

## Overview
This project implements a lightweight and customizable web server in C#. It provides essential features for handling HTTP requests, routing, and response management, making it a foundational solution for building web applications or APIs.

## Features
- **Routing System**: Dynamically map incoming requests to appropriate handlers.
- **Controller Integration**: Use attributes to define and manage endpoints effectively.
- **Request and Response Management**: Handle HTTP requests and generate structured responses.
- **Error Handling**: Improve error responses for unhandled exceptions or missing routes.

## Project Structure
The project consists of the following files:

### Core Components
- **`Server`**: The entry point for the web server. Handles server startup and listens for incoming requests.
- **`Router`**: Manages routing of requests to their respective handlers.
- **`Request`**: Represents an HTTP request with parsed data like headers, method, and body.
- **`RequestHandler`**: Handles processing of incoming requests and invokes appropriate routes.
- **`ResponseHandler`**: Generates and sends HTTP responses to clients.

### Attributes
- **`RouteAttribute`**: Enables declarative routing by defining routes directly on methods.
- **`ControllerAttribute`**: Marks a class as a controller to group related endpoints. Controllers decorated with this attribute are automatically scanned and registered by the router.

### Controllers
- **`Controllers`**: Example or base controllers showcasing route definition and business logic handling.

### Entry Point
- **`Program`**: Contains the `Main` method to bootstrap and start the server.

## Getting Started

### Prerequisites
- .NET 8.0 SDK or later.
- A development environment such as Visual Studio or VS Code.

### Installation
1. Clone the repository:
   ```bash
   git clone <repository-url>
   cd <repository-directory>
   ```
2. Open the project in your preferred IDE.
3. Build the project to restore dependencies and compile the code:
   ```bash
   dotnet build
   ```

### Running the Server
Run the server using the following command:
```bash
dotnet run
```
The server will start and listen on the default port (e.g., `http://localhost:5000`).

### Defining Routes
To define new routes:
1. Add methods to `Controllers.cs` or create a new controller file.
2. Decorate the methods with the `[Route("<path>")]` attribute to specify the endpoint.

Example:
```csharp
[Controller]
public class HelloController {
    [Route("/hello")]
    public string SayHello() {
        return "Hello, world!";
    }
}
```

### Testing
Use tools like Postman or a web browser to send requests to the server and validate responses.

## Extending the Project
- **Middleware**: Add support for middleware by enhancing `RequestHandler`.
- **Static File Serving**: Implement a handler to serve static files.
