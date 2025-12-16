
namespace AuthService.Application.DTOs;

public record FarmListResponse(
    IEnumerable<FarmResponse> Farms,
    int TotalCount
);
