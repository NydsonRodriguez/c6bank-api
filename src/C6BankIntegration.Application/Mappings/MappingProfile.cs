using AutoMapper;
using C6BankIntegration.Application.DTOs.Response;
using C6BankIntegration.Domain.Entities;

namespace C6BankIntegration.Application.Mappings;

/// <summary>Perfil de mapeamento AutoMapper para conversão entre entidades e DTOs.</summary>
public sealed class MappingProfile : Profile
{
    /// <summary>Configura todos os mapeamentos do projeto.</summary>
    public MappingProfile()
    {
        CreateMap<Boleto, BoletoResponse>()
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount.Value))
            .ForMember(dest => dest.PayerDocument, opt => opt.MapFrom(src => src.PayerDocument.Value));

        CreateMap<PixCharge, PixChargeResponse>()
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount.Value))
            .ForMember(dest => dest.DebtorDocument, opt => opt.MapFrom(src => src.DebtorDocument != null ? src.DebtorDocument.Value : null));

        CreateMap<Webhook, WebhookResponse>();
    }
}
