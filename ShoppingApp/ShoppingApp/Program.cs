namespace ShoppingApp;

class Program
{
    public static bool Running = true;
    static void Main(string[] args)
    {
        ProductInventory inventory = new ProductInventory();
        inventory.CreateProduct("Gaming mouse", 59.99f, 10, ProductCategory.GAMING);
        inventory.CreateProduct("Ergonomic mouse", 89.99f, 15, ProductCategory.OFFICE);
        
        inventory.Products[0].SetReducedPrice(50f);
        
        // 1. Initialize User Repository and Managers
        UserRepository repository = new UserRepository();

        // 2. Login System
        LoginSystem loginSystem = new LoginSystem(repository);
        UserManager userManager = new UserManager(repository);
        
        while (Running)
        {
            ShopUI.DisplayMainMenu(inventory);
        }

        User currentUser = null;

        // 3. User Authentication Loop
        while (currentUser == null)
        {
            currentUser = loginSystem.Authenticate();

            if (currentUser == null)
            {
                Console.WriteLine("Press any key to try again...");
                Console.ReadKey();
            }
        }

        // 4. Main Application Loop
        bool isRunning = true;
        while (isRunning)
        {
            Console.Clear();
            Console.WriteLine($"Welcome, {currentUser.Username}!");
            Console.WriteLine($"Current Address: {currentUser.DeliveryAddress}");
            Console.WriteLine("-----------------------------");
            Console.WriteLine("1. View My Info");

            // Only show non-admin options if user is not admin
            if (!currentUser.IsAdmin())
            {
                Console.WriteLine("2. Update My Info");
            }

            // Only show admin options if user is admin
            if (currentUser.IsAdmin())
            {
                Console.WriteLine();
                Console.WriteLine("---[ADMIN OPTIONS]---");
                Console.WriteLine("2. Update User Info");
                Console.WriteLine("3. Create User");
                Console.WriteLine("4. Delete User");
                Console.WriteLine("5. List All Users");
            }

            Console.WriteLine("0. Logout / Exit");
            Console.WriteLine();
            Console.Write("Select an option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Clear();
                    Console.WriteLine($"\nUser: {currentUser.Username}");
                    Console.WriteLine($"Address: {currentUser.DeliveryAddress}");
                    Console.WriteLine("Press any key...");
                    Console.ReadKey();
                    break;
                case "2":
                    if (!currentUser.IsAdmin())
                    {
                        loginSystem.UpdateUserInfo(currentUser);
                    }
                    else
                    {
                        loginSystem.UpdateUserInfo(currentUser);
                    }
                    Console.ReadKey();
                    break;
                case "3":
                    if (currentUser.IsAdmin())
                    {
                        Console.Clear();
                        userManager.CreateUser(currentUser);
                        Console.ReadKey();
                    }
                    break;
                case "4":
                    if (currentUser.IsAdmin())
                    {
                        Console.Clear();
                        userManager.DeleteUser(currentUser);
                        Console.ReadKey();
                    }
                    break;
                case "5":
                    if (currentUser.IsAdmin())
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
                case "0":
                    isRunning = false;
                    break;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }

        Console.WriteLine("Goodbye!");
    }
}