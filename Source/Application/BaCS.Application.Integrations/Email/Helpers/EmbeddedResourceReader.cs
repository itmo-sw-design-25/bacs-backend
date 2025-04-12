namespace BaCS.Application.Integrations.Email.Helpers;

using System.Collections.Concurrent;
using System.Reflection;

public static class EmbeddedResourceReader
{
    private static readonly Assembly Assembly = Assembly.GetExecutingAssembly();
    private static readonly ConcurrentDictionary<string, string> Cache = new();

    public static string ReadAsString(string resourceName) => Cache.GetOrAdd(resourceName, ReadResourceText);

    private static string ReadResourceText(string resourceName)
    {
        using var stream = Assembly.GetManifestResourceStream(resourceName);

        if (stream == null)
        {
            throw new FileNotFoundException(
                $"Embedded resource '{resourceName}' not found in assembly {Assembly.FullName}."
            );
        }

        using var reader = new StreamReader(stream);

        return reader.ReadToEnd();
    }
}
