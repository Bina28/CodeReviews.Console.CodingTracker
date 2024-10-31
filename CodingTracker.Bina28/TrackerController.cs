
using Spectre.Console;
using System.Diagnostics;

namespace CodingTracker.Bina28;

internal class TrackerController
{
	private DatabaseManager databaseManager;
	private List<TrackerModel> records;
	Validation validation = new();
	public TrackerController()
	{
		databaseManager = new DatabaseManager();
		records = databaseManager.GetAllRecords(DBconfig.ConnectionString) ?? new List<TrackerModel>();
	}
	public void ViewRecords()
	{
		var table = new Table();
		table.Border = TableBorder.Rounded;

		table.AddColumn("[#ff007f]ID[/]");
		table.AddColumn("[#ff007f]Date[/]");
		table.AddColumn("[#ff007f]Start Time[/]");
		table.AddColumn("[#ff007f]End Time[/]");
		table.AddColumn("[#ff007f]Duration[/]");

		foreach (TrackerModel record in records)
		{
			table.AddRow(
				record.Id.ToString(),
				$"[yellow]{record.Date}[/]",
				$"[yellow]{record.StartTime}[/]",
				$"[yellow]{record.EndTime}[/]",
				record.Duration.ToString()
				);
		}
		AnsiConsole.Write(table);
		AnsiConsole.MarkupLine("Press any key");
		Console.ReadKey();
	}

	public void AddRecord()
	{
		var newRecord = CollectInput(records.Count + 1);
		try
		{
			databaseManager.SaveRecord(DBconfig.ConnectionString, newRecord);
			records.Add(newRecord);
			AnsiConsole.MarkupLine("\n[#FFA500]Record added successfully![/]");
		}
		catch (Exception ex)
		{
			AnsiConsole.MarkupLine($"[red]Error saving record: {ex.Message}[/]");
		}

		AnsiConsole.MarkupLine("Press any key");
		Console.ReadKey();
	}

	public void RemoveRecord()
	{
		if (records.Count == 0)
		{
			AnsiConsole.MarkupLine("[red]No books available to delete.[/]");
			Console.ReadKey();
			return;
		}

		var recordToDelete = Display("delete");

		// Call the DeleteRecord method to remove from the database
		try
		{
			databaseManager.DeleteRecord(DBconfig.ConnectionString, recordToDelete.Id);
			records.Remove(recordToDelete); // Remove from the in-memory list
			AnsiConsole.MarkupLine("[#FFA500]Record deleted successfully.[/]");
		}
		catch (Exception ex)
		{
			AnsiConsole.MarkupLine($"[red]Error deleting record: {ex.Message}[/]");
		}

		AnsiConsole.MarkupLine("Press any key");
		Console.ReadKey();
	}

	public void UpdateRecord()
	{
		if (records.Count == 0)
		{
			AnsiConsole.MarkupLine("[red]No books available to update.[/]");
			Console.ReadKey();
			return;
		}

		var recordToUpdate = Display("update");

		AnsiConsole.MarkupLine($"You selected the following record to update: [yellow]{recordToUpdate}[/]");
		var updatedRecord = CollectInput(recordToUpdate.Id);

		try
		{
			databaseManager.UpdateRecord(DBconfig.ConnectionString, updatedRecord);
			records[records.IndexOf(recordToUpdate)] = updatedRecord;
			AnsiConsole.MarkupLine("[#FFA500]Record updated successfully.[/]");
		}
		catch (Exception ex)
		{
			AnsiConsole.MarkupLine($"[red]Error updating record: {ex.Message}[/]");
			// Consider logging the exception or using Console.WriteLine for debugging
			Console.WriteLine($"Exception: {ex}");
		}

		AnsiConsole.MarkupLine("Press any key");
		Console.ReadKey();
	}

	public string CalculateDuration(string startTime, string endTime)
	{
		TimeSpan start = TimeSpan.Parse(startTime);
		TimeSpan end = TimeSpan.Parse(endTime);
		TimeSpan duration = end - start;
		string durationString = duration.ToString();
		return durationString;
	}

	string GetValidDate(string prompt)
	{
		var date = AnsiConsole.Ask<string>(prompt);
		while (!validation.IsValidDate(date))
		{
			Console.WriteLine("The format is incorrect. Please try again: ");
			date = AnsiConsole.Ask<string>("Enter the date of coding session in this format yyyy-MM-dd: ");
		}
		return date;
	}

	string GetValidTime(string prompt)
	{
		var time = AnsiConsole.Ask<string>(prompt);
		while (!validation.IsValidTime(time))
		{
			Console.WriteLine("The format is incorrect. Please try again: ");
			time = AnsiConsole.Ask<string>(prompt);
		}
		return time;
	}

	public TrackerModel Display(string operation)
	{
		return AnsiConsole.Prompt(
			   new SelectionPrompt<TrackerModel>()
			   .Title($"Select the [blue]record[/] to {operation}: ")
			   .AddChoices(records));
	}

	TrackerModel CollectInput(long id)
	{
		var date = GetValidDate("Enter the date of coding session in this format yyyy-MM-dd: ");
		var startTime = GetValidTime("Enter the start time in this format HH:mm: ");
		var endTime = GetValidTime("Enter the end time in this format HH:mm: ");
		var duration = CalculateDuration(startTime, endTime);

		while (!validation.IsValidTimeRange(startTime, endTime))
		{
			AnsiConsole.MarkupLine("[red bold]The end time must be after the start time.[/]");
			startTime = GetValidTime("Enter the start time in this format HH:mm: ");
			endTime = GetValidTime("Enter the end time in this format HH:mm: ");
		}
		return new TrackerModel(id, date, startTime, endTime, duration);
	}


	internal void StopWatch()
	{
		Stopwatch stopWatch = new Stopwatch();
		bool running = false;

		while (true) // Loop to allow repeated start/stop
		{
			Console.Clear();
			var result = AnsiConsole.Prompt(
				new SelectionPrompt<string>()
					.Title("To utilize the StopWatch functionality, select [green]\"Start\"[/] at the beginning of your coding session \nand then choose [green]\"Stop\"[/] when you finish. Select [red]\"Exit\"[/] to end the program.")
					.AddChoices("Start", "Stop", "Exit"));

			switch (result)
			{
				case "Start":
					if (!running) // Check if it's already running
					{
						stopWatch.Start();
						running = true;
						AnsiConsole.MarkupLine("[yellow]The time counting has started.[/]");
					}
					else
					{
						AnsiConsole.MarkupLine("[red]The timer is already running.[/]");
					}
					break;

				case "Stop":
					if (running) // Check if it was running
					{
						
						stopWatch.Stop();
						TimeSpan ts = stopWatch.Elapsed;
						string elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
							ts.Hours, ts.Minutes, ts.Seconds,
							ts.Milliseconds / 10);
						Console.WriteLine("Total duration time: " + elapsedTime);
						running = false; // Reset running status
					}
					else
					{
						AnsiConsole.MarkupLine("[red]The timer is not running.[/]");
					}
					break;

				case "Exit":
					AnsiConsole.MarkupLine("[cyan]Exiting StopWatch functionality.[/]");
					return; // Exit the method

				default:
					AnsiConsole.MarkupLine("[red]Invalid choice. Please try again.[/]");
					break;
			}

			AnsiConsole.MarkupLine("Press any key to continue...");
			Console.ReadKey();
		}
	}
}
