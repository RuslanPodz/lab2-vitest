using YogaApp.Application.ServiceContact.DTO.Preview;

namespace MobileYogaApp.Presentation.ViewModels;

public class ProgramPageModel
{
    public string Title { get; set; } = "Programs";
    public List<ProgramPreviewDto> Programs { get; set; } 
}

