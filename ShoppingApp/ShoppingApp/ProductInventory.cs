namespace ShoppingApp;

public class ProductInventory
{
    private Dictionary<uint, Product?> Products = new();
    private uint CurrentProductId; // The id that the next added product is gonna get
    
    public void CreateProduct( string name, float price, uint availableStock, ProductCategory category)
    {
        Products.Add(CurrentProductId ,new Product(CurrentProductId, name, price, availableStock, category));
        CurrentProductId++;
    }

    public void DeleteProduct(uint id)
    {
        Products.Remove(id);
    }

    // Sets the products quantity
    public void SetProductQuantity(uint id, uint quantity)
    {
        Products.TryGetValue(id, out Product? product);
        if (product != null)
            product.Quantity = (int)quantity;
    }

    // Adds or removes from the products Quantity
    public void ChangeProductQuantity(uint id, int quantity)
    {
        Products.TryGetValue(id, out Product? product);
        if (product != null)
            product.Quantity += quantity;
    }
    
    public Product? GetProduct(uint id)
    {
        Products.TryGetValue(id, out var product);
        return product;
    }
    
    
}