namespace ShoppingApp;

public class ProductInventory
{
    public List<Product> Products = new();
    private uint CurrentProductId; // The id that the next added product is gonna get
    
    public ProductInventory()
    {
        InitializeProducts();
    }

    private void InitializeProducts()
    {
        // Gaming products
        CreateProduct("Gaming mouse", 59.99f, 10, ProductCategory.Gaming);
        CreateProduct("Mechanical keyboard RGB", 149.99f, 8, ProductCategory.Gaming);
        CreateProduct("Gaming headset 7.1", 119.99f, 12, ProductCategory.Gaming);
        CreateProduct("Gaming monitor 240Hz", 399.99f, 5, ProductCategory.Gaming);
       
        
        // Audio products
        CreateProduct("Wireless headphones", 79.99f, 25, ProductCategory.Audio);
        CreateProduct("Bluetooth speaker", 39.99f, 30, ProductCategory.Audio);
        CreateProduct("Studio microphone", 89.99f, 14, ProductCategory.Audio);
        
        // Storage products
        CreateProduct("SSD 1TB NVMe", 99.99f, 16, ProductCategory.Storage);
        CreateProduct("External HDD 2TB", 79.99f, 12, ProductCategory.Storage);
        CreateProduct("USB flash drive 64GB", 24.99f, 45, ProductCategory.Storage);
        
        // Accessories products
        CreateProduct("USB-C cable 6ft", 12.99f, 50, ProductCategory.Accessories);
        CreateProduct("HDMI cable 10ft", 14.99f, 40, ProductCategory.Accessories);
        CreateProduct("Cable organizer kit", 19.99f, 35, ProductCategory.Accessories);
        CreateProduct("Mouse pad large", 24.99f, 50, ProductCategory.Accessories);
        CreateProduct("Keyboard wrist rest", 29.99f, 22, ProductCategory.Accessories);
        
        // Apply discounts to selected products
        Products[0].SetReducedPrice(50f);
        Products[2].SetReducedPrice(25f);
        Products[5].SetReducedPrice(35f);
        Products[9].SetReducedPrice(15f);
        Products[11].SetReducedPrice(40f);

        // Link items to their recommended products
        Products[0].LinkedItem = Products[13]; // Gaming mouse -> Mouse pad
        Products[1].LinkedItem = Products[14]; // Mechanical keyboard -> Keyboard wrist rest
        Products[2].LinkedItem = Products[10]; // Gaming headset -> USB-C cable
        Products[3].LinkedItem = Products[11]; // Gaming monitor -> HDMI cable
        Products[4].LinkedItem = Products[10]; // Wireless headphones -> USB-C cable
        Products[6].LinkedItem = Products[10]; // Studio microphone -> USB-C cable
        Products[7].LinkedItem = Products[10]; // SSD -> USB-C cable
        Products[8].LinkedItem = Products[10]; // External HDD -> USB-C cable
    }
    
    public void CreateProduct(string name, float price, uint availableStock, ProductCategory category)
    {
        Products.Add(new Product(CurrentProductId, name, price, availableStock, category));
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

    // Returns the linked item for a product if it's in stock
    public Product? GetRecommendedProduct(Product product)
    {
        if (product.LinkedItem != null && product.LinkedItem.Quantity > 0)
        {
            return product.LinkedItem;
        }
        return null;
    }
}