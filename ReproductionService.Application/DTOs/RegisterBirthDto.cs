namespace ReproductionService.Application.DTOs;

public record RegisterBirthDto(long MotherAnimalId, string OffspringTag, decimal Weight, string Gender, DateTime BirthDate);
