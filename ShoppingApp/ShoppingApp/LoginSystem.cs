using System;
using System.Collections.Generic;



public class LoginSystem
{
	private List<Users> userList = new List<Users>();

	public LoginSystem()
	{
		userList.Add(new Users("admin", "password", "Pentagon 5", true));
		userList.Add(new Users("Doris", "spaghetti", "Tired Street 1", false));
		userList.Add(new Users("Carlos", "dogwater", "Boring Avenue 22", false));
		userList.Add(new Users("Benke", "sweden123", "Winter Boulevard 46", false));
	}


	public Users Authenticate()
	{
		Console.Clear();
		Console.WriteLine("=== User Login ===");

		while (true)
		{
			Console.WriteLine("Enter your username (or leave blank to exit):");
			string inputName = Console.ReadLine();

			if (string.IsNullOrEmpty(inputName))
			{
				Console.WriteLine("Login cancelled.");
				return null;
			}

			Console.WriteLine("Enter your password:");
			string inputPassword = Console.ReadLine();

			foreach (Users user in userList)
			{
				if (user.Namn == inputName && user.Password == inputPassword)
				{
					Console.WriteLine($"Welcome back, {user.Namn}!");
					return user;
                }
			}
			Console.WriteLine("Authentication failed. Invalid username or password.");
            Console.WriteLine();
			
        }
	}
}
