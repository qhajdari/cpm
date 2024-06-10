using DocumentManagementService.Models;
using DocumentManagementService.Services;
using Microsoft.AspNetCore.Mvc;

namespace DocumentManagementService.Controllers;
[ApiController]
[Route("[controller]")]
public class DocumentController : ControllerBase
{
    private readonly DocumentService _documentService;
    private readonly ILogger<DocumentController> _logger;

    public DocumentController(DocumentService documentService, ILogger<DocumentController> logger)
    {
        _documentService = documentService;
        _logger = logger;
    }

    // GET: /Document
    [HttpGet]
    public ActionResult<IEnumerable<Document>> GetDocuments()
    {
        _logger.LogInformation("Getting all documents");
        var documents = _documentService.GetDocuments();
        return Ok(documents);
    }

    // GET: /Document/{id}
    [HttpGet("{id}")]
    public ActionResult<Document> GetDocument(string id)
    {
        _logger.LogInformation("Getting document with ID {Id}", id);
        var document = _documentService.GetDocument(id);
        if (document == null)
        {
            _logger.LogWarning("Document with ID {Id} not found", id);
            return NotFound();
        }

        return Ok(document);
    }

    // POST: /Document
    [HttpPost]
    public IActionResult UploadDocument([FromForm] IFormFile file)
    {
        _logger.LogInformation("Uploading document {FileName}", file.FileName);
        _documentService.SaveDocument(file);
        return Ok();
    }

    // DELETE: /Document/{id}
    [HttpDelete("{id}")]
    public IActionResult DeleteDocument(string id)
    {
        _logger.LogInformation("Deleting document with ID {Id}", id);
        _documentService.DeleteDocument(id);
        return NoContent();
    }
}
