namespace ReproductionService.Application.DTOs;

public record BirthDto(long MotherAnimalId, string OffspringTag, DateTime BirthDate, decimal Weight, string Gender);
