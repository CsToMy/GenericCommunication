//using Newtonsoft.Json.Linq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Runtime.CompilerServices;
//using System.Threading;
//using System.Threading.Channels;
//using System.Threading.Tasks;

//namespace Common;

//public static class InterserviceCommunicationBuilder
//{
//    public static Task<IInterserviceCommunication> Connect(
//        string brokerAddress,
//        string brokerPort,
//        string clientId,
//        CancellationToken cancellationToken)
//    {
//        throw new NotImplementedException();
//    }
//}
///* or */
//public interface IInterserviceCommunicationBuilder
//{
//    Task<IInterserviceCommunication> Connect(
//        string brokerAddress,
//        string brokerPort,
//        string clientId,
//        CancellationToken cancellationToken);
//}

//public interface IInterserviceCommunication : IDisposable, /* maybe, if there needs to be a call to Connection.CloseAsync() */ IAsyncDisposable
//{
//    Task SendMessage<T>(
//        string topic,
//        T message,
//        IEnumerable<string> ackClientIds,
//        CancellationToken cancellationToken)
//        where T : class;

//    IAsyncEnumerable<T> Subscribe1<T>(string topic, CancellationToken cancellationToken)
//        where T : class;

//    Task<IAsyncEnumerable<T>> Subscribe1_5<T>(string topic, CancellationToken cancellationToken)
//        where T : class;

//    Task<ISubscription<T>> Subscribe2<T>(string topic, CancellationToken cancellationToken)
//        where T : class;
//}

//public interface ISubscription<T> : IAsyncDisposable
//    where T : class
//{
//    Task<T> Next(CancellationToken cancellationToken);
//}

//public static class InterserviceCommunicationExtensions
//{
//    public static Task SendMessage<T>(
//        this IInterserviceCommunication communication,
//        string topic,
//        T message,
//        CancellationToken cancellationToken)
//        where T : class
//    {
//        return communication.SendMessage(topic, message, Enumerable.Empty<string>(), cancellationToken);
//    }
//}

//public static class CodeSnippets
//{
//    async static Task SendMessage<T>(
//        string topic,
//        T message,
//        IEnumerable<string> ackClientIds,
//        CancellationToken cancellationToken)
//        where T : class
//    {
//        var payload = JObject.FromObject(message);
//        var mqttMessage = new
//        {
//            Payload = payload,
//            AckData = new object() /* AckData.Create(...) */,
//        };
//    }

//    public static void ChannelStuff<T>()
//        where T : class
//    {
//        var channel = Channel.CreateUnbounded<T>(new UnboundedChannelOptions()
//        {
//            AllowSynchronousContinuations = false,
//            SingleReader = false,
//            SingleWriter = true,
//        });

//        IAsyncEnumerable<T> reader = channel.Reader.ReadAllAsync();

//        channel.Writer.TryWrite(null!);
//        channel.Writer.TryWrite(null!);
//        channel.Writer.WriteAsync(null!);
//    }

//    public static async IAsyncEnumerable<T> Subscribe1<T>(string topic, [EnumeratorCancellation] CancellationToken cancellationToken)
//        where T : class
//    {
//        var channel = Channel.CreateUnbounded<T>(new UnboundedChannelOptions()
//        {
//            AllowSynchronousContinuations = false,
//            SingleReader = false,
//            SingleWriter = true,
//        });

//        var handler = (object sender, EventArgs<T> e) =>
//        {
//            if (e.topic == topic)
//            {
//                if (!channel.Writer.TryWrite(e.Data))
//                {
//                    throw new InvalidOperationException("Operation on unbounded channel failed.");
//                }
//            }
//        };

//        try
//        {
//            connection.OnMessage += handler;
//            await connection.Subscribe(topic, cancellationToken);

//            await foreach (var item in channel.Reader.ReadAllAsync(cancellationToken))
//            {
//                // item is the string from the message, parse it here and yield return it.
//                yield return item;
//            }
//        }
//        finally
//        {
//            await connection.Unsuscribe(topic, CancellationToken.None);
//            connection.OnMessage -= handler;
//        }
//    }
//}

//public sealed class EventArgs<T> : EventArgs
//{
//    public T Data { get; }

//    public EventArgs(T data)
//    {
//        Data = data;
//    }
//}

//public sealed class AsyncEnumerableToEvent<T>
//{
//    public event EventHandler<EventArgs<T>>? Items;

//    public async Task Run(IAsyncEnumerable<T> items, CancellationToken cancellationToken)
//    {
//        await foreach (var item in items.WithCancellation(cancellationToken))
//        {
//            Items?.Invoke(this, new(item));
//        }
//    }
//}
