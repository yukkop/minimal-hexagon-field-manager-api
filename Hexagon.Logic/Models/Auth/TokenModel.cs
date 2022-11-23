namespace Hexagon.Logic.Models.Auth;

public class TokenModel
{
        public bool Successed { get; set; } = false;
        public string Jwt { get; set; }
        public GetUserModel User { get; set; }
}
