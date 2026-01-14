namespace ShoppingApp;

// Product categories for organizing inventory
public enum ProductCategory
{
    NONE,
    GAMING,
    AUDIO,
    STORAGE,
    ACCESSORIES
}

// Represents a product in the inventory with pricing and discount functionality
public class Product
{
    public Product(uint id, string name, float price, uint quantity, ProductCategory category)
    {
        Id = (int)id;
        Name = name;
        Price = price;
        Quantity = (int)quantity;
        Categories.Add(category);
    }
    
    // Copy constructor
    public Product(Product product)
    {
        Id = product.Id;
        Name = product.Name;
        Price = product.Price;
        Quantity = product.Quantity;
        Categories = product.Categories;
        LinkedItem = product.LinkedItem;
        HasReducedPrice = product.HasReducedPrice;
        ReducedPercent = product.ReducedPercent;
        ReducedPrice = product.ReducedPrice;
    }

    public int Id;
    public string Name;
    public float Price;
    public int Quantity;
    public List<ProductCategory> Categories = new();
    public Product? LinkedItem; // Recommended product to pair with this item
    
    // Sale pricing
    public bool HasReducedPrice = false;
    public float ReducedPercent;
    public float ReducedPrice;
    

    // Returns the appropriate price based on sale status
    public float GetPrice()
    {
        return HasReducedPrice ? ReducedPrice : Price;
    }
    
    // Applies a discount percentage to the product and marks it as on sale
    public void SetReducedPrice(float percentage)
    {
        ReducedPercent = percentage;
        ReducedPrice = Price * (percentage * 0.01f);
        HasReducedPrice = percentage < 100f;
    }
}