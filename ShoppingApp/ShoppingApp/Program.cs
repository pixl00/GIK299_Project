namespace ShoppingApp;

class Program
{
    static void Main(string[] args)
    {
        LoginSystem loginSystem = new LoginSystem();
        var user = loginSystem.Authenticate();
        if (user != null)
        {
            if (user.adminCheck())
            {
                Console.WriteLine("You have admin privileges.");
                Console.ReadKey();
                // Admin-specific functionality can be added here
            }
            else
            {
                Console.WriteLine("You are logged in as a regular user.");
                Console.ReadKey();
                // Regular user functionality can be added here
            }
        }
    }
}
