using System;
using System.Collections.Generic;

public class UserRepository
{
	private List<User> users = new List<User>();

	public UserRepository()
	{

		users.Add(new User("admin", "password", "Pentagon 5", true));
		users.Add(new User("Doris", "spaghetti", "Tired Street 1", false));
		users.Add(new User("Carlos", "dogwater", "Boring Avenue 22", false));
		users.Add(new User("Benke", "sweden123", "Winter Boulevard 46", false));
	}

	public User GetUser(string username)
	{
		foreach (User user in users)
		{
			if (user.Username == username)
			{
				return user;
			}
		}
		return null;
	}

	public List<User> GetAllUsers()
	{
		return users;
	}

	public void AddUser(User user)
	{
		users.Add(user);
	}

	public bool RemoveUser(string username)
	{
		User userToRemove = GetUser(username);
		if (userToRemove != null)
		{
			users.Remove(userToRemove);
			return true;
		}
		return false;
	}
}