﻿using System.Data.SQLite;

class Program
{
    static public SQLiteConnection connection;

    static void Main(string[] args)
    {
	InitializeDatabase();
	
	DrawScreen();
	
	while(true)
	{
	ConsoleKeyInfo key = Console.ReadKey(true);
	switch(key.Key){
		case ConsoleKey.D0:
			Console.WriteLine("pressed 0");
			return;
			break;
		case ConsoleKey.D1:
			Console.WriteLine("pressed 1");
			ViewRecords();
			break;
		case ConsoleKey.D2:
			Console.WriteLine("pressed 2");
			InsertRecord();
			break;
		case ConsoleKey.D3:
			Console.WriteLine("pressed 3");
			DeleteRecord();
			break;
		case ConsoleKey.D4:
			Console.WriteLine("pressed 4");
			UpdateRecord();
			break;
		default:
			Console.WriteLine("unrecognized Key please try again");
			break;
	}
	
	}
    }

    static private void InitializeDatabase(){
    
      	string dbPath = "HabitLoggerDb.sqlite";
        if(!File.Exists(dbPath)) SQLiteConnection.CreateFile(dbPath);
    	
	connection = new SQLiteConnection($"Data Source={dbPath};Version=3;");
	connection.Open();
	string createTableQuery = @"
		CREATE TABLE IF NOT EXISTS Habits
		(
		 Id INTEGER PRIMARY KEY AUTOINCREMENT, 
		 Habit TEXT NOT NULL,
		 Time DATETIME NOT NULL
		 );
		";

	using (SQLiteCommand command = new SQLiteCommand(createTableQuery, connection))
	{
		command.ExecuteNonQuery();
	}


    }

    static public void ViewRecords(){
    	string query = "SELECT * FROM Habits";
	SQLiteCommand command = new SQLiteCommand(query, connection);
	SQLiteDataReader reader = command.ExecuteReader();
	while(reader.Read())
	{
		Console.WriteLine(reader["Id"] + " " +reader["Habit"] + " " + reader["Time"]);
	}
	reader.Close();

    }
	
    static public void InsertRecord()
    {

	   Console.WriteLine("What habit have u done?");
	   string habit = Console.ReadLine();

	   string query = "INSERT INTO Habits (Habit, Time) VALUES (@Habit, @Time)";

	   SQLiteCommand command = new SQLiteCommand(query, connection);
		
	  command.Parameters.AddWithValue("@Habit", habit);
	  command.Parameters.AddWithValue("@Time", DateTime.Now);
	  command.ExecuteNonQuery();
    
    }

    static public void DeleteRecord()
    {
	Console.WriteLine("What record do u want to delete?");

	int id = GetInt();

	string query = $"DELETE FROM Habits WHERE Id = {id}";

	SQLiteCommand command = new SQLiteCommand(query, connection);

	command.ExecuteNonQuery();

			
    }

    static public void UpdateRecord()
    {
	int id = GetInt();

	string query = $"UPDATE Habits SET DATETIME = {DateTime.Now} WHERE Id = {id}";

	SQLiteCommand command = new SQLiteCommand(query, connection);

	command.ExecuteNonQuery();

    }

    static public void DrawScreen(){
    	Console.Clear();
	Console.WriteLine("MAIN MENU");
	Console.WriteLine("");
	Console.WriteLine("What would you like to do?");
	Console.WriteLine("");
	Console.WriteLine("Type 0 to close application");
	Console.WriteLine("Type 1 to view all records");
	Console.WriteLine("Type 2 to insert record");
	Console.WriteLine("Type 3 to delete record");
	Console.WriteLine("Type 4 to update record");
	Console.WriteLine("-------------------");

    }
    static public int GetInt()
    {
	    int number;
	    
	    while(!int.TryParse(Console.ReadLine(), out number))
			    {
			    Console.WriteLine("number isnt it correct format, please try again");
			    
			    }	
			   
	    return number;
    }
}

