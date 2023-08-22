using CatalogService.Entitites;
using CatalogService.EventBus;
using CatalogService.Repositories.Abstractions;
using MediatR;
using MessagingHelper.Events;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CatalogService.UseCases.ProductUseCases.Commands.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventBus _eventBus;

    public CreateProductCommandHandler(
        IProductRepository productRepository, 
        IUnitOfWork unitOfWork, 
        IEventBus eventBus)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _eventBus = eventBus;
    }

    public async Task Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await InsertCreatedProductInDatabase(
            request.Name, 
            request.Description, 
            request.Price, 
            request.SubcategoryId, 
            request.Quantity);

        await _eventBus.PublishAsync<ProductCreatedEvent>(
            new ProductCreatedEvent { 
                ProductName = product.Name, 
                ProductId = product.Id,
                Qtty = product.Quantity, 
                Price = product.Price
            });
    }

    private async Task<Product> InsertCreatedProductInDatabase(
        string name, 
        string description, 
        decimal price, 
        int subcategoryId, 
        int quantity) 
    {
        Product product = new Product
        {
            Name = name,
            Description = description,
            Price = price,
            SubcategoryId = subcategoryId,
            Quantity = quantity
        };
        _productRepository.CreateProduct(product);
        await _unitOfWork.SaveChangesAsync();
        return product;
    }

}

