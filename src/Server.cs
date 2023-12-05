using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

class Server {
  public static int port = 3000;
  public static ConcurrentDictionary<Guid, WebSocket> ConnectedClients = new ConcurrentDictionary<Guid, WebSocket>();

  public static async Task Start() {
    // Create an http listener
    HttpListener listener = new HttpListener();
    listener.Prefixes.Add($"http://+:{port}/");

    // Start it
    listener.Start();
    Console.WriteLine($"Listening on port {port}");

    // Keep it running
    while (true) {
      // Check for requests
      HttpListenerContext context = await listener.GetContextAsync();

      // Handle them if websocket
      if (context.Request.IsWebSocketRequest) {
        // Send it to handler
        _ = RequestHandler.HandleWebSocketConnectionAsync(context);
      }
      // Otherwise send 400 status
      else {
        context.Response.StatusCode = 400;
        context.Response.Close();
      }
    }
  }
}
