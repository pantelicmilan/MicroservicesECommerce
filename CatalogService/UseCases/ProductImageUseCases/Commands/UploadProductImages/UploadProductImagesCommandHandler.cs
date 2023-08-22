using CatalogService.Repositories.Abstractions;
using MediatR;

namespace CatalogService.UseCases.ProductImageUseCases.Commands.UploadProductImages;

public class UploadProductImagesCommandHandler : IRequestHandler<UploadProductImagesCommand>
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IProductRepository _productRepository;
    private readonly IProductImageRepository _productImageRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UploadProductImagesCommandHandler(
        IWebHostEnvironment webHostEnvironment,
        IProductRepository productRepository,
        IProductImageRepository productImageRepository,
        IUnitOfWork unitOfWork)
    {
        _webHostEnvironment = webHostEnvironment;
        _productRepository = productRepository;
        _productImageRepository = productImageRepository;
        _unitOfWork = unitOfWork;
    }


    public async Task Handle(UploadProductImagesCommand request, CancellationToken cancellationToken)
    {
        if (request.Images.Count > 5) throw new Exception("Image limit reached");
        var product = await _productRepository.GetProductById(request.ProductId);
        if (product == null) throw new Exception("Product with this id does not exist!");
        int maxSizeInMB = 5;
        long maxSizeInBytes = maxSizeInMB * 1024 * 1024;
        List<string> allowedExtensions = new List<string> { ".jpg", ".jpeg", ".png" };

        foreach (var image in request.Images)
        {
            if (image == null) throw new Exception("Image can not be null!");
            var currentFileExtension = System.IO.Path.GetExtension(image.FileName).ToLower();
            if (allowedExtensions.Contains(currentFileExtension) == false) throw new Exception("You upload invalid data format (.jpg, .jpeg, .png allowed only)");
            if (image.Length > maxSizeInBytes) throw new Exception("Size limit reached!");
        }

        foreach(var image in request.Images)
        {
            var newImageLink = await UploadImageAsync(image, request.ProductId);
            _productImageRepository.UploadProductImage(newImageLink, request.ProductId);
        }
        await _unitOfWork.SaveChangesAsync();
    }


    private async Task<string> UploadImageAsync(IFormFile imageFile, int productId)
    {
        if (imageFile == null || imageFile.Length <= 0)
        {
            throw new Exception("Error with upload request!");
        }
        // Kreiranje putanje do foldera za čuvanje slika
        string uploadsFolder = Path.Combine(_webHostEnvironment.ContentRootPath, "ImageStorage", productId.ToString());

        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder); // Ako folder ne postoji, kreiraj ga
        }

        // Generisanje jedinstvenog imena datoteke
        string uniqueFileName = $"{Guid.NewGuid()}_{imageFile.FileName}";

        // Kreiranje putanje do krajnjeg fajla
        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await imageFile.CopyToAsync(fileStream);
        }

        return $"https://localhost:7242/productImages/{productId.ToString()}/{uniqueFileName}"; // Vraćanje jedinstvenog imena datoteke
    }

}
