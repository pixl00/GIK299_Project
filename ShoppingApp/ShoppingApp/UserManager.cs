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
		
		string username;
		while (true)
		{
            Console.WriteLine();
			Console.Write("Username: ");
			username = Console.ReadLine();
			
			if (!string.IsNullOrEmpty(username))
			{
				break;
			}
			Console.WriteLine("Username cannot be empty.");
		}

		if (userRepository.GetUser(username) != null)
		{
			Console.WriteLine("User already exists.");
			return;
		}

		string password;
		while (true)
		{
            Console.WriteLine();
			Console.Write("Password: ");
			password = Console.ReadLine();
			
			if (!string.IsNullOrEmpty(password))
			{
				break;
			}
			Console.WriteLine("Password cannot be empty.");
		}

		string address;
		while (true)
		{
            Console.WriteLine();
			Console.Write("Address: ");
			address = Console.ReadLine();
			
			if (!string.IsNullOrEmpty(address))
			{
				break;
			}
			Console.WriteLine("Address cannot be empty.");
		}
        Console.WriteLine();
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