namespace Basket.API.Entities;

public class ShoppingCart
{
    public ShoppingCart(string username)
    {
        this.UserName = username;
    }

    public string UserName { get; set; }
    public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();

    public decimal TotalPrice
    {
        get
        {
            decimal totalPrice = 0;
            foreach (var item in Items)
                totalPrice += item.Price * item.Quantity;

            return totalPrice;
        }
    }
}

public class ShoppingCartItem
{
    public int Quantity { get; set; }
    public string Color { get; set; }
    public decimal Price { get; set; }
    public string ProductId  { get; set; }
    public string ProductName { get; set; }
}
