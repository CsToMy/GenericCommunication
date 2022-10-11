using MQTTClient.Interfaces;
using MQTTnet;
using MQTTnet.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MQTTClient
{
    public sealed class MQTTClient : IMQTTCommunication
    {
        private IMqttClient? _mqttClient;
        private readonly string _defaultTopic = "default";

        public MQTTClient (MqttClientOptions clientOptions)
        {
            var factory = new MqttFactory();
            _mqttClient = factory.CreateMqttClient();
            var connectionTask = _mqttClient.ConnectAsync(clientOptions, CancellationToken.None);
            connectionTask.Wait();
        }

        public MQTTClient(IMqttClient? mqttClient)
        {
            _mqttClient = mqttClient;
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
                Disconnect(CancellationToken.None).Wait();
                _mqttClient.Dispose();
                _mqttClient = null;
            }
        }

        public async Task SendMessage<T>(T message, IEnumerable<string> ackClientIds, CancellationToken cancellationToken)
        where T : class
        {
            if(_mqttClient != null)
            {
                if (_mqttClient.IsConnected)
                {
                    if(message != null)
                    {
                        var messageBuilder = new MqttApplicationMessageBuilder();
                        MqttApplicationMessage payload;

                        if (message is MQTTMessageDTO)
                        {
                            var mqttMessageDTO = message as MQTTMessageDTO;
                            if(mqttMessageDTO is null)
                            {
                                throw new InvalidCastException("Cannot cast message into MQTTMessageDTO.");
                            }

                            payload = messageBuilder
                                .WithPayload(mqttMessageDTO.Payload?.ToString())
                                .WithTopic(mqttMessageDTO.Topic)
                                .Build();
                        }
                        else
                        {
                            payload = messageBuilder.WithPayload(message?.ToString())
                                .WithTopic(_defaultTopic)
                                .Build();
                        }
                        
                        
                        await _mqttClient.PublishAsync(payload, cancellationToken);
                    }
                }
            }
        }

        public async Task Subscribe(string topic, CancellationToken cancelationToken)
        {
            if (_mqttClient != null)
            {
                if (_mqttClient.IsConnected)
                {
                    var subscriptionOptions = new MqttClientSubscribeOptionsBuilder()
                        .WithTopicFilter(new MqttTopicFilterBuilder().WithTopic(topic).Build())
                        .Build();

                    await _mqttClient.SubscribeAsync(subscriptionOptions, cancelationToken);
                }
            }
        }

        public async Task Unsubscribe(string topic, CancellationToken cancelationToken)
        {
            if (_mqttClient != null)
            {
                if (_mqttClient.IsConnected)
                {
                    var unsubscriptionOptions = new MqttClientUnsubscribeOptionsBuilder()
                        .WithTopicFilter(topic)
                        .Build();

                    await _mqttClient.UnsubscribeAsync(unsubscriptionOptions, cancelationToken);
                }
            }
        }
    }
}
