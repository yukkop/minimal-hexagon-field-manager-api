namespace Hexagon.Logic.Logic;

public interface IBaseLogic
{
    int? UserId { get; set; }
}

public class BaseLogic : IBaseLogic
{
    public int? UserId { get; set; }
}