using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

class WelcomeController {
  public static async void Index(WebSocket webSocket, ClientMessage message) {
    ServerResponse response = new ServerResponse {
      Type = "WELCOME",
      Welcome = {
        Message = "Welcome to the server!\nThis is mahhh shitt"
      }
    };

    string responseJSONString = JsonSerializer.Serialize(response);

    byte[] messageBytes = Encoding.UTF8.GetBytes(responseJSONString);

    if (webSocket.State == WebSocketState.Open) {
      await webSocket.SendAsync(new ArraySegment<byte>(messageBytes, 0, messageBytes.Length), WebSocketMessageType.Text, true, CancellationToken.None);
    }
  }
}