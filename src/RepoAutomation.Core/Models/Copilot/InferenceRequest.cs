namespace RepoAutomation.Core.Models.Copilot
{
    public class InferenceRequest
    {
        public string model {  get; set; }
        public InferenceMessage[] messages { get; set; }
    }
}
