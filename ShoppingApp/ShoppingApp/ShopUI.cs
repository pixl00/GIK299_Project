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
                DisplayAllProducts(inventory.Products);
                break;
            case "2":
                DisplaySearchProducts(inventory.Products);
                break;
            case "3":
                DisplayCartProducts(currentUser!.Cart);
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
                if (currentUser != null && currentUser.IsAdmin())
                {
                    loginSystem.UpdateUserInfo(currentUser);
                    Console.ReadKey();
                }
                break;
            case "7":
                // Admin: Create a new user account
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
        if (currentUser == null)
            return;

        Console.Clear();
        Console.WriteLine($"\nUser: {currentUser.Username}");
        Console.WriteLine($"Address: {currentUser.DeliveryAddress}");
        Console.WriteLine("Press any key...");
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
    public static void DisplayAllProducts(List<Product> products)
    {
        Console.Clear();
        Console.WriteLine("[0] Return to main menu");
        Console.WriteLine("--------------------------------------------------------");

        for (int i = 0; i < products.Count; i++)
        {
            Console.Write($"[{i + 1}] ");
            DisplayProduct(products[i]);
            Console.WriteLine("--------------------------------------------------------");
        }
        string? command = Console.ReadLine();
        if (command == "0")
            return;

        int.TryParse(command, out int index);
        if (index > products.Count)
            return;

        // Display selected product details
        Console.Clear();
        Product product = products[index - 1];
        DisplayProduct(product);
        Console.WriteLine("[0] Return to main menu");
        Console.WriteLine("[1] Add to cart");
        command = Console.ReadLine();
        if (command == "0")
        {
            return;
        }
        if (command == "1")
        {
            // TODO: Implement add to cart functionality
            if (currentUser == null)
            {
                Console.WriteLine("You have to log in to add items to your cart");
                Console.ReadKey();
                return;
            }

            if (product.Quantity <= 0)
            {
                Console.WriteLine("This item is out of stock");
                Console.ReadKey();
                return;
            }

            int quantity;
            while (true)
            {
                Console.WriteLine($"Enter quantity (Max: {product.Quantity}): ");
                bool success = int.TryParse(Console.ReadLine(), out quantity);
                if (success && quantity <= product.Quantity && quantity >= 1)
                    break;
            }

            currentUser.AddToCart(product, quantity);
            Console.WriteLine("Item added to cart!");

            // Prompt to add linked item if available and in stock
            if (product.LinkedItem != null && product.LinkedItem.Quantity > 0)
            {
                Console.WriteLine();
                Console.WriteLine($"Would you like to add the recommended item '{product.LinkedItem.Name}' to your cart?");
                Console.WriteLine("[1] Yes");
                Console.WriteLine("[0] No");
                Console.Write("\nSelect an option: ");
                string? linkedItemCommand = Console.ReadLine();

                if (linkedItemCommand == "1")
                {
                    int linkedQuantity;
                    while (true)
                    {
                        Console.WriteLine($"Enter quantity for {product.LinkedItem.Name} (Max: {product.LinkedItem.Quantity}): ");
                        bool success = int.TryParse(Console.ReadLine(), out linkedQuantity);
                        if (success && linkedQuantity <= product.LinkedItem.Quantity && linkedQuantity >= 1)
                            break;
                    }

                    currentUser.AddToCart(product.LinkedItem, linkedQuantity);
                    Console.WriteLine("\nRecommended item added to cart!");
                }
            }

            Console.ReadKey();
            return;
        }
    }

    // Searches products by name or category and displays matching results
    public static void DisplaySearchProducts(List<Product> products)
    {
        Console.Clear();
        string? search = null;
        while (string.IsNullOrWhiteSpace(search))
        {
            Console.Clear();
            Console.WriteLine("Available categories: \nGAMING | AUDIO | STORAGE | ACCESSORIES\n--------------------------------------------------------\n  ");
            Console.Write("Enter a search term : ");
            search = Console.ReadLine();
        }

        Console.WriteLine("Choose a product");
        Console.WriteLine("--------------------------------------------------------");
        List<Product> searchedProducts = new();

        // Filter products matching search term in name or category
        for (int i = 0; i < products.Count; i++)
        {
            if (!products[i].Name.ToLower().Contains(search.ToLower()))
            {
                bool categoryContains = false;
                foreach (var category in products[i].Categories)
                {
                    if (category.ToString().ToLower().Contains(search.ToLower()))
                    {
                        categoryContains = true;
                        break;
                    }
                }
                if (!categoryContains)
                    continue;
            }

            searchedProducts.Add(products[i]);
        }

        // Display "no results" message if search yields no matches
        if (searchedProducts.Count == 0)
        {
            Console.Clear();
            Console.WriteLine("No products found");
            Console.ReadLine();
            return;
        }

        // Display search results
        for (int i = 0; i < searchedProducts.Count; i++)
        {
            Console.Write($"[{i + 1}]");
            DisplayProduct(searchedProducts[i]);
            Console.WriteLine("--------------------------------------------------------");
        }
        string? command = Console.ReadLine();
        if (command == "0")
            return;

        int.TryParse(command, out int index);
        if (index < 1 || index > searchedProducts.Count)
            return;
        Product product = searchedProducts[index - 1];
        // Allow user to add selected product to cart
        while (true)
        {
            Console.Clear();
            DisplayProduct(product);
            Console.WriteLine("[0] Return to main menu");
            Console.WriteLine("[1] Add to cart");
            command = Console.ReadLine();
            if (command == "0")
            {
                return;
            }
            if (command == "1")
            {
                if (currentUser == null)
                {
                    Console.WriteLine("You have to log in to add items to your cart");
                    Console.ReadKey();
                    return;
                }

                int quantity;
                while (true)
                {
                    Console.WriteLine($"Enter quantity (Max: {product.Quantity}): ");
                    bool success = int.TryParse(Console.ReadLine(), out quantity);
                    if (success && quantity <= product.Quantity && quantity >= 1)
                        break;
                }

                currentUser.AddToCart(product, quantity);
                return;
            }
        }
    }

    public static void DisplayCartProduct(Product product)
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
        Console.WriteLine($"Quantity: {product.Quantity}");

        // Show total price
        float itemPrice = product.HasReducedPrice ? product.ReducedPrice : product.Price;
        float itemTotalPrice = itemPrice * product.Quantity;
        Console.WriteLine($"Total Price: ${FormatPrice(itemTotalPrice)}");

        if (product.LinkedItem != null)
        {
            Console.WriteLine($"Related product: {product.LinkedItem.Name}");
        }
    }

    public static void DisplayCartProducts(List<Product> cart)
    {
        Console.Clear();
        if (currentUser == null)
        {
            Console.WriteLine("You have to log in to view your cart");
            Console.ReadLine();
            return;
        }

        Console.Clear();
        Console.WriteLine("[0] Return to main menu");
        Console.WriteLine($"{currentUser.Username}'s cart");
        Console.WriteLine("--------------------------------------------------------");

        for (int i = 0; i < cart.Count; i++)
        {
            Console.Write($"[{i + 1}] ");
            DisplayCartProduct(cart[i]);
            Console.WriteLine("--------------------------------------------------------");
        }

        // Calculate and display cart totals
        float totalPrice = 0;
        float totalDiscount = 0;

        foreach (var item in cart)
        {
            float itemPrice = item.HasReducedPrice ? item.ReducedPrice : item.Price;
            totalPrice += itemPrice * item.Quantity;

            if (item.HasReducedPrice)
            {
                float discountPerItem = item.Price - item.ReducedPrice;
                totalDiscount += discountPerItem * item.Quantity;
            }
        }

        Console.WriteLine();
        Console.WriteLine($"Total items: {cart.Count}");
        if (totalDiscount > 0)
        {
            Console.WriteLine($"Total discount savings: ${FormatPrice(totalDiscount)}");
        }
        Console.WriteLine($"\nTotal price: ${FormatPrice(totalPrice)}");
        Console.WriteLine("--------------------------------------------------------");
        Console.WriteLine();

        string? command = Console.ReadLine();
        if (command == "0")
            return;

        if (!int.TryParse(command, out int index) || index <= 0 || index > cart.Count)
            return;

        Product product = cart[index - 1];
        Console.WriteLine("[0] Return to main menu");
        Console.WriteLine("[1] Remove from cart");
        if (product.LinkedItem != null)
            Console.WriteLine("[2] Add recommended product to cart");
        command = Console.ReadLine();
        if (command == "")
        {
            return;
        }
        if (command == "1")
        {
            if (product.Quantity == 1)
            {
                cart.Remove(product);
                Console.WriteLine("Item removed");
                Console.ReadKey();
                return;
            }

            while (true)
            {
                Console.Write($"How many items do you want to remove (Max:{product.Quantity}): ");
                bool parsed = int.TryParse(Console.ReadLine(), out int remove);
                if (parsed)
                {
                    product.Quantity -= remove;
                    if (product.Quantity <= 0)
                    {
                        cart.Remove(product);
                    }
                    Console.WriteLine("Items removed");
                    Console.ReadKey();
                    return;
                }
            }
        }
        if (command == "2" && product.LinkedItem != null)
        {
            int quantity;
            while (true)
            {
                Console.WriteLine($"Enter quantity (Max: {product.LinkedItem.Quantity}): ");
                bool success = int.TryParse(Console.ReadLine(), out quantity);
                if (success && quantity <= product.LinkedItem.Quantity && quantity >= 1)
                    break;
            }

            User.AddToCart(cart, product.LinkedItem, quantity);
            return;
        }
    }
}