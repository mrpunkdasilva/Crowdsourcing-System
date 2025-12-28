using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CisApi.Infrastructure.MySQL.Entities
{
    [Table("users")]
    public class UserEntity
    {
        [Key]
        [Column("id")]
        public required string Id { get; set; }
        
        [Required]
        [Column("login")]
        public required string Login { get; set; }

        public ICollection<TopicEntity> TopicsCreated { get; set; } = new List<TopicEntity>();
        public ICollection<IdeaEntity> IdeasCreated { get; set; } = new List<IdeaEntity>();
        public ICollection<IdeaVotesEntity> Votes { get; set; } = new List<IdeaVotesEntity>();
    }
}
