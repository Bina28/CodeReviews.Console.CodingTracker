using Microsoft.Data.Sqlite;
using Dapper;


namespace CodingTracker.Bina28;

internal class DatabaseManager
{
	internal void CreateTable(string connectionString)
	{
		using (var connection = new SqliteConnection(connectionString))
		{
			connection.Open();
		
			// Define the SQL command to create the table
			var sql = @"CREATE TABLE IF NOT EXISTS CodingTracker (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Date TEXT,
                            StartTime TEXT,
                            EndTime TEXT,
                            Duration TEXT)";

			// Execute the SQL command using Dapper
			connection.Execute(sql);
		}
	}

	public List<TrackerModel> GetAllRecords(string connectionString)
	{
		using (var connection = new SqliteConnection(connectionString))
		{
			connection.Open();
			string sql = "SELECT Id, Date, StartTime, EndTime, Duration FROM CodingTracker";
			return connection.Query<TrackerModel>(sql).ToList();
		}
	}

	public void SaveRecord(string connectionString, TrackerModel record)
	{
		using(var connection = new SqliteConnection(connectionString))
		{
			connection.Open();
			var sql= "INSERT INTO  CodingTracker(Date, StartTime, EndTime, Duration) VALUES (@Date, @StartTime, @EndTime, @Duration)";
			connection.Execute(sql, new
			{
				record.Date,
				record.StartTime,
				record.EndTime,
				record.Duration
			});
		}
	}

	public void DeleteRecord(string connectionString, long recordId)
	{
		using (var connection = new SqliteConnection(connectionString))
		{
			connection.Open();
			var sql = "DELETE FROM CodingTracker WHERE Id = @Id"; 
			connection.Execute(sql, new { Id = recordId });
		}
	}

	public void UpdateRecord(string connectionString, TrackerModel updatedRecord)
	{
		 using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        var sql = @"
            UPDATE CodingTracker 
            SET Date = @Date, 
                StartTime = @StartTime, 
                EndTime = @EndTime, 
                Duration = @Duration 
            WHERE Id = @Id";
        
        connection.Execute(sql, new 
        {
            updatedRecord.Date,
            updatedRecord.StartTime,
            updatedRecord.EndTime,
            updatedRecord.Duration,
           updatedRecord.Id
        });
    }
	}
}
