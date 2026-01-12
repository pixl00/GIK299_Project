using System;

public class Users
{
	public string Namn { get; set; }
	public string Password { get; set; }
	public string Adress { get; set; }

	private bool adminStatus;

	public Users(string namn, string password, string adress, bool isAdmin)
	{
		Namn = namn;
		Password = password;
		Adress = adress;
		adminStatus = isAdmin;
    }

	public bool adminCheck()
		{
		return adminStatus;
		}

}
