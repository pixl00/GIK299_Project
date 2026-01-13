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
        Console.WriteLine();

		while (true)
		{
			Console.Write("Enter your username (or leave blank to exit): ");
			string inputName = Console.ReadLine();
            Console.WriteLine();

			if (string.IsNullOrEmpty(inputName))
			{
				Console.WriteLine("Login cancelled.");
				return null;
			}
			Console.Write("Enter your password: ");
			string inputPassword = Console.ReadLine();
			Console.WriteLine();

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

	public void UpdateUserInfo(User currentUser)
	{
		Console.Clear();
        User userToUpdate;

		// Determine which user to update
		if (currentUser.IsAdmin())
		{
			Console.WriteLine("\n--- Update User Information ---");
			Console.Write("Enter username to update: ");
			string username = Console.ReadLine();

			if (string.IsNullOrEmpty(username))
			{
				Console.WriteLine("Username cannot be empty.");
				return;
			}

			// Case-insensitive user search
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
            // Regular users gets their own username picked automatically
            userToUpdate = currentUser;
		}

		Console.WriteLine($"\nUpdating user: {userToUpdate.Username}");
		Console.WriteLine("Leave blank to keep current value.");

		// Store original values in case of revert
		string originalPassword = userToUpdate.Password;
		string originalAddress = userToUpdate.DeliveryAddress;

		// Update Password
		Console.WriteLine();
		Console.Write($"New password (current: {userToUpdate.Password}): ");
		string newPassword = Console.ReadLine();
		if (!string.IsNullOrEmpty(newPassword))
		{
			userToUpdate.Password = newPassword;
		}

        // Update Address
        Console.WriteLine();
		Console.Write($"New address (current: {userToUpdate.DeliveryAddress}): ");
		string newAddress = Console.ReadLine();
		if (!string.IsNullOrEmpty(newAddress))
		{
			userToUpdate.DeliveryAddress = newAddress;
		}

		// Confirmation window
		Console.Clear();
        Console.WriteLine("\n--- Confirm Changes ---");
		Console.WriteLine($"Username: {userToUpdate.Username}");
		Console.WriteLine($"Password: {originalPassword} -> {userToUpdate.Password}");
		Console.WriteLine($"Address: {originalAddress} -> {userToUpdate.DeliveryAddress}");
		Console.Write("\nApply changes? (y/n): ");
		string confirmation = Console.ReadLine();

		if (confirmation.ToLower() == "y")
		{
			Console.WriteLine($"User {userToUpdate.Username} updated successfully.");
		}
		else
		{
			// Revert changes
			userToUpdate.Password = originalPassword;
			userToUpdate.DeliveryAddress = originalAddress;
			Console.WriteLine("Changes cancelled.");
		}
	}
}
