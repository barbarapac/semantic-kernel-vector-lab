namespace Semantic.Kernel.Vector.WebApi.Dtos;

public record CreateProductDto(
    string Title, 
    string Category, 
    string Summary, 
    string Description);