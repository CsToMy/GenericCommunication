using MQTTClient;
using MQTTClient.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

Console.WriteLine("Demo test application.");
var clientBuilder = new MQTTClientBuilder(1, false);


UriBuilder uriBuilder = new();
uriBuilder.Port = 1883;
uriBuilder.Host = "192.168.0.32";
Uri brokerAddress = uriBuilder.Uri;

using IMQTTCommunication client = (IMQTTCommunication)await clientBuilder.Connect(brokerAddress, "Service1", CancellationToken.None);
Console.WriteLine($"Service1 connected.");

await client.Subscribe("TEST", CancellationToken.None);

MQTTMessageDTO message = new("TEST", JObject.Parse("{'measurement': 34}"));
await client.SendMessage<MQTTMessageDTO>(message, new List<string>(), CancellationToken.None);
await Task.Delay(2300);
await client.Disconnect(CancellationToken.None);