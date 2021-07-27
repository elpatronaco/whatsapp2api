using System;
using System.ComponentModel.DataAnnotations;

namespace whatsapp2api.Models.Message
{
    public class MessageCreate
    {
        [Required] public Guid RecipientId { get; set; }
        [Required, MinLength(1)] public string Content { get; set; }
    }
}