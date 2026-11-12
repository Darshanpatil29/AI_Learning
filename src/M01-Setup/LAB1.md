# Lab 1 — "Fake LLM" console app (you write this one!)

**Goal:** Use every Module 1 concept in your own code, with no copy-paste from the walkthrough. Peek at it only when stuck.
**Time:** 1–2 hours. **When done:** tell me `review src/M01-Setup/M01.Lab0`.

## Setup

```bash
dotnet new console -n M01.Lab0 -o src/M01-Setup/M01.Lab0
```

```bash
dotnet sln AI.slnx add src/M01-Setup/M01.Lab0
```

## Tasks

1. **Host & config.** Build the app on the Generic Host. Make sure `appsettings.json` is found even when you run from the repo root.

2. **Options with validation.** Create a `ChatSettings` class bound to a `Chat` section:
   - `Model` (string, required, not empty)
   - `Temperature` (double, must be 0.0–2.0)
   - `MaxTokens` (int, 1–4000)

   Validate on start. Prove it works by passing `--Chat:Temperature 3` on the command line and watching startup fail.

3. **Secret.** Store a fake key `Lab:ApiKey` with user-secrets. Log it **masked**: first 4 characters + `****`. If it's missing, show a helpful message and exit with code 1.

4. **Streaming service.** Create `IFakeLlm` with
   `IAsyncEnumerable<string> StreamAsync(string prompt, CancellationToken ct)`.
   The implementation:
   - picks a canned answer based on keywords in the prompt (use a `switch` expression)
   - yields it word by word with a **random delay of 50–300 ms**
   - stops after `MaxTokens` words.

5. **Chat loop.** Read user input in a loop until `exit`:
   - stream the answer to the console as words arrive
   - **Ctrl+C cancels only the current answer**, not the whole app (hint: a new `CancellationTokenSource` per turn)
   - after each answer, print `[words: N, time: X ms]` (use `Stopwatch`)

6. **History.** Store each turn as `record ChatTurn(string Role, string Text, int WordCount)` in a `List<ChatTurn>`. Typing `/history` prints all turns. Typing `/reset` clears them.

### Bonus
- Override `Chat:Model` with an environment variable (hint: `__` instead of `:`) and explain in a code comment why it beats `appsettings.json`.
- Add a unit test project (xUnit) that tests `FakeLlm` stops at `MaxTokens` and respects cancellation.

## Checkpoint questions (answer in `notes/M01-notes.md` or just tell me)

1. Why does a console app *not* load user-secrets automatically, and how did we fix it?
2. You set `Demo:Count` in appsettings.json, user-secrets **and** an env var. Which one wins, and why?
3. What's the difference between `IEnumerable<T>` and `IAsyncEnumerable<T>`? Why do LLM streams use the second?
4. What does `[EnumeratorCancellation]` do?
5. Why do we depend on an interface (`ITokenStream`) instead of the concrete class? How will that help with LLM providers?
6. In the walkthrough, `Demo:Count` is 10 but the output says "Short stream". Why? Fix it so 10 counts as long. Which line changes?
7. Where on disk is `secrets.json` stored, and why is that safer than `appsettings.Development.json`?
