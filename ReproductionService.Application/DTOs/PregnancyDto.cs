namespace ReproductionService.Application.DTOs;

public record PregnancyDto(long AnimalId, string AnimalTag, DateOnly ExpectedBirthDate, int GestationWeek);
