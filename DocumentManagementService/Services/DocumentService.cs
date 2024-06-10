using DocumentManagementService.Models;

namespace DocumentManagementService.Services;
public class DocumentService : IDocumentService
{
    private readonly string _storagePath;

    public DocumentService(IConfiguration configuration)
    {
        _storagePath = configuration.GetValue<string>("StoragePath");
        if (!Directory.Exists(_storagePath))
        {
            Directory.CreateDirectory(_storagePath);
        }
    }

    public IEnumerable<Document> GetDocuments()
    {
        var files = Directory.GetFiles(_storagePath);
        return files.Select(file => new FileInfo(file)).Select(fileInfo => new Document
        {
            Id = Path.GetFileNameWithoutExtension(fileInfo.Name),
            Name = fileInfo.Name,
            ContentType = GetContentType(fileInfo.Extension),
            Size = fileInfo.Length,
            UploadedAt = fileInfo.CreationTime
        });
    }

    public Document GetDocument(string id)
    {
        var filePath = Path.Combine(_storagePath, id);
        if (!File.Exists(filePath))
        {
            return null;
        }

        var fileInfo = new FileInfo(filePath);
        return new Document
        {
            Id = Path.GetFileNameWithoutExtension(fileInfo.Name),
            Name = fileInfo.Name,
            ContentType = GetContentType(fileInfo.Extension),
            Size = fileInfo.Length,
            UploadedAt = fileInfo.CreationTime
        };
    }

    public void SaveDocument(IFormFile file)
    {
        var filePath = Path.Combine(_storagePath, file.FileName);
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            file.CopyTo(stream);
        }
    }

    public void DeleteDocument(string id)
    {
        var filePath = Path.Combine(_storagePath, id);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    private string GetContentType(string extension)
    {
        return extension.ToLower() switch
        {
            ".txt" => "text/plain",
            ".pdf" => "application/pdf",
            ".jpg" => "image/jpeg",
            ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            _ => "application/octet-stream",
        };
    }
}
