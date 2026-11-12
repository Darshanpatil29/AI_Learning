# Module 1 — Setup & the C# plumbing behind every AI app

Code for this module: `src/M01-Setup/M01.Walkthrough/Program.cs` (the numbers below match the numbered sections in that file).

## Step 1 · Get your GitHub Models token (5 min)

1. Go to **github.com → Settings → Developer settings → Personal access tokens → Fine-grained tokens → Generate new token**.
2. Name: `ai-learning`, Expiration: 90 days, Repository access: **Public repositories (read-only)**.
3. **Permissions → Account permissions → Models → Read-only**. Nothing else is needed.
4. Generate, copy the token, and store it with **user-secrets** (from the repo root):

   ```bash
   dotnet user-secrets set "GitHubModels:Token" "PASTE_TOKEN_HERE" --project src/M01-Setup/M01.SmokeTest
   ```

5. Run the smoke test:

   ```bash
   dotnet run --project src/M01-Setup/M01.SmokeTest
   ```

   You should see a short streamed answer ending with `✅ Connected. Tokens in/out: …`.

Free tier notes: rate-limited (requests/minute and requests/day per model). If you get HTTP 429, wait and retry. Browse models at https://github.com/marketplace/models. Model ids look like `openai/gpt-4.1-mini`, `openai/gpt-4.1`, `meta/Llama-3.3-70B-Instruct`, `microsoft/Phi-4`. Change `Model` in `appsettings.json` to try others.

> 🔐 **Rule for the whole course:** API keys go in user-secrets (local) or Key Vault (cloud). Never in `appsettings.json`, never in code, never in chat (including with me).

## Step 2 · Concepts in the walkthrough

| # | Concept | Why AI apps need it |
|---|---|---|
| 1 | **Generic Host** (`Host.CreateApplicationBuilder`) | One place for config + DI + logging. ASP.NET Core, workers and MAF agents all build on it. |
| 2 | **Configuration layering** | Endpoints and model names come from config, so you can switch models without recompiling. Precedence: json → env-specific json → user-secrets → env vars → command line (**last wins**). |
| 3 | **Options pattern + validation** | Model settings like temperature (0–2) are checked when the app starts, not halfway through a conversation. |
| 4 | **Dependency injection** | You'll register `IChatClient` and depend on the *interface*, which is how you swap providers. |
| 5 | **CancellationToken** | Stops a slow or expensive LLM call when the user presses Ctrl+C or closes the browser. |
| 6 | **`IAsyncEnumerable<T>` + `await foreach`** | This is how LLM **streaming** works: tokens arrive one at a time. |
| 7 | **Records + pattern matching** | Messages, tool results and structured outputs are modelled as immutable records. |

Gotchas found while building this:
- Console apps run as `Production`, so user-secrets **aren't loaded automatically** → we call `AddUserSecrets<Program>()`.
- `appsettings.json` must be copied to the output folder (`CopyToOutputDirectory` in the `.csproj`), and we set `ContentRootPath = AppContext.BaseDirectory`. Without both, the file isn't found when you run from another folder.
- The secret's **value** is never logged, only whether it exists.

Try it:

```bash
dotnet run --project src/M01-Setup/M01.Walkthrough
```

```bash
dotnet run --project src/M01-Setup/M01.Walkthrough -- --Demo:Count 25
```

The second command should fail with an `OptionsValidationException` ("Demo:Count must be between 1 and 20").

> 🧠 Subtle point: `ValidateOnStart()` runs when the host **starts** (`host.StartAsync()` / `host.RunAsync()`). The walkthrough never starts the host, so validation actually happens lazily the first time `.Value` is read (line 61). In ASP.NET Core and worker apps the host is started, so you get a true fail-fast.
