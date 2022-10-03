using Common.Interfaces;
using MQTTClient.Interfaces;
using MQTTnet.Client;
using MQTTnet.Protocol;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MQTTClient
{
    public sealed class MQTTClientBuilder: IInterserviceCommunicationBuilder
    {
        private readonly MqttClientOptionsBuilder _optionBuilder;
        private MqttQualityOfServiceLevel _qos;
        private bool _withCleanSession;

        public MQTTClientBuilder(int qos, bool withCleanSession)
        {
            _optionBuilder =  new MqttClientOptionsBuilder();
            switch (qos)
            {
                case 0:
                    _qos = MqttQualityOfServiceLevel.AtMostOnce;
                    break;
                case 1:
                    _qos = MqttQualityOfServiceLevel.AtLeastOnce;
                    break;
                case 2:
                    _qos = MqttQualityOfServiceLevel.ExactlyOnce;
                    break;
                default:
                    throw new ArgumentException("Unknown QoS level.");
            }

            _withCleanSession = withCleanSession;
        }
        public async Task<IInterServiceCommunication> Connect(Uri address, string clientId, CancellationToken cancellationToken)
        {
            return await Task.Run(() =>
            {
                IMQTTCommunication client;
                try
                {
                    var clientOptions = _optionBuilder.WithCleanSession(_withCleanSession)
                        .WithClientId(clientId)
                        .WithConnectionUri(address)
                        .WithProtocolVersion(MQTTnet.Formatter.MqttProtocolVersion.V311)
                        .WithWillQualityOfServiceLevel(_qos)
                        .Build();

                    client = new MQTTClient(clientOptions);  
                }
                catch (Exception)
                {
                    throw;
                }

                return client;
            }, cancellationToken);
        }
    }
}
