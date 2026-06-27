namespace Session1
{
    public record LoginRequest(string Username, string Password);

    public record LoginResponse(string Token);
}
