using Hexagon.Database.HexagonDb.Models;
using Hexagon.Logic.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Profile = AutoMapper.Profile;

namespace Hexagon.Logic;

public class MapperProfile : Profile
{
    private IConfigurationRoot _configuration;
    
    public MapperProfile()
    {
            _configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            
            UserMapper();
    }

    public void UserMapper()
    {
        CreateMap<User, GetUserModel>();
    }
}