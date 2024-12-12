using System.ComponentModel.DataAnnotations;

namespace Ozon.Application.DTOs;

public class DeleteProductDto
{
    [Required]
    public Guid Id { get; set; }
}