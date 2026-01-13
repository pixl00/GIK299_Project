namespace ShoppingApp;

public class ShopUI
{
    public static void DisplayMainMenu(ProductInventory inventory)
    {
        Console.Clear();
        Console.WriteLine("--TEMU--");
        Console.WriteLine("[1] View all products");
        Console.WriteLine("[2] Search products");
        Console.WriteLine("[3] Go to cart");
        Console.WriteLine("[4] Log in");
        Console.WriteLine("[5] Exit");

        string? command = Console.ReadLine();
        switch (command)
        {
            case "1":
                DisplayAllProducts(inventory);
                break;
            case "2":
                command = null;
                while (command == null || command == "")
                {
                    Console.Clear();
                    Console.Write("Enter a search term : ");
                    command = Console.ReadLine();
                    DisplaySearchedProducts(inventory, command);
                }
                break;
            case "3":
                break;
            case "4":
                break;
            case "5":
                Program.Running = false;
                break;
            default:
                Console.Write("Please enter a valid option");
                break;
        }
    }
    
    public static void DisplayProduct(Product product)
    {
        Console.WriteLine(product.Name);
        if (product.HasReducedPrice)
        {
            Console.Write($"${product.ReducedPrice} /");
            Console.Write($" ${product.Price} ");
            Console.Write($"{product.ReducedPercent}% off\n");
        }
        else
            Console.WriteLine($"${product.Price}");
        Console.WriteLine($"in stock: {product.Quantity}");
    }
    
    public static void DisplayAllProducts(ProductInventory inventory)
    {
        Console.Clear();
        Console.WriteLine("[0] Return to main menu");
        Console.WriteLine("--------------------------------------------------------");
        
        var products = inventory.Products;
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
        Console.Clear();
        DisplayProduct(products[index-1]);
        Console.WriteLine("[1] Add to cart");
        Console.WriteLine("[2] Return to main menu");
        command = Console.ReadLine();
        if (command == "1")
        {
            
        }
        else if (command == "2")
        {
            return;
        }
    }
    
    public static void DisplaySearchedProducts(ProductInventory inventory, string search)
    {
        Console.Clear();
        Console.WriteLine("Choose a product");
        Console.WriteLine("--------------------------------------------------------");
        List<Product> products = new();
  
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
            //Console.Write($"[{i}]");
            //DisplayProduct(inventory.Products[i]);
            //Console.WriteLine("--------------------------------------------------------");
        }

        if (products.Count == 0)
        {
            Console.Clear();
            Console.WriteLine("No products found");
            Console.ReadLine();
            return;
        }
        
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
        while (true)
        {
            Console.Clear();
            DisplayProduct(products[index-1]);
            Console.WriteLine("[1] Add to cart");
            Console.WriteLine("[2] Return to main menu");
            command = Console.ReadLine();
            if (command == "1")
            {
                //todo Add to cart
                return;
            }
            if (command == "2")
            {
                return;
            }
        }
        
    }
}