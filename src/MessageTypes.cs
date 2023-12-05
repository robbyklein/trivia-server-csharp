public class ClientMessage {
  public string Type { get; set; } = string.Empty;
  public string AuthToken { get; set; } = string.Empty;
  public AnswerMessage Answer { get; set; } = new AnswerMessage();
  public RegisterMessage Register { get; set; } = new RegisterMessage();
  public LoginMessage Login { get; set; } = new LoginMessage();
}

public class AnswerMessage {
  public string Text { get; set; } = string.Empty;
}

public class RegisterMessage {
  public string Email { get; set; } = string.Empty;
  public string Password { get; set; } = string.Empty;
  public string Username { get; set; } = string.Empty;
}

public class LoginMessage {
  public string Email { get; set; } = string.Empty;
  public string Password { get; set; } = string.Empty;
  public string Username { get; set; } = string.Empty;
}

public class ServerResponse {
  public string Type { get; set; } = string.Empty;
  public WelcomeResponse Welcome { get; set; } = new WelcomeResponse();
}

public class WelcomeResponse {
  public string Message { get; set; } = string.Empty;
}
