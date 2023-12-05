using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

class RequestHandler {
  public static async Task HandleWebSocketConnectionAsync(HttpListenerContext context) {
    try {
      // Get the context
      WebSocketContext webSocketContext = await context.AcceptWebSocketAsync(subProtocol: null);

      // Get the websocket
      using WebSocket webSocket = webSocketContext.WebSocket;

      // Generate a unique ID for this client
      Guid clientId = Guid.NewGuid();

      // Add the client to the connectedClients dictionary
      Server.ConnectedClients.TryAdd(clientId, webSocket);

      // Create some space for messages
      byte[] buffer = new byte[1024];

      // While open accept messages
      while (webSocket.State == WebSocketState.Open) {
        var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

        if (result.MessageType == WebSocketMessageType.Text) {
          try {
            // Decode the data
            string receivedMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
            ClientMessage? clientMessage = JsonSerializer.Deserialize<ClientMessage>(receivedMessage);
            if (clientMessage == null) continue;

            Console.WriteLine(receivedMessage);
            Console.WriteLine(clientMessage.Type);

            // Pass to controller
            switch (clientMessage.Type) {


              case "ANSWER":
                // Handle the answer message
                break;
              case "REGISTER":
                // Handle the registration message
                break;
              case "LOGIN":
                // Handle the login message
                break;
              case "JOIN":
                // Handle the login message
                break;
              case "WELCOME":
                WelcomeController.Index(webSocket, clientMessage);
                break;
              default:
                break;
            }

          }
          catch (JsonException e) {
            Console.WriteLine($"Invalid JSON format: {e.Message}");
          }



        }
        else if (result.MessageType == WebSocketMessageType.Close) {
          await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
        }
      }
    }
    catch (Exception e) {
      // Log errors
      Console.WriteLine("WebSocket connection error: " + e.Message);
    }
  }
}


// Broadcast the received message to all connected clients
// foreach (var clientWebSocket in Server.ConnectedClients.Values) {
//   await clientWebSocket.SendAsync(new ArraySegment<byte>(messageBytes), WebSocketMessageType.Text, true, CancellationToken.None);
// }