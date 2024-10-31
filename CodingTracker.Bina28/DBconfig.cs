using System.Configuration;


namespace CodingTracker.Bina28;

public static class DBconfig
{
	public static string ConnectionString { get; } = ConfigurationManager.AppSettings.Get("ConnectionString");
}
