using CatalogService.Entitites;
using MediatR;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CatalogService.UseCases.ProductUseCases.Commands.CreateProduct;

public class CreateProductCommand : IRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int SubcategoryId { get; set; }
    public int Quantity { get; set; }
}
