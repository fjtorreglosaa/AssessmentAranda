using System.ComponentModel.DataAnnotations;

namespace Assessment.Logic.Dtos.PermissionDtos
{
    public class EditAdminDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
