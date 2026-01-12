namespace ShoppingApp;

enum ProductCategory
{
    NONE,
    OFFICE,
    GAMING
}

public class Product
{
    uint id;
    string name;
    float price;
    List<ProductCategory> categories;
    
    bool hasReducedPrice = false;
    float reducedPercent;
    float reducedPrice;
}