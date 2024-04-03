namespace Catalog.API.Endpoints.GetProductByCategory;

//public record GetProductByCategoryRequest();
public record GetProductByCategoryResponse(IEnumerable<Product> Products);