// ============================================================================
// Module 1 · Walkthrough — the C# plumbing every AI app in this course uses
// ----------------------------------------------------------------------------
// Read top-to-bottom. Each numbered section maps to a concept in
// notes/M01-notes.md. Run it with:   dotnet run --project src/M01-Setup/M01.Walkthrough
// ============================================================================

using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

// ---------------------------------------------------------------------------
// 1) GENERIC HOST
//    One object that wires up Configuration + Dependency Injection + Logging.
//    ASP.NET Core, worker services and (later) our agents all sit on this.
//    ContentRootPath = output folder, so appsettings.json is found no matter
//    which directory you run `dotnet run` from.
// ---------------------------------------------------------------------------
var builder = Host.CreateApplicationBuilder(new HostApplicationBuilderSettings
{
    Args = args,
    ContentRootPath = AppContext.BaseDirectory,
});

// ---------------------------------------------------------------------------
// 2) CONFIGURATION — layered sources, last one wins:
//    appsettings.json  →  appsettings.{Environment}.json  →  user-secrets
//    →  environment variables  →  command-line args
//
//    User-secrets are only auto-loaded when Environment == "Development".
//    Console apps default to "Production", so we add them explicitly.
//    Secrets live in %APPDATA%\Microsoft\UserSecrets\<id>\secrets.json —
//    OUTSIDE the repo, so they can never be committed by accident.
// ---------------------------------------------------------------------------
builder.Configuration.AddUserSecrets<Program>(optional: true);

// ---------------------------------------------------------------------------
// 3) OPTIONS PATTERN — bind a config section to a strongly typed class,
//    and fail fast at startup if it's invalid (instead of at the 1st API call).
// ---------------------------------------------------------------------------
builder.Services
    .AddOptions<DemoOptions>()
    .Bind(builder.Configuration.GetSection(DemoOptions.Section))
    .Validate(o => o.Count is > 0 and <= 20, "Demo:Count must be between 1 and 20")
    .ValidateOnStart();

// ---------------------------------------------------------------------------
// 4) DEPENDENCY INJECTION — register services by interface.
//    Later you'll register IChatClient exactly like this, which is what lets
//    you swap GitHub Models ⇄ Azure OpenAI ⇄ Ollama without touching code.
// ---------------------------------------------------------------------------
builder.Services.AddSingleton<ITokenStream, FakeTokenStream>();

using var host = builder.Build();

// Resolve services from the container (in ASP.NET Core this is done for you).
var logger = host.Services.GetRequiredService<ILogger<Program>>();
var options = host.Services.GetRequiredService<IOptions<DemoOptions>>().Value;
var stream = host.Services.GetRequiredService<ITokenStream>();

logger.LogInformation("Environment: {Env}", builder.Environment.EnvironmentName);
logger.LogInformation("Secret configured: {HasSecret}",
    !string.IsNullOrEmpty(builder.Configuration["Demo:Secret"])); // never log the value itself!

// ---------------------------------------------------------------------------
// 5) CANCELLATION — Ctrl+C should stop a long LLM stream cleanly.
// ---------------------------------------------------------------------------
using var cts = new CancellationTokenSource();
Console.CancelKeyPress += (_, e) => { e.Cancel = true; cts.Cancel(); };

// ---------------------------------------------------------------------------
// 6) STREAMING with IAsyncEnumerable + await foreach
//    This is EXACTLY the shape of an LLM streaming response: items arrive one
//    at a time, and you print each as soon as it lands.
// ---------------------------------------------------------------------------
Console.WriteLine();
try
{
    await foreach (var token in stream.StreamAsync(options.Prompt, options.Count, cts.Token))
    {
        Console.Write(token.Text);
    }
    Console.WriteLine();
}
catch (OperationCanceledException)
{
    Console.WriteLine("\n[cancelled]");
}

// ---------------------------------------------------------------------------
// 7) PATTERN MATCHING on a record result
// ---------------------------------------------------------------------------
var summary = new StreamSummary(options.Count, cts.IsCancellationRequested);
Console.WriteLine(summary switch
{
    { Cancelled: true } => "Stream was stopped by the user.",
    { TokenCount: > 10 } => $"Long stream: {summary.TokenCount} tokens.",
    _ => $"Short stream: {summary.TokenCount} tokens.",
});

// ============================================================================
// Types
// ============================================================================

/// <summary>Strongly typed view of the "Demo" config section.</summary>
public sealed class DemoOptions
{
    public const string Section = "Demo";
    public string Prompt { get; set; } = "Hello";
    public int Count { get; set; } = 5;
}

/// <summary>RECORDS: immutable data with value equality — perfect for messages.</summary>
public sealed record Token(int Index, string Text);
public sealed record StreamSummary(int TokenCount, bool Cancelled);

public interface ITokenStream
{
    IAsyncEnumerable<Token> StreamAsync(string prompt, int count, CancellationToken ct = default);
}

/// <summary>Pretends to be an LLM: yields words with a small delay.</summary>
public sealed class FakeTokenStream(ILogger<FakeTokenStream> logger) : ITokenStream // primary constructor
{
    private static readonly string[] Words =
        ["Large ", "language ", "models ", "produce ", "text ", "one ", "token ", "at ", "a ", "time. "];

    // [EnumeratorCancellation] lets callers pass a token via .WithCancellation() too.
    public async IAsyncEnumerable<Token> StreamAsync(
        string prompt, int count, [EnumeratorCancellation] CancellationToken ct = default)
    {
        logger.LogDebug("Streaming {Count} tokens for prompt '{Prompt}'", count, prompt);
        for (var i = 0; i < count; i++)
        {
            await Task.Delay(150, ct);          // simulates network latency per token
            yield return new Token(i, Words[i % Words.Length]);
        }
    }
}
