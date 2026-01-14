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
		return CartContains(Cart, product);
	}
	
	// Static function for checking if a cart contains a product
	public static Product? CartContains(List<Product> cart, Product product)
	{
		foreach (var cartProduct in cart)
			if (cartProduct.Name == product.Name)
				return cartProduct;
		return null;
	}
	
	public void AddToCart(Product product, int quantity)
	{
		AddToCart(Cart, product, quantity);
	}

	// Static function for adding products to a specific cart instead of a user
	public static void AddToCart(List<Product> cart, Product product, int quantity)
	{
		Product? cartProduct = CartContains(cart, product);
		if (cartProduct != null)
		{
			cartProduct.Quantity += quantity;
		}
		else
		{
			Product newProduct = new Product(product);
			newProduct.Quantity = quantity;
			cart.Add(newProduct);
		}
	}
}
