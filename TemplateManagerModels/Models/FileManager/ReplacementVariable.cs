namespace TemplateManagerModels.Models.FileManager;

internal class ReplacementVariable
{
    internal string Key;

    internal string? Value { get; private set; }

    public ReplacementVariable(string key)
    {
        Key = key;
    }

    internal void SetValue(string value)
    {
        Value = value;
    }
}
