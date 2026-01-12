using System;
using System.Collections.Generic;

public class LoginSystem
{
	private UserRepository userRepository;

	public LoginSystem(UserRepository repo)
	{
		userRepository = repo;
	}

	public User Authenticate()
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

			foreach (User user in userRepository.GetAllUsers())
			{
				if (user.Username.ToLower() == inputName.ToLower() && user.Password == inputPassword)
				{
					Console.WriteLine($"Welcome back, {user.Username}!");
					return user;
				}
			}
			Console.WriteLine("Authentication failed. Invalid username or password.");
			Console.WriteLine();
		}
	}
}
