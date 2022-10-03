using MQTTClient.Interfaces;
using MQTTnet;
using MQTTnet.Client;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MQTTClient
{
    public sealed class MQTTClient : IMQTTCommunication
    {
        private IMqttClient? _mqttClient;

        public MQTTClient (MqttClientOptions clientOptions)
        {
            var factory = new MqttFactory();
            _mqttClient = factory.CreateMqttClient();
            var connectionTask = _mqttClient.ConnectAsync(clientOptions, CancellationToken.None);
            connectionTask.Wait();
        }

        public async Task Disconnect(CancellationToken cancellationToken)
        {
            if(_mqttClient != null)
            {
                if (_mqttClient.IsConnected)
                {
                    var disconnectOptions = new MqttClientDisconnectOptions();
                    disconnectOptions.Reason = MqttClientDisconnectReason.NormalDisconnection;
                    disconnectOptions.ReasonString = String.Empty;

                    await _mqttClient.DisconnectAsync(disconnectOptions, cancellationToken);
                }
            }
        }

        public void Dispose()
        {
            if(_mqttClient != null)
            {
                if (_mqttClient.IsConnected)
                {
                    Disconnect(CancellationToken.None).Wait();
                }

                _mqttClient.Dispose();
                _mqttClient = null;
            }
        }

        public Task ReciveMessage<T>(T eventArgs, CancellationToken cancellationToken) 
            where T : class
        {
            throw new NotImplementedException();
        }

        public async Task SendMessage<T>(T message, IEnumerable<string> ackClientIds, CancellationToken cancellationToken) 
            where T: class
        {
            if(_mqttClient != null)
            {
                if (_mqttClient.IsConnected)
                {
                    var messageBuilder = new MqttApplicationMessageBuilder();
                    MQTTMessageDTO? messageDTO = message as MQTTMessageDTO;
                    if(messageDTO != null)
                    {
                        MqttApplicationMessage payload = messageBuilder.WithPayload(messageDTO.Payload.ToString())
                        .WithTopic(messageDTO.Topic)
                        .Build();
                        await _mqttClient.PublishAsync(null, cancellationToken);
                    }
                }
            }
        }

        public Task Subscribe(string topic, CancellationToken cancelationToken)
        {
            throw new NotImplementedException();
        }

        public Task Unsubscribe(string topic, CancellationToken cancelationToken)
        {
            throw new NotImplementedException();
        }
    }
}
