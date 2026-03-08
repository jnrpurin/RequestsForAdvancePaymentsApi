using Anticipation.Domain.Entities;
using Anticipation.Domain.Enums;

namespace Anticipation.Application.DTOs;

public sealed record AnticipationResponse(
    Guid id,
    string creator_id,
    decimal valor_solicitado,
    decimal valor_liquido,
    DateTime data_solicitacao,
    string status)
{
    public static AnticipationResponse FromDomain(AnticipationRequest request)
    {
        return new AnticipationResponse(
            request.Id,
            request.CreatorId,
            request.RequestedAmount.Amount,
            request.NetAmount,
            request.RequestDate,
            MapStatus(request.Status));
    }

    private static string MapStatus(RequestStatus status)
    {
        return status switch
        {
            RequestStatus.Pending => "pendentef",
            RequestStatus.Approved => "aprovada",
            RequestStatus.Rejected => "recusada",
            _ => "desconhecido"
        };
    }
}