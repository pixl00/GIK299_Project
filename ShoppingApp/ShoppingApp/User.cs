using System;

public class User
{
	public string Username { get; set; }
	public string Password { get; set; }
	public string DeliveryAddress { get; set; }
	private bool adminStatus;

	public User(string username, string password, string deliveryAddress, bool isAdmin)
	{
		Username = username;
		Password = password;
		DeliveryAddress = deliveryAddress;
		adminStatus = isAdmin;
	}

	public bool IsAdmin()
	{
		return adminStatus;
	}
}
