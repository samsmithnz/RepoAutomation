using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoAutomation.Core.APIAccess;
using RepoAutomation.Core.Models.Copilot;
using RepoAutomation.Tests.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepoAutomation.Tests
{
    [TestClass]
    public class GitHubApiAccessTests : BaseAPIAccessTests
    {
        [TestMethod]
        public async Task GetCopilotModelCatalogTest()
        {
            // Arrange

            // Act
            List<ModelInfo> results = await GitHubApiAccess.GetCopilotModelCatalogList(base.GitHubId, base.GitHubSecret);

            // Assert
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Count > 0);
            ModelInfo model = results[0];
            Assert.AreEqual("openai/gpt-4.1", model.Id);
            Assert.AreEqual("OpenAI GPT-4.1", model.Name);
            Assert.IsNotNull(model.Publisher);
            Assert.IsNotNull(model.RateLimitTier);
            Assert.IsNotNull(model.Summary);
            Assert.IsNotNull(model.SupportedInputModalities);
            Assert.IsNotNull(model.SupportedOutputModalities);
            Assert.IsNotNull(model.Tags);

            //{ "id":,"name":"OpenAI GPT-4.1","publisher":"OpenAI","summary":"gpt-4.1 outperforms gpt-4o across the board, with major gains in coding, instruction following, and long-context understanding","rate_limit_tier":"high","supported_input_modalities":["text", "image"],"supported_output_modalities":["text"],"tags":["multipurpose", "multilingual", "multimodal"]}
        }

        [TestMethod]
        public async Task GetCopilotChatResponseTest()
        {
            //Arrange
            InferenceRequest body = new();
            body.model = "openai/gpt-4.1";
            body.messages = new[]
            {
                new InferenceMessage {
                    Role = "user",
                    Content = "What is the capital of France?"
                }
            };

            //Act
            InferenceResponse result = await GitHubApiAccess.CopilotChatCompletionRequest(base.GitHubId, base.GitHubSecret, body);

            //Assert
            Assert.IsNotNull(result);

        }
    }
}
