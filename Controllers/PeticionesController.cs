using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeticionesAPI.Data;
using PeticionesAPI.Models;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System.IO;

[Route("api/[controller]")]
[ApiController]
public class PeticionesController : ControllerBase
{
    private readonly PeticionesDbContext _context;

    public PeticionesController(PeticionesDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> PostPeticion([FromBody] Peticion peticion)
    {
        if (string.IsNullOrEmpty(peticion.NombreOpcional))
        {
            peticion.NombreOpcional = "Anónimo";
        }

        peticion.FechaCreacion = DateTime.Now; 
        _context.Peticiones.Add(peticion);
        await _context.SaveChangesAsync();

        return Ok(peticion);
    }

    // este método acepta parámetros opcionales para filtrar por fecha
    [HttpGet("todas")]
    public async Task<ActionResult<IEnumerable<Peticion>>> GetTodasLasPeticiones([FromQuery] DateTime? fechaInicio, [FromQuery] DateTime? fechaFin)
    {
        var peticiones = _context.Peticiones.AsQueryable();

        if (fechaInicio.HasValue)
        {
            peticiones = peticiones.Where(p => p.FechaCreacion.Date >= fechaInicio.Value.Date);
        }

        if (fechaFin.HasValue)
        {
            peticiones = peticiones.Where(p => p.FechaCreacion.Date <= fechaFin.Value.Date);
        }

        return await peticiones.ToListAsync();
    }

    // método de PDF que acepta y usa los filtros de fecha
    [HttpGet("pdf")]
    public async Task<IActionResult> GetPeticionesPdf([FromQuery] DateTime? fechaInicio, [FromQuery] DateTime? fechaFin)
    {
        var peticionesQuery = _context.Peticiones.AsQueryable();

        if (fechaInicio.HasValue)
        {
            peticionesQuery = peticionesQuery.Where(p => p.FechaCreacion.Date >= fechaInicio.Value.Date);
        }

        if (fechaFin.HasValue)
        {
            peticionesQuery = peticionesQuery.Where(p => p.FechaCreacion.Date <= fechaFin.Value.Date);
        }
        
        var peticiones = await peticionesQuery.ToListAsync();

        var document = new PdfDocument();
        var page = document.AddPage();
        var gfx = XGraphics.FromPdfPage(page);
        
        var fontTitle = new XFont("Arial", 24, XFontStyle.Bold);
        var fontSubtitle = new XFont("Arial", 16, XFontStyle.Regular);
        var fontBody = new XFont("Arial", 12, XFontStyle.Regular);

        gfx.DrawString("Lista de Peticiones de Oración", fontTitle, XBrushes.Black, new XRect(0, 30, page.Width, page.Height), XStringFormats.TopCenter);
        gfx.DrawString($"Total de Peticiones: {peticiones.Count}", fontSubtitle, XBrushes.Black, new XRect(0, 60, page.Width, page.Height), XStringFormats.TopCenter);
        
        double yPoint = 100;

        foreach (var peticion in peticiones)
        {
            if (yPoint > page.Height - 50)
            {
                page = document.AddPage();
                gfx = XGraphics.FromPdfPage(page);
                yPoint = 50;
            }

            gfx.DrawString($"--------------------------------------------------------------------------------", fontBody, XBrushes.Gray, new XRect(30, yPoint, page.Width - 60, page.Height), XStringFormats.TopLeft);
            yPoint += 15;
            gfx.DrawString($"Petición de: {peticion.NombreOpcional}", fontBody, XBrushes.Black, new XRect(30, yPoint, page.Width - 60, page.Height), XStringFormats.TopLeft);
            yPoint += 15;
            gfx.DrawString($"Fecha: {peticion.FechaCreacion.ToShortDateString()}", fontBody, XBrushes.Black, new XRect(30, yPoint, page.Width - 60, page.Height), XStringFormats.TopLeft); 
            yPoint += 15;
            gfx.DrawString($"Petición: {peticion.PeticionTexto}", fontBody, XBrushes.Black, new XRect(30, yPoint, page.Width - 60, page.Height), XStringFormats.TopLeft);
            yPoint += 30;
        }

        var stream = new MemoryStream();
        document.Save(stream);
        stream.Position = 0;

        return new FileStreamResult(stream, "application/pdf")
        {
            FileDownloadName = "PeticionesDeOracion.pdf"
        };
    }
}