
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace RepoAutomation.Core.Models.Copilot
{
    public class ModelInfo
    {
        /// <summary>
        /// The unique identifier for the model
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The name of the model
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The publisher of the model
        /// </summary>
        public string Publisher { get; set; }

        /// <summary>
        /// A brief summary of the model's capabilities
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// The rate limit tier for the model
        /// </summary>   
        [JsonProperty("rate_limit_tier")]
        public string RateLimitTier { get; set; }

        /// <summary>
        /// A list of tags associated with the model
        /// </summary>
        public List<string> Tags { get; set; }

        /// <summary>
        /// A list of input modalities supported by the model
        /// </summary>
        [JsonProperty("supported_input_modalities")]
        public List<string> SupportedInputModalities { get; set; }

        /// <summary>
        /// A list of output modalities supported by the model
        /// </summary>
        [JsonProperty("supported_output_modalities")]
        public List<string> SupportedOutputModalities { get; set; }
    }
}
