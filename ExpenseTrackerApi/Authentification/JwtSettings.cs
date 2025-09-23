namespace ExpenseTrackerApi.Authentification;

public sealed class JwtSettings
{
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty ;
    public string Key {  get; set; } = string.Empty ;

    public int ExpirationMinutes { get; set; } = 120;

    public void EnsureValid()
    {
        if(string.IsNullOrWhiteSpace(Key) || System.Text.Encoding.UTF8.GetByteCount(Key) < 16)
        {
            throw new InvalidOperationException(
                   "Jwt:Key trebuie să aibă cel puțin 16 bytes (128 biți). Recomandat 32+ caractere.");
        }
    }
}
