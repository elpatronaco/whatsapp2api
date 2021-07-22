using System.Threading.Tasks;

namespace whatsapp2api.Contracts.Hubs
{
    public interface IChatHub
    {
        Task SendGlobalMessage(params object[] args);
    }
}