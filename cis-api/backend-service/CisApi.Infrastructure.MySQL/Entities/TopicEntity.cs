using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CisApi.Infrastructure.MySQL.Entities
{
    [Table("topics")]
    public class TopicEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("title")]
        public required string Title { get; set; }

        [Required]
        [Column("description")]
        public required string Description { get; set; }

        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("created_by_id")]
        public required string CreatedById { get; set; }

        [ForeignKey("CreatedById")]
        public UserEntity CreatedBy { get; set; } = null!;

        public List<IdeaEntity> Ideas { get; set; } = new List<IdeaEntity>();
    }
}
