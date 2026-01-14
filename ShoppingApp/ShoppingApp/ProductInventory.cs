namespace ShoppingApp;

public class ProductInventory
{
    public List<Product> Products = new();
    private uint CurrentProductId; // The id that the next added product is gonna get
    
    public void CreateProduct( string name, float price, uint availableStock, List<ProductCategory> categories)
    {
        Products.Add(new Product(CurrentProductId, name, price, availableStock, categories));
        CurrentProductId++;
    }

    public void DeleteProduct(uint id)
    {
        foreach (var product in Products.ToList())
        {
            if (product.Id == id)
            {
                Products.Remove(product);
                return;
            }
        }
    }

    // Sets the products quantity
    public void SetProductQuantity(uint id, uint quantity)
    {
        var product = GetProduct(id);
        if (product != null)
            product.Quantity = (int)quantity;
    }

    // Adds or removes from the products Quantity
    public void ChangeProductQuantity(uint id, int quantity)
    {
        var product = GetProduct(id);
        if (product != null)
            product.Quantity += quantity;
    }
    
    public Product? GetProduct(uint id)
    {
        foreach (var product in Products)
        {
            if (product.Id == id)
                return product;
        }
        return null;
    }
    
    public Product? GetProduct(string name)
    {
        foreach (var product in Products)
        {
            if (product.Name == name)
                return product;
        }
        return null;
    }
}