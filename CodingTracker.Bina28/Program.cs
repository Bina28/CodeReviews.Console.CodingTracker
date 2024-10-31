using SQLitePCL;

namespace CodingTracker.Bina28;

internal class Program{
	
	static void Main(string[] args)
	{
		Batteries.Init();
	
		DatabaseManager databaseManager = new();
		databaseManager.CreateTable(DBconfig.ConnectionString);

		UserInterface userInterface = new();
		userInterface.MainMenu();
	}
}