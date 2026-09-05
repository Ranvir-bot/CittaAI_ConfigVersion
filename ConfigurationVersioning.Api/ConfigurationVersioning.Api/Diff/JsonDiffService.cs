using JsonDiffPatchDotNet;
using Newtonsoft.Json.Linq;

namespace ConfigurationVersioning.Api.Diff
{
    public class JsonDiffService : IJsonDiffService
    {
        public string Compare(string oldJson, string newJson)
        {
            // Convert JSON strings to JToken
            var oldToken = JToken.Parse(oldJson);
            var newToken = JToken.Parse(newJson);

            // Create JsonDiffPatch object
            var jsonDiffPatch = new JsonDiffPatch();

            // Generate diff
            var diff = jsonDiffPatch.Diff(oldToken, newToken);

            // Return diff as JSON string
            return diff?.ToString() ?? string.Empty;
        }
    }
}