using MQTTClient;
using MQTTClient.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading;

Console.WriteLine("Hello, World!");
var clientBuilder = new MQTTClientBuilder(1, false);

Uri brokerAddress = new Uri("http://192.168.1.32:1883");
IMQTTCommunication client = (IMQTTCommunication)await clientBuilder.Connect(brokerAddress, "Service1", CancellationToken.None);
MQTTMessageDTO message = new("TEST", JObject.Parse("{'measurement': 34}"));
await client.SendMessage<MQTTMessageDTO>(message, null, CancellationToken.None);
