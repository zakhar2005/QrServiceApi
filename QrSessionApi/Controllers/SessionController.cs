using Microsoft.AspNetCore.Mvc;
using PartyMaker.API.Models;
using QRCoder;
using System.Text;

namespace PartyMaker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SessionController : ControllerBase
{
    private static List<SessionQrResponse> _generatedInvites = new List<SessionQrResponse>();
    private static int _nextId = 1;

    
    [HttpGet]
    public ActionResult<IEnumerable<SessionQrResponse>> Get()
    {
        return Ok(_generatedInvites);
    }

    
    [HttpPost("create-invite")]
    public ActionResult<SessionQrResponse> CreateSessionInvite([FromBody] CreateSessionRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.RoomId))
        {
            return BadRequest(new { error = "Идентификатор комнаты (RoomId) обязателен для заполнения." });
        }

        string joinLink = $"https://partymaker.app/join?room={request.RoomId}";

        if (!string.IsNullOrWhiteSpace(request.RoomName))
        {
            joinLink += $"&name={Uri.EscapeDataString(request.RoomName)}";
        }

        string qrCodeBase64 = GenerateQrCodeImage(joinLink);

        var response = new SessionQrResponse
        {
            Id = _nextId++,
            RoomId = request.RoomId,
            RoomName = request.RoomName ?? "Безымянная комната",
            JoinLink = joinLink,
            QrCodeBase64 = qrCodeBase64,
            CreatedAt = DateTime.Now,
            ExpiresInMinutes = 60 // Приглашение действительно 60 минут
        };

        _generatedInvites.Add(response);

        return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
    }
    [HttpGet("{id}")]
    public ActionResult<SessionQrResponse> GetById(int id)
    {
        var invite = _generatedInvites.FirstOrDefault(i => i.Id == id);
        if (invite == null)
        {
            return NotFound(new { error = "Приглашение не найдено" });
        }
        return Ok(invite);
    }

    private string GenerateQrCodeImage(string data)
    {
        using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
        using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q))
        using (PngByteQRCode qrCode = new PngByteQRCode(qrCodeData))
        {
            byte[] qrCodeAsPngBytes = qrCode.GetGraphic(20);
            return Convert.ToBase64String(qrCodeAsPngBytes);
        }
    }
}