using System;
using ShoppingApp;

public class User
{
	public string Username { get; set; }
	public string Password { get; set; }
	public string DeliveryAddress { get; set; }
	public List<Product> Cart = new();
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

	public Product? CartContains(Product product)
	{
		foreach (var cartProduct in Cart)
			if (cartProduct.Name == product.Name)
				return cartProduct;
		return null;
	}
}
