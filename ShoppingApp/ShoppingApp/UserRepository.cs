using System;
using System.Collections.Generic;

// Manages persistent storage and retrieval of user accounts
public class UserRepository
{
	private List<User> users = new List<User>();

	// Initialize with default admin and user accounts
	public UserRepository()
	{
		users.Add(new User("admin", "password", "Pentagon 5", true));
		users.Add(new User("1", "1", "1", false));
		users.Add(new User("Doris", "spaghetti", "Tired Street 1", false));
		users.Add(new User("Carlos", "dogwater", "Boring Avenue 22", false));
		users.Add(new User("Benke", "sweden123", "Winter Boulevard 46", false));
	}

	// Retrieves a user by username (case-insensitive)
	public User? GetUser(string username)
	{
		foreach (User user in users)
		{
			if (user.Username.ToLower() == username.ToLower())
			{
				return user;
			}
		}
		return null;
	}

	// Returns all registered users
	public List<User> GetAllUsers()
	{
		return users;
	}

	// Adds a new user to the repository
	public void AddUser(User user)
	{
		users.Add(user);
	}

	// Removes a user by username (case-insensitive) and returns success status
	public bool RemoveUser(string username)
	{
		User? userToRemove = GetUser(username);
		if (userToRemove != null)
		{
			users.Remove(userToRemove);
			return true;
		}
		return false;
	}
}