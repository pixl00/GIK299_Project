using System;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingApp
{

    internal class ProductManager
    {
        // main method to manage products, displays menu and routes to appropriate functions
        public static void ManageProducts(ProductInventory inventory)
        {
            // Loop continuously until user exits
            while (true)
            {
                Console.Clear();
                Console.WriteLine("---[PRODUCT MANAGEMENT]---");
                Console.WriteLine("[1] Create product");
                Console.WriteLine("[2] Change product");
                Console.WriteLine("[3] Discount product");
                Console.WriteLine("\n[0] Exit product management");
                Console.WriteLine();
                Console.Write("Select an option: ");

                string? command = Console.ReadLine();

                // Route to appropriate method based on user selection
                switch (command)
                {
                    case "1":
                        CreateProduct(inventory);
                        break;
                    case "2":
                        ChangeProduct(inventory);
                        break;
                    case "3":
                        DiscountProduct(inventory);
                        break;
                    case "0":
                        // User exits, method returns to main menu
                        return;
                    default:
                        Console.WriteLine("Invalid option.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        // method to create a new product in the inventory
        private static void CreateProduct(ProductInventory inventory)
        {
            Console.Clear();
            Console.WriteLine("---[CREATE PRODUCT]---\n");

            // Set name for new item
            Console.Write("Enter product name: ");
            string? name = Console.ReadLine();
            
            // Validate name is not empty
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Invalid name.");
                Console.ReadKey();
                return;
            }

            // Check if a product with this name already exists
            // For new products, we don't pass excludeId (default is null) to make sure every product is checked

            if (inventory.ProductNameExists(name))
            {
                Console.WriteLine("A product with this name already exists.");
                Console.ReadKey();
                return;
            }

            // Set price for new item
            Console.Write("Enter price (with or without decimals, e.g., 19.99 or 19): ");

            // TryParse safely converts string to float, returns false if conversion fails
            // Also validate that price is not negative
            if (!float.TryParse(Console.ReadLine(), out float price) || price < 0)
            {
                Console.WriteLine("Invalid price. Please enter a valid number.");
                Console.ReadKey();
                return;
            }

            // Set quantity for new item
            Console.Write("Enter quantity in stock: ");

            // Use uint (unsigned int) to ensure quantity can't be negative
            if (!uint.TryParse(Console.ReadLine(), out uint quantity))
            {
                Console.WriteLine("Invalid quantity. Please enter a whole number.");
                Console.ReadKey();
                return;
            }

            // Set category for new item
            Console.WriteLine("Available categories: GAMING | AUDIO | STORAGE | ACCESSORIES");
            Console.Write("Enter category (case insensitive): ");
            string? categoryInput = Console.ReadLine();
            
            // Validate category input with multiple checks:
            // 1. categoryInput is not empty
            // 2. Can be parsed to ProductCategory enum (ignoreCase: true handles any case variation)
            // 3. Is not "None" (which is not a valid category)
            if (string.IsNullOrWhiteSpace(categoryInput) || 
                !Enum.TryParse<ProductCategory>(categoryInput, ignoreCase: true, out ProductCategory category) || 
                category == ProductCategory.None)
            {
                Console.WriteLine("Invalid category. Please choose from: GAMING, AUDIO, STORAGE, ACCESSORIES");
                Console.ReadKey();
                return;
            }

            // Create the product after all validations pass using the create method from ProductInventory
            inventory.CreateProduct(name, price, quantity, category);
            Console.WriteLine("\nProduct created successfully!");
            Console.ReadKey();
        }


        // method to change details of an existing product
        private static void ChangeProduct(ProductInventory inventory)
        {
            Console.Clear();
            Console.WriteLine("---[CHANGE PRODUCT]---\n");

            // Display all products with their details for user to choose from
            DisplayProducts(inventory);
            Console.Write("Enter product number to change (0 to cancel): ");
            
            // Get product selection and validate it's within range
            if (!int.TryParse(Console.ReadLine(), out int index) || index < 1 || index > inventory.Products.Count)
            {
                return;
            }

            // Get the actual product object using the selected index
            // Subtract 1 because display shows [1], [2], etc. but list is 0-indexed
            Product product = inventory.Products[index - 1];
            
            Console.Clear();
            Console.WriteLine($"Editing: {product.Name}\n");

            // Update name for the product
            Console.Write("Enter new name (leave blank to keep current): ");
            string? newName = Console.ReadLine();
            
            // Only process name change if user actually entered something
            if (!string.IsNullOrWhiteSpace(newName))
            {
                // Check for name uniqueness, passing product.Id to EXCLUDE this product from the check
                // This prevents "false positive" duplicates when user doesn't change the name
                if (inventory.ProductNameExists(newName, product.Id))
                {
                    Console.WriteLine("A product with this name already exists.");
                    Console.ReadKey();
                    return;
                }
                product.Name = newName;
            }

            // Update product price
            Console.Write("Enter new price (leave blank to keep current): ");
            string? priceInput = Console.ReadLine();
            
            // Only process price change if user entered something
            if (!string.IsNullOrWhiteSpace(priceInput))
            {
                // Validate it's a valid float and not negative
                if (float.TryParse(priceInput, out float newPrice) && newPrice >= 0)
                {
                    product.Price = newPrice;
                }
                else
                {
                    Console.WriteLine("Invalid price format.");
                    Console.ReadKey();
                    return;
                }
            }

            // Update product quantity
            Console.Write("Enter new quantity (leave blank to keep current): ");
            string? quantityInput = Console.ReadLine();

            // Only process quantity change if user entered something, just like above
            if (!string.IsNullOrWhiteSpace(quantityInput))
            {
                // Validate it's a valid unsigned integer
                if (uint.TryParse(quantityInput, out uint newQuantity))
                {
                    // Use inventory method to update quantity (passes the product ID and new quantity)
                    inventory.SetProductQuantity((uint)product.Id, newQuantity);
                }
                else
                {
                    Console.WriteLine("Invalid quantity format.");
                    Console.ReadKey();
                    return;
                }
            }

            // update product category
            Console.WriteLine("Available categories: GAMING | AUDIO | STORAGE | ACCESSORIES");
            Console.Write("Enter new category (leave blank to keep current): ");
            string? newCategoryInput = Console.ReadLine();
            
            // Only process category change if user entered something
            if (!string.IsNullOrWhiteSpace(newCategoryInput))
            {
                // Validate it's a valid category enum and not "None"
                // ignoreCase: true allows "audio", "AUDIO", "Audio" to all match the Audio enum value
                if (Enum.TryParse<ProductCategory>(newCategoryInput, ignoreCase: true, out   ProductCategory newCategory) && 
                    newCategory != ProductCategory.None)
                {
                    // Replace categories list with single new category to prevent multiple categories being assigned
                    product.Categories = new List<ProductCategory> { newCategory };
                }
                else
                {
                    Console.WriteLine("Invalid category. Please choose from: GAMING, AUDIO, STORAGE, ACCESSORIES");
                    Console.ReadKey();
                    return;
                }
            }

            Console.WriteLine("\nProduct updated successfully!");
            Console.ReadKey();
        }

        private static void DiscountProduct(ProductInventory inventory)
        {
            Console.Clear();
            Console.WriteLine("---[DISCOUNT PRODUCT]---\n");

            // Display all products for user to select from
            DisplayProducts(inventory);
            Console.Write("Enter product number to discount (0 to cancel): ");
            
            // Get product selection and validate it's within range
            if (!int.TryParse(Console.ReadLine(), out int index) || index < 1 || index > inventory.Products.Count)
            {
                return;
            }

            // Get the selected product
            Product product = inventory.Products[index - 1];
            
            Console.Clear();
            Console.WriteLine($"Product: {product.Name}");
            Console.WriteLine($"Current price: ${product.Price}\n");

            // set discount percentage
            Console.Write("Enter discount percentage (0-100): ");
            
            // Validate discount is a valid integer between 0 and 100
            if (!int.TryParse(Console.ReadLine(), out int discountPercent) || 
                discountPercent < 0 || discountPercent > 100)
            {
                Console.WriteLine("Invalid discount percentage.");
                Console.ReadKey();
                return;
            }

            // Apply the discount using Product's method
            // This ensures discount is correctly calculated as: Price * (1 - percentage/100)
            product.SetReducedPrice(discountPercent);
            
            Console.WriteLine($"\nDiscount applied! New price: ${product.ReducedPrice:F2}");
            Console.ReadKey();
        }

        private static void DisplayProducts(ProductInventory inventory)
        {
            // Handle case where there are no products
            if (inventory.Products.Count == 0)
            {
                Console.WriteLine("No products available.\n");
                return;
            }

            // Loop through and display each product with its details
            for (int i = 0; i < inventory.Products.Count; i++)
            {
                // Products are guaranteed to have exactly 1 category by design
                string category = inventory.Products[i].Categories[0].ToString();
                
                // Display product info: [number] name - $price (stock) [category]
                // Add 1 to index so display shows [1], [2], etc. instead of [0], [1], etc.
                Console.WriteLine($"[{i + 1}] {inventory.Products[i].Name} - ${inventory.Products[i].Price} (Stock: {inventory.Products[i].Quantity}) [{category}]");
            }
            Console.WriteLine();
        }
    }
}
