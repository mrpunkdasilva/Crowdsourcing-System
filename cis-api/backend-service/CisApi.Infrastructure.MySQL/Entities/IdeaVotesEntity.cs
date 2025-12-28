using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CisApi.Infrastructure.MySQL.Entities
{
    [Table("idea_votes")]
    public class IdeaVotesEntity
    {
        [Column("idea_id")]
        public int IdeaId { get; set; }

        [ForeignKey("IdeaId")]
        public required IdeaEntity Idea { get; set; }

        [Column("user_id")]
        public required string UserId { get; set; }

        [ForeignKey("UserId")]
        public required UserEntity User { get; set; }

        [Column("voted_at")]
        public DateTime VotedAt { get; set; }
    }
}
