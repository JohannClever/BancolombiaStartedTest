using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BancolombiaStarter.Backend.Domain.Dto
{
    public class ProjectsDto
    {
        public long Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(100)]
        public string Description { get; set; }
        [Required]

        public decimal Goal { get; set; }
        public string PictureUrl { get; set; }
        public decimal Pledged { get; set; }
        public int BackersCount { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserPicture { get; set; }
        public DateTime? FinancedDate { get; set; }
        public DateTime CreationOn { get; set; }
    }


    public class DeleteProjectDto
    {
        public long Id { get; set; }
    }
    public class ProjectsDeleteResponseDto : ResponseBaseDto
    {
    }

    public class ProjectsUpdateResponseDto : ResponseBaseDto
    {
    }

    public class ProjectsUpdateDto
    {
        [Required]
        public long Id { get; set; }
        public string Name { get; set; }
        public decimal? Goal { get; set; }
        public string Description { get; set; }
    }

    public class ProjectsCreateDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public decimal Goal { get; set; }
        public IFormFile? Picture { get; set; }
    }

    public class ProjectsCreateResponseDto
    {
        public long Id { get; set; }
    }

}
