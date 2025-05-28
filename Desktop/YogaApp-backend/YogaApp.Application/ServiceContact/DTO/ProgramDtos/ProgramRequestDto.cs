using YogaApp.Application.ServiceContact.DTO.WorkoutDtos;

namespace YogaApp.Application.ServiceContact.DTO.ProgramDtos;

public class ProgramRequestDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty; 
}