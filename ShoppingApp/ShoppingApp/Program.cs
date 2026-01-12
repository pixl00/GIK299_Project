namespace ShoppingApp;

class Program
{
    static void Main(string[] args)
    {
        // 1. Initialize User Repository and Managers
        UserRepository repository = new UserRepository();

        // 2. Login System
        LoginSystem loginSystem = new LoginSystem(repository);
        UserManager userManager = new UserManager(repository);

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

            // Only show admin options if user is admin
            if (currentUser.IsAdmin())
            {
                Console.WriteLine("[ADMIN OPTIONS]");
                Console.WriteLine("2. Create User");
                Console.WriteLine("3. Delete User");
                Console.WriteLine("4. List All Users");
            }

            Console.WriteLine("0. Logout / Exit");
            Console.Write("Select an option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.WriteLine($"\nUser: {currentUser.Username}");
                    Console.WriteLine($"Address: {currentUser.DeliveryAddress}");
                    Console.WriteLine("Press any key...");
                    Console.ReadKey();
                    break;
                case "2":
                    userManager.CreateUser(currentUser);
                    Console.ReadKey();
                    break;
                case "3":
                    userManager.DeleteUser(currentUser);
                    Console.ReadKey();
                    break;
                case "4":
                    if (currentUser.IsAdmin())
                    {
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