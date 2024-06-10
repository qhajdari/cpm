using DocumentManagementService.Models;
using DocumentManagementService.Services;
using Microsoft.AspNetCore.Mvc;

namespace DocumentManagementService.Controllers;
[ApiController]
[Route("[controller]")]
public class DocumentController : ControllerBase
{
    private readonly DocumentService _documentService;

    public DocumentController(DocumentService documentService)
    {
        _documentService = documentService;
    }

    // GET: /Document
    [HttpGet]
    public ActionResult<IEnumerable<Document>> GetDocuments()
    {
        var documents = _documentService.GetDocuments();
        return Ok(documents);
    }

    // GET: /Document/{id}
    [HttpGet("{id}")]
    public ActionResult<Document> GetDocument(string id)
    {
        var document = _documentService.GetDocument(id);
        if (document == null)
        {
            return NotFound();
        }

        return Ok(document);
    }

    // POST: /Document
    [HttpPost]
    public IActionResult UploadDocument([FromForm] IFormFile file)
    {
        _documentService.SaveDocument(file);
        return Ok();
    }

    // DELETE: /Document/{id}
    [HttpDelete("{id}")]
    public IActionResult DeleteDocument(string id)
    {
        _documentService.DeleteDocument(id);
        return NoContent();
    }
}
