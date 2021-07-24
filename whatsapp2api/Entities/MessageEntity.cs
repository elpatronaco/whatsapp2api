using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using whatsapp2api.Models.Message;

namespace whatsapp2api.Entities
{
    [Table("messages")]
    public class MessageEntity
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

        public MessageEntity()
        {
        }

        public MessageEntity(Guid senderId, Guid recipientId, string content, DateTime? sentDate)
        {
            SenderId = senderId;
            RecipientId = recipientId;
            Content = content;
            SentDate = sentDate ?? DateTime.Now;
        }

        public MessageModel ToDto()
        {
            return new()
            {
                Id = Id,
                Sender = Sender.ToDto(),
                Recipient = Recipient.ToDto(),
                Content = Content,
                SentDate = SentDate
            };
        }
    }
}