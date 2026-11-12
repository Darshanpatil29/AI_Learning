// ============================================================================
// Module 1 · Smoke test — proves your GitHub Models token works.
// Don't worry about understanding the AI parts yet; Module 2 covers them.
//
// One-time setup (run from the repo root, paste YOUR token — never commit it):
//   dotnet user-secrets set "GitHubModels:Token" "<your PAT>" --project src/M01-Setup/M01.SmokeTest
// Then:
//   dotnet run --project src/M01-Setup/M01.SmokeTest
// ============================================================================

using System.ClientModel;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using OpenAI;

var builder = Host.CreateApplicationBuilder(new HostApplicationBuilderSettings
{
    Args = args,
    ContentRootPath = AppContext.BaseDirectory,
});
builder.Configuration.AddUserSecrets<Program>(optional: true);

var config = builder.Configuration.GetSection("GitHubModels");
var token = config["Token"];
var endpoint = config["Endpoint"] ?? "https://models.github.ai/inference";
var model = config["Model"] ?? "openai/gpt-4.1-mini";

if (string.IsNullOrWhiteSpace(token))
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("No token found. Run:");
    Console.WriteLine("  dotnet user-secrets set \"GitHubModels:Token\" \"<your PAT>\" --project src/M01-Setup/M01.SmokeTest");
    Console.ResetColor();
    return 1;
}

// GitHub Models speaks the OpenAI API, so we use the OpenAI SDK pointed at
// GitHub's endpoint, then wrap it as the provider-neutral IChatClient.
IChatClient chat = new OpenAIClient(
        new ApiKeyCredential(token),
        new OpenAIClientOptions { Endpoint = new Uri(endpoint) })
    .GetChatClient(model)
    .AsIChatClient();

Console.WriteLine($"Model: {model}\n");

try
{
    UsageDetails? usage = null;
    await foreach (var update in chat.GetStreamingResponseAsync(
        "In two sentences, what is an AI agent? Greet a .NET developer named Darshan first."))
    {
        Console.Write(update.Text);
        usage = update.Contents.OfType<UsageContent>().FirstOrDefault()?.Details ?? usage;
    }

    Console.WriteLine($"\n\n✅ Connected. Tokens in/out: {usage?.InputTokenCount}/{usage?.OutputTokenCount}");
    return 0;
}
catch (ClientResultException ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"❌ HTTP {ex.Status}: {ex.Message}");
    Console.WriteLine(ex.Status switch
    {
        401 => "Token is invalid or expired.",
        403 => "Token lacks the 'Models: read' permission.",
        429 => "Free-tier rate limit hit — wait a minute and retry.",
        _ => "Check endpoint/model name in appsettings.json.",
    });
    Console.ResetColor();
    return 1;
}
