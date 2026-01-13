using System;

// Manages user account creation and deletion (admin-only operations)
public class UserManager
{
	private UserRepository userRepository;

	public UserManager(UserRepository repo)
	{
		userRepository = repo;
	}

	// Creates a new user account (admin-only)
	public void CreateUser(User adminUser)
	{
		if (!adminUser.IsAdmin()) return;

		Console.WriteLine("\n--- Create New User ---");
		
		// Prompt for username with validation
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

		// Check if username already exists
		if (userRepository.GetUser(username) != null)
		{
			Console.WriteLine("User already exists.");
			return;
		}

		// Prompt for password with validation
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

		// Prompt for delivery address with validation
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
        
		// Prompt for admin privileges
		Console.WriteLine();
		Console.Write("Is Admin? (y/n): ");
		bool isAdmin = Console.ReadLine().ToLower() == "y";

		// Create and store the new user
		User newUser = new User(username, password, address, isAdmin);
		userRepository.AddUser(newUser);
		Console.WriteLine($"User {username} created successfully.");
	}

	// Deletes a user account (admin-only)
	public void DeleteUser(User adminUser)
	{
		if (!adminUser.IsAdmin()) return;

		Console.Write("\nEnter username to delete: ");
		string username = Console.ReadLine();

		// Prevent admin from deleting their own account
		if (username.ToLower() == adminUser.Username.ToLower())
		{
			Console.WriteLine("You cannot delete your own account.");
			return;
		}

		// Attempt to remove the user
		bool success = userRepository.RemoveUser(username);
		if (success) 
			Console.WriteLine("User deleted.");
		else 
			Console.WriteLine("User not found.");
	}
}