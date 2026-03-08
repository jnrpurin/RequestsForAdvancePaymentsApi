namespace Anticipation.Application.DTOs;

public sealed record AnticipationSimulationResponse(
    string creator_id,
    decimal valor_solicitado,
    decimal taxa_percentual,
    decimal valor_taxa,
    decimal valor_liquido,
    DateTime data_solicitacao,
    string status);