using RepoAutomation.Core.Models;

namespace RepoAutomation.Core.Helpers
{
    public static class RepoLanguageHelper
    {
        public static List<RepoLanguage> TransformRepoLanguages(Dictionary<string, int> languages)
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
                    Percent = Math.Round((decimal)item.Value / total,4)
                });
            }

            return repoLanguages;
        }
    }
}
