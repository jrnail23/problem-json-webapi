using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace ProblemJsonWebapi
{
    [DataContract(Name = "problem", Namespace = "urn:problem-json-webapi")]
    [XmlRoot("problem", Namespace = "urn:problem-json-webapi")]
    public class ProblemJsonModel
    {
        [XmlElement("type")]
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DataMember(Name = "type", EmitDefaultValue = false)]
        public string TypeUri { get; set; }

        [XmlElement("title")]
        [JsonProperty("title", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DataMember(Name = "title", EmitDefaultValue = false)]
        public string Title { get; set; }

        [XmlElement("detail")]
        [JsonProperty("detail", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DataMember(Name = "detail", EmitDefaultValue = false)]
        public string Detail { get; set; }

        [XmlElement("status")]
        [JsonProperty("status", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DataMember(Name = "status", EmitDefaultValue = false)]
        public int Status { get; set; }

        [XmlElement("instance")]
        [JsonProperty("instance", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DataMember(Name = "instance", EmitDefaultValue = false)]
        public string Instance { get; set; }

        [XmlElement("debug")]
        [JsonProperty("debug", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DataMember(Name = "debug", EmitDefaultValue = false)]
        public string DebugInfo { get; set; }

    }
}