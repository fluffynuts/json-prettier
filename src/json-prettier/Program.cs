using System.Text;
using System.Text.Json;

var didSomething = false;
var options = Options.From(args);
if (options.ExitCode is not null)
{
    return options.ExitCode.Value;
}

var mutations = new List<Func<string, string>>();
if (options.Escaped)
{
    mutations.Add(s => s.Replace("&", "&amp;").Replace("\"", "&quot;"));
}

foreach (var file in options.Files)
{
    if (!File.Exists(file))
    {
        Console.Error.WriteLine($"File not found: {file}");
        continue;
    }

    DumpPrettyJson(File.ReadAllText(file, Encoding.UTF8));
}

if (!Console.IsInputRedirected)
{
    return CheckDidSomething();
}

using var reader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding);
var raw = reader.ReadToEnd();
DumpPrettyJson(raw);

void DumpPrettyJson(string json)
{
    try
    {
#pragma warning disable IL2026
        var obj = JsonSerializer.Deserialize<object>(json);
        var prettier = JsonSerializer.Serialize(obj, options.SerializerOptions);
#pragma warning restore IL2026
        foreach (var mutation in mutations)
        {
            prettier = mutation(prettier);
        }

        Console.WriteLine(prettier);
        didSomething = true;
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine(ex.Message);
    }
}

return 0;

int CheckDidSomething()
{
    if (!didSomething)
    {
        Console.Error.WriteLine("Please specify at least one file on the commandline or pipe json into my stdin");
        return 1;
    }
    return 0;
}
