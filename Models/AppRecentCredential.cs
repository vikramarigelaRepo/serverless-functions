using System;

public class AppRecentCredential
{
    public string AppName { get; set; }
    public string APIKey { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public int DaysToExpiry
    {
        get
        {
            return
          ExpirationDate != null ? (Convert.ToDateTime(ExpirationDate) - DateTime.UtcNow).Days : -1;
        }
    }
}