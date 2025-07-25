using Newtonsoft.Json;

namespace RepoAutomation.Core.Models.Copilot
{
    // Root model that can represent either response
    public class InferenceResponse
    {
        [JsonProperty("choices")]
        public List<InferenceChoice>? Choices { get; set; }

        [JsonProperty("data")]
        public InferenceData? Data { get; set; }
    }

    // For non-streaming: choices/message/content/role
    public class InferenceChoice
    {
        [JsonProperty("message")]
        public InferenceMessage? Message { get; set; }
    }

    public class InferenceMessage
    {
        [JsonProperty("content")]
        public string? Content { get; set; }

        [JsonProperty("role")]
        public string? Role { get; set; }
    }

    // For streaming: data/choices/delta/content
    public class InferenceData
    {
        [JsonProperty("choices")]
        public List<InferenceDataChoice>? Choices { get; set; }
    }

    public class InferenceDataChoice
    {
        [JsonProperty("delta")]
        public InferenceDelta? Delta { get; set; }
    }

    public class InferenceDelta
    {
        [JsonProperty("content")]
        public string? Content { get; set; }
    }
}