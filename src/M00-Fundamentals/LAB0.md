# Lab 0 — Talk to an LLM with raw HTTP (no SDK)

**Goal:** See *exactly* what goes over the wire when you "call AI". There's no magic: it's an HTTPS POST with JSON.
**Do this after:** Lessons 0.1–0.5 in `notes/M00-AI-Fundamentals.md`.
**When done:** tell me `review src/M00-Fundamentals/M00.RawHttp`.

## Before you start
You need a GitHub token with **Models: Read-only** permission (steps in `notes/M01-Setup-notes.md`, Step 1).
For this lab only, put it in an environment variable for your current terminal. Don't save it in a file:

```powershell
$env:GITHUB_MODELS_TOKEN = "PASTE_TOKEN_HERE"
```

## The API contract (OpenAI-compatible chat completions)

**Request:** `POST https://models.github.ai/inference/chat/completions`
Header: `Authorization: Bearer <token>`

```json
{
  "model": "openai/gpt-4.1-mini",
  "messages": [
    { "role": "system", "content": "You are a helpful car service advisor." },
    { "role": "user",   "content": "How often should I change engine oil?" }
  ],
  "temperature": 0.2,
  "max_tokens": 200
}
```

**Response (trimmed):**

```json
{
  "choices": [
    { "message": { "role": "assistant", "content": "..." }, "finish_reason": "stop" }
  ],
  "usage": { "prompt_tokens": 31, "completion_tokens": 87, "total_tokens": 118 }
}
```

## Tasks

```bash
dotnet new console -n M00.RawHttp -o src/M00-Fundamentals/M00.RawHttp
```

```bash
dotnet sln AI.slnx add src/M00-Fundamentals/M00.RawHttp
```

1. **First call.** Use only `HttpClient` + `System.Text.Json`. Send the request above and print the assistant's text, `finish_reason` and the 3 usage numbers. Model the JSON with C# **records**.
2. **See the raw JSON.** Also print the raw request body and raw response body once, so you know exactly what's sent.
3. **Temperature experiment.** Ask *"Suggest a name for a car-wash business"* 3 times at temperature `0`, then 3 times at `1.5`. Print the results side by side. What do you notice?
4. **Truncation.** Set `max_tokens` to `10`. What is `finish_reason` now? Handle it: print a warning `⚠️ answer was cut off`.
5. **Prove statelessness.** Call 1: *"My name is Darshan."* Call 2 (new message list): *"What is my name?"* Then call 2 again **with** the history included. Explain the difference in a code comment.
6. **Token cost.** Using `usage`, print the cost assuming $0.40 per 1M input tokens and $1.60 per 1M output tokens. Watch `prompt_tokens` grow in task 5's history version.
7. **Errors.** Handle 401 (bad token) and 429 (rate limit) with friendly messages. Test 401 by sending a wrong token.

### Bonus
- Use `"stream": true` and read the Server-Sent Events (`data: {...}` lines) with `StreamReader` to print tokens as they arrive. This is what the SDK does for you in Module 2.
