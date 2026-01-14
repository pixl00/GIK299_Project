namespace ShoppingApp;

class Program
{
    public static bool Running = true;
    
    static void Main(string[] args)
    {
        // Initialize product inventory 
        ProductInventory inventory = new ProductInventory();
        
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