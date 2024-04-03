namespace Shopping.API.Aggregator.Models;

public class CatalogModel
{

    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public List<string> Category { get; set; } = new();
    //public string Summary { get; set; }
    public string Description { get; set; } = default!;
    public string ImageFile { get; set; } = default!;
    public decimal Price { get; set; }
}
