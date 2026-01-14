using System;
using System.Collections.Generic;

// Handles user authentication and profile management
public class LoginSystem
{
	private UserRepository userRepository;

	public LoginSystem(UserRepository repo)
	{
		userRepository = repo;
	}

	// Prompts user for credentials and returns authenticated user or null if login fails
	public User? Authenticate()
	{
		Console.Clear();
		Console.WriteLine("=== User Login ===");
        Console.WriteLine();

		while (true)
		{
			Console.Write("Enter your username (or leave blank to exit): ");
			string? inputName = Console.ReadLine();
            Console.WriteLine();

			if (string.IsNullOrEmpty(inputName))
			{
				Console.WriteLine("Login cancelled.");
				return null;
			}
			Console.Write("Enter your password: ");
			string? inputPassword = Console.ReadLine();
			Console.WriteLine();

			// Check credentials against all users (username is case-insensitive)
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

	// Allows users to update their account information (admins can update any user)
	public void UpdateUserInfo(User? currentUser)
	{
		if(currentUser == null)
			return;
		
		Console.Clear();
        User? userToUpdate;

		// Determine which user to update based on admin status
		if (currentUser.IsAdmin())
		{
			Console.WriteLine("\n--- Update User Information ---");
			Console.Write("Enter username to update: ");
			string? username = Console.ReadLine();

			if (string.IsNullOrEmpty(username))
			{
				Console.WriteLine("Username cannot be empty.");
				return;
			}

			// Search for user with case-insensitive username matching
			userToUpdate = null;
			foreach (User user in userRepository.GetAllUsers())
			{
				if (user.Username.ToLower() == username.ToLower())
				{
					userToUpdate = user;
					break;
				}
			}

			if (userToUpdate == null)
			{
				Console.WriteLine("User not found.");
				return;
			}
		}
		else
		{
            // Non-admin users can only update their own account
            userToUpdate = currentUser;
		}

		Console.WriteLine($"\nUpdating user: {userToUpdate.Username}");
		Console.WriteLine("Leave blank to keep current value.");

		// Store original values for rollback if user cancels
		string originalPassword = userToUpdate.Password;
		string originalAddress = userToUpdate.DeliveryAddress;

		// Prompt for new password
		Console.WriteLine();
		Console.Write($"New password (current: {userToUpdate.Password}): ");
		string? newPassword = Console.ReadLine();
		if (!string.IsNullOrEmpty(newPassword))
		{
			userToUpdate.Password = newPassword;
		}

        // Prompt for new delivery address
        Console.WriteLine();
		Console.Write($"New address (current: {userToUpdate.DeliveryAddress}): ");
		string? newAddress = Console.ReadLine();
		if (!string.IsNullOrEmpty(newAddress))
		{
			userToUpdate.DeliveryAddress = newAddress;
		}

		// Show confirmation dialog with changes
		Console.Clear();
        Console.WriteLine("\n--- Confirm Changes ---");
		Console.WriteLine($"Username: {userToUpdate.Username}");
		Console.WriteLine($"Password: {originalPassword} -> {userToUpdate.Password}");
		Console.WriteLine($"Address: {originalAddress} -> {userToUpdate.DeliveryAddress}");
		
		string? confirmation = null;
		while (string.IsNullOrWhiteSpace(confirmation))
		{
			Console.Write("\nApply changes? (y/n): ");
			confirmation= Console.ReadLine();
		}
		
		if (confirmation.ToLower() == "y")
		{
			Console.WriteLine($"User {userToUpdate.Username} updated successfully.");
		}
		else
		{
			// Revert to original values if user cancels
			userToUpdate.Password = originalPassword;
			userToUpdate.DeliveryAddress = originalAddress;
			Console.WriteLine("Changes cancelled.");
		}
	}
}
