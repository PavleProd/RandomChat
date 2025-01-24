using ChatClient.Common;
using System.Windows.Controls;
using System.Windows;

namespace RandomChat
{
    public class MessageStyleSelector : StyleSelector
    {
        public Style? OutgoingMessageStyle { get; set; }
        public Style? IncomingMessageStyle { get; set; }
        public Style? ServerMessageStyle { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item is Message message)
            {
                return message.Type switch
                {
                    MessageType.Outgoing => OutgoingMessageStyle,
                    MessageType.Incoming => IncomingMessageStyle,
                    MessageType.Server => ServerMessageStyle,
                    _ => base.SelectStyle(item, container)
                };
            }

            return base.SelectStyle(item, container);
        }
    }
}

