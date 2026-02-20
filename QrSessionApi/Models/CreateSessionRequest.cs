namespace PartyMaker.API.Models;

public class CreateSessionRequest
{
    public string RoomId { get; set; } = string.Empty; // Уникальный идентификатор комнаты
    public string? RoomName { get; set; }              // Название комнаты
    public string? HostName { get; set; }              // Имя создателя комнаты
}