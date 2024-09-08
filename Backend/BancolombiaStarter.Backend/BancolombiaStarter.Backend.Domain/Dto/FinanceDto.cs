using System.ComponentModel.DataAnnotations;

namespace BancolombiaStarter.Backend.Domain.Dto
{
    public class FinanceDto
    {
        public string UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Description { get; set; }
        public long Value { get; set; }
        public int ProjectId { get; set; }
    }

    public class DeleteFinanceDto
    {
        public long Id { get; set; }
    }

    public class FinanceDeleteResponseDto : ResponseBaseDto
    {
    }

    public class FinanceUpdateResponseDto : ResponseBaseDto
    {
    }

    public class FinanceUpdateDto
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public string Description { get; set; }
    }

    public class FinanceCreateDto
    {
        [Required]
        public string Description { get; set; }
        [Required]
        public int ProjectId { get; set; }
        public long Value { get; set; }
    }

    public class FinanceCreateResponseDto
    {
        public long Id { get; set; }
    }

}
