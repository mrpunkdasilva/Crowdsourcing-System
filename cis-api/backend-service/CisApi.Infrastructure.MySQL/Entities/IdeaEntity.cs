using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CisApi.Infrastructure.MySQL.Entities
{
    [Table("ideas")]
    public class IdeaEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("topic_id")]
        public int TopicId { get; set; }

        [ForeignKey("TopicId")]
        public required TopicEntity Topic { get; set; }

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
        public required UserEntity CreatedBy { get; set; }

        [Column("vote_count")]
        public int VoteCount { get; set; }

        public ICollection<IdeaVotesEntity> Votes { get; set; } = new List<IdeaVotesEntity>();
    }
}