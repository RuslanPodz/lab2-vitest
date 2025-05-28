using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YogaApp.Core.Domain.Program;

namespace YogaApp.Core.Domain.User;

public class UserProgramModel
{
    [Key]
    public Guid UserProgramId { get; set; }
    
    [ForeignKey("User")]
    public Guid? UserId { get; set; }
    
    [ForeignKey("Program")]
    public Guid ProgramId { get; set; }

    public DateTime JoinedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    
    [StringLength(50)]
    public string? UserName { get; set; }
    
     public ProgramModel? Program { get; set; }
    public UserModel? User { get; set; }
}