using System.ComponentModel.DataAnnotations;

namespace BancolombiaStarter.Backend.Domain.Dto
{
    public class CommentsFilterDto
    {
        public long? ProjectId { get; set; }
        public string? IdUser { get; set; }
    }

    public class CommentsDto
    {
        public long Id { get; set; }
        public  string CompanyServiceName { get; set; }
        public int CompanyServiceId { get; set; }
        public string StatusName { get; set; }
        public int StatusId { get; set; }
        public string Observations { get; set; }
        public string IdUser { get; set; }
        public string UserName { get; set; }
        public DateTime CreationOn { get; set; }
    }

    public class CommentsDeleteDto
    {
        [Required]
        public long Id { get; set; }
    }
    public class CommentsResponseBaseDto
    {
        public bool Result { get; set; }
        public string Message { get; set; }
    }
    public class CommentsDeleteResponseDto: CommentsResponseBaseDto
    {
    }

    public class CommentsUpdateResponseDto: CommentsResponseBaseDto
    {
    }

    public class CommentsUpdateDto
    {
        [Required]
        public long Id { get; set; }
        public string? Observations { get; set; }
    }

    public class CommentsCreateDto
    {
        [Required]
        public long CompanyServiceId { get; set; }
        [Required]
        public int ProjectId { get; set; }
        [Required]
        public string Observations { get; set; }
        public string IdUser { get; set; }
    }

    public class CommentsCreateResponseDto
    {
        public long Id { get; set; }
    }
}
