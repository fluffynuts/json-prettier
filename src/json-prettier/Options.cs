using System.Text.Json;

public class Options
{
    public bool Escaped { get; set; }
    public List<string> Files { get; set; } = new();
    public int? ExitCode { get; set; }

    public JsonSerializerOptions SerializerOptions { get; set; } = new()
    {
        WriteIndented = true
    };

    public static Options From(string[] args)
    {
        var options = new Options();
        foreach (var arg in args)
        {
            if (File.Exists(arg))
            {
                options.Files.Add(arg);
                continue;
            }

            switch (arg)
            {
                case "-1":
                case "--one-line":
                    options.SerializerOptions.WriteIndented = false;
                    break;
                case "-e":
                case "--escape":
                case "--escaped":
                    options.Escaped = true;
                    break;
                case "-h":
                case "--help":
                    options.ExitCode = 0;
                    PrintHelp();
                    break;
                default:
                    Console.Error.WriteLine($"Unknown argument: '{arg}");
                    options.ExitCode = 2;
                    PrintHelp();
                    break;
            }
        }

        return options;
    }

    static void PrintHelp()
    {
        Console.WriteLine(@"
Usage: json-prettier [options] {..files}
    or pipe text into json-prettier with [options]
possible options:
-1
--one-line          output in one line for embedding

-e
--escape
--escaped           escape quotes and ampersands for embedding

-h
--help              print this incredibly useful help
".Trim());
    }
}