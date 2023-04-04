using ERP.Shared.Ticketing;
using ERP.Shared.Users;
using ERP.Ticketing.HttpApi.Features.Ticketing;
using ERP.Ticketing.HttpApi.Features.Users;
using Mapster;
using MapsterMapper;

namespace ERP.Ticketing.HttpApi.Configuration;

public static class MappingConfigurations
{
    public static IServiceCollection AddMapping(this IServiceCollection services)
    {
        var typeAdapterConfig = GetTypeAdapterConfig();
        MapsterMapper.IMapper mapper = new Mapper(typeAdapterConfig);

        // TypeAdapterConfig.GlobalSettings.NewConfig<Ticket, TicketDto>()
        //     .Map(dst => dst.Comments, src => src.Comments);
        //
        // TypeAdapterConfig.GlobalSettings.NewConfig<TicketComment, TicketCommentDto>();
        
        return services;
    }

    private static TypeAdapterConfig GetTypeAdapterConfig()
    {
        var config = new TypeAdapterConfig();

        config.NewConfig<TicketComment, TicketCommentDto>();
        config.NewConfig<Ticket, TicketDto>()
            .Map(dest => dest.Creator, src => src.Creator.Adapt<CreatorDto>());

        return config;
    }
}