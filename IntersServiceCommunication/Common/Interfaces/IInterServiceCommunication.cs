using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IInterServiceCommunication : IDisposable
    {
        Task SendMessage<T>(T message, IEnumerable<string> ackClientIds, CancellationToken cancellationToken) where T:class;
        Task Disconnect(CancellationToken cancellationToken);
        //Task ReciveMessage<T>(T eventArgs, CancellationToken cancellationToken) where T: class;
    }
}
