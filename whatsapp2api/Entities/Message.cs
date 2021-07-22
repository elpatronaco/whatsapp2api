using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace whatsapp2api.Entities
{
    [Table("messages")]
    [Index]
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [ForeignKey("Sender")] public Guid SenderId { get; set; }

        public UserEntity Sender { get; set; }

        [ForeignKey("Recipient")] public Guid RecipientId { get; set; }

        public UserEntity Recipient { get; set; }

        public string Content { get; set; }

        public DateTime SentDate { get; set; }
    }
}