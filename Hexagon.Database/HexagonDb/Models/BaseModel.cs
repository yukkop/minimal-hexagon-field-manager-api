using System.Net.NetworkInformation;

namespace Hexagon.Database.HexagonDb.Models;

public class BaseModel
{
    public Guid Id { get; set; }
    public DateTime Create { get; set; }
    public DateTime LastUpdate { get; set; }
}