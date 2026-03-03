using AuthService.Application.DTOs;
using MediatR;

namespace AuthService.Application.Queries.GetMyFarms;

public record GetMyFarmsQuery(bool IncludeInactive = false) : IRequest<FarmListResponse>;
