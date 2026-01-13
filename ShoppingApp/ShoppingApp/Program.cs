namespace ShoppingApp;

class Program
{
    public static bool Running = true;
    
    static void Main(string[] args)
    {
        // Initialize product inventory with sample products
        ProductInventory inventory = new ProductInventory();
        inventory.CreateProduct("Gaming mouse", 59.99f, 10, ProductCategory.GAMING);
        inventory.CreateProduct("Ergonomic mouse", 89.99f, 15, ProductCategory.OFFICE);
        
        // Apply 50% discount to first product
        inventory.Products[0].SetReducedPrice(50f);
        
        // Initialize user repository and system components
        UserRepository repository = new UserRepository();
        LoginSystem loginSystem = new LoginSystem(repository);
        UserManager userManager = new UserManager(repository);
        
        // Main application loop
        while (Running)
        {
            ShopUI.DisplayMainMenu(inventory, loginSystem, userManager, repository);
        }

        Console.WriteLine("Goodbye!");
    }
}