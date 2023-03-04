using System.Diagnostics;
using Spectre.Console;

namespace MyRpg.Logging;

public class ConsoleLogger : ILogger
{
    public void Log(string message)
    {
        AnsiConsole.WriteLine($"[{CallingMethodName}] {message}");
    }

    public void Warn(string message)
    {
        AnsiConsole.MarkupInterpolated($"[{CallingMethodName}] [yellow]Warning: {message}[/]");
        AnsiConsole.WriteLine();
    }

    public void Error(string message)
    {
        AnsiConsole.MarkupInterpolated($"[{CallingMethodName}] [red]Error: {message}[/]");
        AnsiConsole.WriteLine();
    }

    private string CallingMethodName => new StackTrace().GetFrame(2)?.GetMethod()?.Name ?? string.Empty;
}
