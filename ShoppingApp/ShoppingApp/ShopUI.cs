namespace ShoppingApp;

public class ShopUI
{
    private static User? currentUser;

    public static void DisplayMainMenu(ProductInventory inventory, LoginSystem loginSystem, UserManager userManager, UserRepository repository)
    {
        Console.Clear();
        Console.WriteLine("--TEMU--");
        
        if (currentUser != null)
        {
            Console.WriteLine($"Logged in as: {currentUser.Username}");
            Console.WriteLine("-----------------------------");
        }
        
        // Core shopping options available to all users
        Console.WriteLine("[1] View all products");
        Console.WriteLine("[2] Search products");
        Console.WriteLine("[3] Go to cart");

        // Display login/logout and account options based on authentication state
        if (currentUser == null)
        {
            Console.WriteLine();
            Console.WriteLine("[10] Log in");
        }
        else
        {
            Console.WriteLine();
            Console.WriteLine("---[ACCOUNT]---");
            Console.WriteLine("[4] View My Info");
            Console.WriteLine("[5] Update My Info");

            // Admin-only options
            if (currentUser.IsAdmin())
            {
                Console.WriteLine();
                Console.WriteLine("---[ADMIN OPTIONS]---");
                Console.WriteLine("[6] Update User Info");
                Console.WriteLine("[7] Create User");
                Console.WriteLine("[8] Delete User");
                Console.WriteLine("[9] List All Users");
            }
            Console.WriteLine();
            Console.WriteLine("[10] Logout");
        }

        Console.WriteLine("[0] Exit");
        Console.WriteLine();

        Console.Write("Select an option: ");
        string? command = Console.ReadLine();
        
        switch (command)
        {
            case "1":
                DisplayAllProducts(inventory);
                break;
            case "2":
                DisplaySearchProducts(inventory);
                break;
            case "3":
                DisplayCart();
                break;
            case "4":
                // View account info for authenticated users
                    ViewMyInfo();
                break;
            case "5":
                // Update authenticated user's own information
                loginSystem.UpdateUserInfo(currentUser);
                Console.ReadKey();
                break;
            case "6":
                // Admin: Update any user's information
                if (currentUser.IsAdmin())
                {
                    // todo borde inte vara currentUser här \/
                    loginSystem.UpdateUserInfo(currentUser);
                    Console.ReadKey();
                }
                break;
            case "7":
                // Admin: Create a new user account
                    Console.Clear();
                    userManager.CreateUser(currentUser);
                    Console.ReadKey();
                break;
            case "8":
                // Admin: Delete an existing user account
                    Console.Clear();
                    userManager.DeleteUser(currentUser);
                    Console.ReadKey();
                break;
            case "9":
                // Admin: View all registered users
                if (currentUser != null && currentUser.IsAdmin())
                {
                    Console.Clear();
                    Console.WriteLine("\n--- All Users ---");
                    foreach (var u in repository.GetAllUsers())
                    {
                        Console.WriteLine($"- {u.Username} | Address: {u.DeliveryAddress} (Admin: {u.IsAdmin()})");
                    }
                    Console.ReadKey();
                }
                break;
            case "10":
                // Handle login/logout based on current authentication state
                if (currentUser == null)
                {
                    HandleLogin(loginSystem);
                }
                else
                {
                    currentUser = null;
                }
                break;
            case "0":
                Program.Running = false;
                break;
            default:
                Console.WriteLine("Invalid option.");
                Console.ReadKey();
                break;
        }
    }

    // Attempts to authenticate a user through the login system
    private static void HandleLogin(LoginSystem loginSystem)
    {
        currentUser = loginSystem.Authenticate();

        if (currentUser == null)
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }

    // Displays the currently logged-in user's account information
    private static void ViewMyInfo()
    {
        if(currentUser == null)
            return;
        
        Console.Clear();
        Console.WriteLine($"\nUser: {currentUser.Username}");
        Console.WriteLine($"Address: {currentUser.DeliveryAddress}");
        Console.WriteLine("Press any key...");
        Console.ReadKey();
    }

    private static void DisplayCart()
    {
        Console.Clear();
        Console.WriteLine("Cart functionality coming soon!");
        Console.ReadKey();
    }

    // Formats the price to 2 decimal places, but without rounding up (e.g., 29,995 -> 29.99)
    private static string FormatPrice(float price)
    {
        // Cast to decimal data type for Math.Truncate to work correctly
        // Math.Floor only works with whole numbers, so we multiply by 100 first so we're working with whole numbers, format it, then divide by 100 again to get the correct decimal placement.
        decimal truncated = Math.Floor((decimal)price * 100) / 100;
        return truncated.ToString("F2");
    }

    // Displays product details including name, price, and stock information
    // Shows original and discounted prices if a sale is active
    public static void DisplayProduct(Product product)
    {
        Console.WriteLine(product.Name);
        if (product.HasReducedPrice)
        {
            Console.WriteLine();
            Console.WriteLine($"Original price: ${FormatPrice(product.Price)}");
            Console.WriteLine($"Current price: ${FormatPrice(product.ReducedPrice)} ({product.ReducedPercent}% off!)");
        }
        else
        {
            Console.WriteLine();
            Console.WriteLine($"Current price: ${FormatPrice(product.Price)}");
        }
        Console.WriteLine();
        Console.WriteLine($"in stock: {product.Quantity}");
    }
    
    // Displays a paginated list of all products with the ability to view individual product details
    public static void DisplayAllProducts(ProductInventory inventory)
    {
        Console.Clear();
        Console.WriteLine("[0] Return to main menu");
        Console.WriteLine("--------------------------------------------------------");
        
        var products = inventory.Products;
        for (int i = 0; i < products.Count; i++)
        {
            Console.Write($"[{i+1}] ");
            DisplayProduct(products[i]);
            Console.WriteLine("--------------------------------------------------------");
        }
        string? command = Console.ReadLine();
        if (command == "0")
            return;
        
        int.TryParse(command, out int index);
        if (index < 1 || index > products.Count)
            return;

        // Display selected product details
        Console.Clear();
        DisplayProduct(products[index-1]);
        Console.WriteLine("[1] Add to cart");
        Console.WriteLine("[2] Return to main menu");
        command = Console.ReadLine();
        if (command == "1")
        {
            // TODO: Implement add to cart functionality
        }
        else if (command == "2")
        {
            return;
        }
    }
    
    // Searches products by name or category and displays matching results
    public static void DisplaySearchProducts(ProductInventory inventory)
    {
        Console.Clear();
        string? search = null;
        while (string.IsNullOrWhiteSpace(search))
        {
            Console.Clear();
            Console.Write("Enter a search term : ");
            search = Console.ReadLine();
        }
        
        Console.WriteLine("Choose a product");
        Console.WriteLine("--------------------------------------------------------");
        List<Product> products = new();
  
        // Filter products matching search term in name or category
        for (int i = 0; i < inventory.Products.Count; i++)
        {
            if (!inventory.Products[i].Name.ToLower().Contains(search.ToLower()))
            {
                bool categoryContains = false;
                foreach (var category in inventory.Products[i].Categories)
                {
                    if (category.ToString().ToLower().Contains(search.ToLower()))
                    {
                        categoryContains = true;
                        break;
                    }
                }
                if(!categoryContains)
                    continue;
            }
            
            products.Add(inventory.Products[i]);
        }

        // Display "no results" message if search yields no matches
        if (products.Count == 0)
        {
            Console.Clear();
            Console.WriteLine("No products found");
            Console.ReadLine();
            return;
        }
        
        // Display search results
        for (int i = 0; i < products.Count; i++)
        {
            Console.Write($"[{i+1}]");
            DisplayProduct(products[i]);
            Console.WriteLine("--------------------------------------------------------");
        }
        string? command = Console.ReadLine();
        if (command == "0")
            return;
        
        int.TryParse(command, out int index);
        if (index < 1 || index > products.Count)
            return;

        // Allow user to add selected product to cart
        while (true)
        {
            Console.Clear();
            DisplayProduct(products[index-1]);
            Console.WriteLine("[1] Add to cart");
            Console.WriteLine("[2] Return to main menu");
            command = Console.ReadLine();
            if (command == "1")
            {
                return;
            }
            if (command == "2")
            {
                return;
            }
        }
    }
}