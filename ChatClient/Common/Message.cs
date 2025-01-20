using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Common
{
    readonly struct Message
    {
        public Message(string authorId, string text)
        {
            AuthorId = authorId;
            Text = text;
            SendingTime = DateTime.Now;
        }

        public override string ToString() 
        {
            return $"[{SendingTime}] | {AuthorId}: {Text}";
        }

        public string AuthorId { get; init; }
        public string Text { get; init; }
        public DateTime SendingTime { get; init; }
    }
}
