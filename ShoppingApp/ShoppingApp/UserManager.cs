using System;

public class UserManager
{
	private UserRepository userRepository;

	public UserManager(UserRepository repo)
	{
		userRepository = repo;
	}

	public void CreateUser(User adminUser)
	{
		if (!adminUser.IsAdmin()) return;

		Console.WriteLine("\n--- Create New User ---");
		Console.Write("Username: ");
		string username = Console.ReadLine();

		if (userRepository.GetUser(username) != null)
		{
			Console.WriteLine("User already exists.");
			return;
		}

		Console.Write("Password: ");
		string password = Console.ReadLine();

		Console.Write("Address: ");
		string address = Console.ReadLine();

		Console.Write("Is Admin? (y/n): ");
		bool isAdmin = Console.ReadLine().ToLower() == "y";

		User newUser = new User(username, password, address, isAdmin);
		userRepository.AddUser(newUser);
		Console.WriteLine($"User {username} created successfully.");
	}

	public void DeleteUser(User adminUser)
	{
		if (!adminUser.IsAdmin()) return;

		Console.Write("\nEnter username to delete: ");
		string username = Console.ReadLine();

		if (username.ToLower() == adminUser.Username.ToLower())
		{
			Console.WriteLine("You cannot delete your own account.");
			return;
		}

		bool success = userRepository.RemoveUser(username);
		if (success) Console.WriteLine("User deleted.");
		else Console.WriteLine("User not found.");
	}
}