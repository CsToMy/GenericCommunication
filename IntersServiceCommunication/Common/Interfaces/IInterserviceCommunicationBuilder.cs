using System;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IInterserviceCommunicationBuilder
    {
        Task<IInterServiceCommunication> Connect(Uri address, string clientId, CancellationToken cancellationToken);
    }
}
