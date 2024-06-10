using DocumentManagementService.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace DocumentManagementService.Services;
public interface IDocumentService
{
    IEnumerable<Document> GetDocuments();
    Document GetDocument(string id);
    void SaveDocument(IFormFile file);
    void DeleteDocument(string id);
}