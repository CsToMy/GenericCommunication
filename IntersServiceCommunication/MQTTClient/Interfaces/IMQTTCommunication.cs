using Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace MQTTClient.Interfaces
{
    public interface IMQTTCommunication: IInterServiceCommunication
    {
        Task Subscribe(string topic, CancellationToken cancelationToken);
        Task Unsubscribe(string topic, CancellationToken cancelationToken);
    }
}
