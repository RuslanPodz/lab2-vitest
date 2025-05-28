using YogaApp.Application.ServiceContact.DTO.Preview;
using YogaApp.Core.Domain.Program;

namespace YogaApp.Application.Mappers.Preview;

public static class ProgramPreviewMapper
{
    public static ProgramPreviewDto ToProgramPreviewDto(this ProgramModel program)
    {
        return new ProgramPreviewDto()
        {
            Name = program.ProgramName,
            ImageUrl = program.ProgramImage
        };
    }
}