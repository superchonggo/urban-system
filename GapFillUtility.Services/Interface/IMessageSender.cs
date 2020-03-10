using System.Collections.Generic;
using System.Threading.Tasks;

namespace GapFillUtility.Services.Interface
{
    public interface IMessageSender
    {
        Task SendToTopicAsync<TMessage>(TMessage body, params KeyValuePair<string, string>[] properties);
    }
}
