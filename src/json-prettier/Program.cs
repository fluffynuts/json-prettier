using System.Text;
using System.Text.Json;

var didSomething = false;
foreach (var file in args)
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
    CheckDidSomething();
    return;
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
        var prettier = JsonSerializer.Serialize(obj, new JsonSerializerOptions() { WriteIndented = true } );
#pragma warning restore IL2026
        Console.WriteLine(prettier);
        didSomething = true;
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine(ex.Message);
    }
}

void CheckDidSomething()
{
    if (!didSomething)
    {
        Console.Error.WriteLine("Please specify at least one file on the commandline or pipe json into my stdin");
    }
}