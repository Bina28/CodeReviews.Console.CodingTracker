namespace CodingTracker.Bina28;

internal class Validation
{
	public bool IsValidDate(string date)
	{
		string format = "yyyy-MM-dd";

		// Try to parse the date string
		return DateTime.TryParseExact(date, format, null, System.Globalization.DateTimeStyles.None, out _);
	}

	public bool IsValidTime(string time)
	{
		string format = "HH:mm";

		return DateTime.TryParseExact(time, format, null, System.Globalization.DateTimeStyles.None, out _);
	}

	public bool IsValidTimeRange(string startTime, string endTime)
	{
		TimeSpan start = TimeSpan.Parse(startTime);
		TimeSpan end = TimeSpan.Parse(endTime);
		return start < end;
	}

}
