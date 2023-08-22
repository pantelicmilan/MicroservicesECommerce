using Microsoft.AspNetCore.Hosting;

namespace CatalogService;

public static class DeleteImageFileHandler
{
    public static void DeleteImageFileByImageLink(string imageLink, IWebHostEnvironment webHostEnvironment)
    {
        var linkAfterProductImages = imageLink.IndexOf("/productImages/");
        int coreInfoStartIndex = linkAfterProductImages + "/productImages/".Length; // Pronalazi početak brojeva
        string coreInfo = imageLink.Substring(coreInfoStartIndex); // Rezultat će biti "3/filename.jpg"
        var filePath = Path.Combine(webHostEnvironment.ContentRootPath, "ImageStorage", coreInfo);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
}
