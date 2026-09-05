namespace ConfigurationVersioning.Api.Diff
{
    public interface IJsonDiffService
    {
        string Compare(string oldJson, string newJson);
    }
}
