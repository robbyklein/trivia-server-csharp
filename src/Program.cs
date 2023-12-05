using System;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

#if SERVER
class Program {
  static async Task Main(string[] args) {
    await Server.Start();
  }
}
#endif

#if CLIENT
class Program
{
  static async Task Main(string[] args)
  {
    var uri = new Uri("ws://localhost:3000");
    using var webSocket = new ClientWebSocket();

    try
    {
      await webSocket.ConnectAsync(uri, CancellationToken.None);
      Console.WriteLine("Connected to the server");
      Console.WriteLine("-------------------------------");

      // Send initial message
      await SendMessage(webSocket, "{\"Type\": \"WELCOME\"}");

      // Start receiving messages
      await ReceiveMessages(webSocket);

      // Uncomment the line below to enable interactive message sending
      // await InteractiveSendMessage(webSocket);
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Error: {ex.Message}");
    }
  }

  static async Task SendMessage(ClientWebSocket webSocket, string message)
  {
    byte[] messageBytes = Encoding.UTF8.GetBytes(message);
    await webSocket.SendAsync(new ArraySegment<byte>(messageBytes, 0, messageBytes.Length), WebSocketMessageType.Text, true, CancellationToken.None);
  }

  static async Task ReceiveMessages(ClientWebSocket webSocket)
  {
    var buffer = new byte[1024];

    try
    {
      while (webSocket.State == WebSocketState.Open)
      {
        var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

        if (result.MessageType == WebSocketMessageType.Close)
        {
          await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
          Console.WriteLine("Disconnected from server");
        }
        else if (result.MessageType == WebSocketMessageType.Text)
        {
          var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
          Console.WriteLine(message);
          Console.WriteLine("-------------------------------");
        }
      }
    }
    catch (WebSocketException)
    {
      Console.WriteLine("Disconnected from server");
    }
  }

  static async Task InteractiveSendMessage(ClientWebSocket webSocket)
  {
    while (webSocket.State == WebSocketState.Open)
    {
      Console.Write("Enter message to send: ");
      string message = Console.ReadLine();
      await SendMessage(webSocket, message);
    }
  }
}
#endif

