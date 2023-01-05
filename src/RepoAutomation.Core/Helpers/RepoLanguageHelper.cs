using GitHubActionsDotNet.Models;
using Newtonsoft.Json;
using RepoAutomation.Core.APIAccess;
using RepoAutomation.Core.Models;
using System.Text;
using YamlDotNet.Serialization;

namespace RepoAutomation.Core.Helpers
{
    public static class RepoLanguageHelper
    {
        public async static Task<List<RepoLanguage>> GetRepoLanguages(string? clientId, string? clientSecret,
            string owner, string repo)
        {
            Dictionary<string, int>? languages = await GitHubApiAccess.GetRepoLanguages(clientId, clientSecret, owner, repo);
            //Get language definition from from linguist repo
            string languageDefinitionOwner = "github";
            string languageDefinitionRepository = "linguist";
            string? languageDefinitionFilePath = "lib/linguist/languages.yml";
            //Don't use the PAT token here - as it's a different organization - instead let's rely on anon rates
            GitHubFile? fileResult = await GitHubFiles.GetFileContents(null, null,
                languageDefinitionOwner, languageDefinitionRepository, languageDefinitionFilePath);
            Dictionary<string, LanguageDefinition>? repoLanguageDetails = null;
            if (fileResult != null && fileResult.content != null)
            {
                IDeserializer deserializer = new DeserializerBuilder()
                    .IgnoreUnmatchedProperties() //Ignore YAML that doesn't have a matching .NET property
                    .Build();
                repoLanguageDetails = deserializer.Deserialize<Dictionary<string, LanguageDefinition>>(fileResult.content);
            }
            List<RepoLanguage> repoLanguages = RepoLanguageHelper.TransformRepoLanguages(languages, repoLanguageDetails);

            return repoLanguages;
        }


        public static List<RepoLanguage> TransformRepoLanguages(Dictionary<string, int> languages, Dictionary<string, LanguageDefinition>? repoLanguageDetails)
        {
            List<RepoLanguage> repoLanguages = new();

            //Get the total for the percent calculation
            int total = 0;
            foreach (KeyValuePair<string, int> item in languages)
            {
                total += item.Value;
            }

            //Build the language with the percent calculation
            foreach (KeyValuePair<string, int> item in languages)
            {
                repoLanguages.Add(new()
                {
                    Name = item.Key,
                    Total = item.Value,
                    Percent = Math.Round((decimal)item.Value / total, 3)
                });
                if (repoLanguageDetails != null && repoLanguageDetails.ContainsKey(item.Key))
                {
                    repoLanguages[^1].Color = repoLanguageDetails[item.Key].color;
                }
            }

            return repoLanguages;
        }
    }
}
