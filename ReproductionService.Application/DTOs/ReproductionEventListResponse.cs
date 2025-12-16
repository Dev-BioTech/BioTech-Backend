namespace ReproductionService.Application.DTOs;

public record ReproductionEventListResponse(List<ReproductionEventResponse> Items, int Count);
