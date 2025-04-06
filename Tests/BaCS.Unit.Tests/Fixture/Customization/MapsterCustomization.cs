namespace BaCS.Unit.Tests.Fixture.Customization;

using Application.Mapping.Configs;
using AutoFixture;
using Mapster;
using MapsterMapper;

public class MapsterCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        var config = new TypeAdapterConfig();
        config.Apply(new MapsterCommandsConfig(), new MapsterEntitiesConfig());
        fixture.Register<IMapper>(() => new Mapper(config));
    }
}
