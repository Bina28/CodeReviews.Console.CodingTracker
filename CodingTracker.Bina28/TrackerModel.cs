

namespace CodingTracker.Bina28;

internal class TrackerModel
{
	public long Id { get; set; }
	public string Date { get; set; }
	public string Duration { get; set; }
	public string StartTime { get; set; }
	public string EndTime { get; set; }

	public TrackerModel(long id, string date, string startTime, string endTime, string duration)
	{
		Id = id;
		Date = date;
		StartTime = startTime;
		EndTime = endTime;
		Duration = duration;

	}
	public override string ToString()
	{
		return $"{Date} - {StartTime} to {EndTime} (Duration: {Duration})";
	}

}
