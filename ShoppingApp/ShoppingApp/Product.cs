namespace ShoppingApp;

public enum ProductCategory
{
    NONE,
    OFFICE,
    GAMING
}

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

    public int Id;
    public string Name;
    public float Price;
    public int Quantity;
    public List<ProductCategory> Categories = new();
    
    public bool HasReducedPrice = false;
    public float ReducedPercent;
    public float ReducedPrice;
}