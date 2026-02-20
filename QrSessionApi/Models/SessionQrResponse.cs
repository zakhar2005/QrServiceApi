namespace PartyMaker.API.Models;

public class SessionQrResponse
{
    public int Id { get; set; }                         // Условный ID созданного приглашения
    public string RoomId { get; set; } = string.Empty;  // Идентификатор комнаты
    public string RoomName { get; set; } = string.Empty;// Название комнаты
    public string JoinLink { get; set; } = string.Empty;// Полная ссылка для подключения
    public string QrCodeBase64 { get; set; } = string.Empty; // QR-код в формате Base64 строки
    public DateTime CreatedAt { get; set; }              // Дата и время создания
    public int ExpiresInMinutes { get; set; } = 60;      // Время жизни приглашения
}