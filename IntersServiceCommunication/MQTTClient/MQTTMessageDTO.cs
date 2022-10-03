using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace MQTTClient
{
    public sealed class MQTTMessageDTO
    {
        [JsonProperty(
            PropertyName="Topic", 
            DefaultValueHandling = DefaultValueHandling.Ignore, 
            NullValueHandling = NullValueHandling.Include, 
            Required = Required.Always)]
        public string Topic { get; } = string.Empty;

        [JsonProperty(
            PropertyName = "Payload",
            DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
            NullValueHandling = NullValueHandling.Include,
            Required = Required.AllowNull)]
        public JObject? Payload { get; }

        public MQTTMessageDTO(string topic, JObject? payload)
        {
            Topic = topic;
            Payload = (payload != null)? (JObject)payload?.DeepClone(): null;
        }

        public override string ToString()
        {
            return System.Text.Json.JsonSerializer.Serialize(this) ??
                throw new ArgumentException("This instance cannot be serialized.");
        }

        public static MQTTMessageDTO Parse(string mqttMessageDTO)
        {
            return System.Text.Json.JsonSerializer.Deserialize<MQTTMessageDTO>(mqttMessageDTO) ??
                throw new ArgumentException("This instance cannot be deserialized.");
        }
    }
}