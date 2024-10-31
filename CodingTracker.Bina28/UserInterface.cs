
using Spectre.Console;
using static CodingTracker.Bina28.Enums;


namespace CodingTracker.Bina28;

internal class UserInterface
{
	private readonly TrackerController _trackerController = new();
	internal void MainMenu()
	{
		bool continueProgram = true;
		while (continueProgram)
		{
			Console.Clear();

			var actionChoice = AnsiConsole.Prompt(
				new SelectionPrompt<MenuAction>()
				.Title("Choose your next action:")
				.AddChoices(Enum.GetValues<MenuAction>()));

			switch (actionChoice)
			{
				case MenuAction.ViewRecord:
					_trackerController.ViewRecords();
					break;
				case MenuAction.UpdateRecord:
					_trackerController.UpdateRecord();
					break;
				case MenuAction.DeleteRecord:
					_trackerController.RemoveRecord();
					break;
				case MenuAction.AddRecord:
					_trackerController.AddRecord();
					break;
				case MenuAction.StopWatch:
					_trackerController.StopWatch();
					break;
				case MenuAction.Exit:
					continueProgram = false;
					break;
				default:
					Console.WriteLine("Invalid input");
					break;
			}
		}
	}
}
