namespace Anticipation.API.Contracts;

public sealed record CreateAnticipationRequest(
    string creator_id,
    decimal valor_solicitado,
    DateTime data_solicitacao);