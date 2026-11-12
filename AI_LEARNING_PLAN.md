# AI & AI Agents in .NET — Learning Plan (Microsoft Agent Framework)

**Learner:** Darshan · **Instructor:** Claude · **Started:** 2026-11-07
**Target:** From zero AI knowledge → building, shipping and operating production-grade AI agents and multi-agent systems in .NET.
**Pace:** ~16 weeks at 6–8 hrs/week (go faster or slower as you like; each module ends with a checkpoint, not a date).

---

## 1. How we will work together

Every lesson follows the same loop:

1. **Concept** – I explain the idea in plain language with a diagram or analogy.
2. **Walkthrough** – I show minimal, runnable C# code and explain every line.
3. **Lab** – *You* write the code for an exercise (I give hints, not answers, unless you ask).
4. **Review** – You tell me "review lab X"; I review your code like a senior engineer would.
5. **Checkpoint** – 5–10 quick questions. Move on when you can answer them without notes.

Commands you can use with me any time:

| Say | What happens |
|---|---|
| `start module N` | I teach the next lesson of module N |
| `review <folder>` | I review your lab code |
| `quiz me on <topic>` | Checkpoint questions |
| `explain <thing> simpler` | Re-explain with a different analogy |
| `I'm stuck on <error>` | We debug together |

All code lives in this folder (`C:\SF Projects\DotNet\AI`) as one solution:

```
AI/
├─ AI_LEARNING_PLAN.md       ← this file (we tick progress here)
├─ notes/                    ← lesson notes per module
├─ src/
│  ├─ M00-Fundamentals/
│  ├─ M01-Setup/
│  ├─ M02-ExtensionsAI/
│  ├─ ...                    ← one folder per module's labs
│  └─ Projects/              ← the 7 portfolio projects
└─ AI.slnx
```

---

## 2. The big picture (the "map" you'll keep coming back to)

```
            ┌──────────────────────────────────────────────┐
  Level 5   │  Production: hosting, observability, evals,  │  Module 11–12
            │  safety, cost, CI/CD                          │
            ├──────────────────────────────────────────────┤
  Level 4   │  Multi-agent systems & Workflows (MAF)       │  Module 9–10
            │  sequential · concurrent · handoff · group   │
            │  chat · magentic · human-in-the-loop · A2A   │
            ├──────────────────────────────────────────────┤
  Level 3   │  Single Agents (MAF): AIAgent, threads,      │  Module 6–8
            │  tools, memory, middleware, MCP              │
            ├──────────────────────────────────────────────┤
  Level 2   │  RAG: embeddings, vector stores, retrieval   │  Module 4–5
            ├──────────────────────────────────────────────┤
  Level 1   │  Microsoft.Extensions.AI: IChatClient,       │  Module 2–3
            │  prompts, streaming, structured output,      │
            │  function calling, embeddings                │
            ├──────────────────────────────────────────────┤
  Level 0   │  AI/LLM fundamentals + modern C# skills      │  Module 0–1
            └──────────────────────────────────────────────┘
```

**Key idea:** Microsoft Agent Framework (MAF) is the successor to Semantic Kernel and AutoGen. It is built *on top of* `Microsoft.Extensions.AI` (`IChatClient`). So we learn the lower layer first — everything in MAF will then feel obvious.

---

## 3. Tools & accounts (Module 1)

| Item | Status | Notes |
|---|---|---|
| .NET 10 SDK | ✅ installed (10.0.114) | Also .NET 9 present |
| Git | ✅ installed (2.47) | We'll init a repo for your work |
| IDE | ☐ | Visual Studio 2026 / VS Code + C# Dev Kit / Rider |
| LLM provider | ✅ **GitHub Models (free)** | Token in user-secrets → run `M01.SmokeTest` |
| Ollama (local models) | ☐ optional | Free, offline, private; good for experiments |
| Docker Desktop | ☐ later (Module 5) | For Qdrant / Postgres+pgvector / Aspire |
| Postman CLI | ✅ installed | Used to test agent HTTP endpoints (Module 11) |

**LLM provider options** (code stays the same across all of them thanks to `IChatClient`):

| Provider | Cost | Best for |
|---|---|---|
| **GitHub Models** | Free tier (rate limited) | Learning with zero cost |
| **Ollama** (llama3.x, qwen, phi) | Free, local | Offline, privacy; tool-calling is weaker on small models |
| **Azure OpenAI / Azure AI Foundry** | Pay-as-you-go | What most .NET companies use in production |
| **OpenAI** | Pay-as-you-go | Simplest paid setup |
| **Anthropic Claude** | Pay-as-you-go | Strong at tool use / agentic tasks |

> Recommendation: start with **GitHub Models (free)** or **Azure OpenAI** if your company has a subscription. Add **Ollama** later to learn local models.

---

## 4. Curriculum

### PHASE A — Foundations (Weeks 1–3)

#### Module 0 · How AI & LLMs actually work (Week 1) — *concepts first, little code*
Notes: `notes/M00-AI-Fundamentals.md`
- **0.1** AI vs ML vs Deep Learning vs Generative AI
- **0.2** What a Large Language Model is: training, next-token prediction, tokens, transformers (intuition, no math overload)
- **0.3** Talking to an LLM: chat roles (system / user / assistant / tool), context window, parameters (temperature, top-p, max tokens, stop, seed), token cost
- **0.4** Model families (chat, reasoning, embedding, image, speech, multimodal; small vs large) and limits: hallucinations, knowledge cutoff, statelessness → why RAG and tools exist
- **0.5** What an **AI Agent** is: LLM + instructions + tools + memory + a loop
- **Lab 0:** Call GitHub Models with raw `HttpClient` (no SDK): see the JSON request/response with your own eyes, read token usage, play with temperature.
- **Checkpoint 0**

#### Module 1 · Setup & Modern C# for AI apps (Week 1–2)
Code: `src/M01-Setup/` · Notes: `notes/M01-Setup-notes.md`
- Environment setup, user-secrets, `appsettings.json`, environment variables (never commit keys!)
- C# features used constantly in AI code: `async/await`, `IAsyncEnumerable` + `await foreach` (streaming), records, pattern matching, `System.Text.Json`, nullable refs
- Generic Host + Dependency Injection + `ILogger` + Options pattern
- GitHub Models smoke test via the OpenAI SDK
- **Lab 1:** "Fake LLM" chat console on Generic Host (see `src/M01-Setup/LAB1.md`).
- **Checkpoint 1**

#### Module 2 · Microsoft.Extensions.AI (MEAI) — the foundation layer (Week 2–3)
- `IChatClient`: `GetResponseAsync` vs `GetStreamingResponseAsync`
- `ChatMessage`, `ChatRole`, `ChatOptions`, `ChatResponse`, usage details
- Switching providers (OpenAI / Azure OpenAI / Ollama / GitHub Models) with **zero business-code changes**
- Conversation history management (you own the list!)
- **Structured output**: `GetResponseAsync<T>()` → strongly typed C# objects
- **Function calling** basics: `AIFunctionFactory.Create`, `UseFunctionInvocation()`
- The **middleware pipeline**: `ChatClientBuilder` + `.UseLogging()`, `.UseDistributedCache()`, `.UseOpenTelemetry()`, writing your own `DelegatingChatClient`
- DI registration: `services.AddChatClient(...)`
- Multimodal input: sending images
- **Lab 2a:** Streaming console chatbot with history and a token-usage footer.
- **Lab 2b:** Custom middleware that logs latency + cost per call.
- 🎯 **Project P1 — "TermChat"**: multi-provider CLI chatbot (switch model via config), streaming, `/reset`, `/system` commands, cost tracker.

#### Module 3 · Prompt Engineering for developers (Week 3)
- Anatomy of a great system prompt: role, task, constraints, output format, examples
- Zero-shot, few-shot, chain-of-thought, self-check, decomposition
- Prompt templates & keeping prompts in files (versioning prompts like code)
- Reasoning models vs chat models — when to use which
- Prompt injection — first look (we go deeper in Module 12)
- **Lab 3:** Classifier + extractor: turn messy free text (emails/tickets) into a typed record with validation & retry on bad JSON.
- 🎯 **Project P2 — "DocExtract"**: extract structured data (invoice / resume / vehicle service request) from text & images into C# records, saved to JSON/SQLite.

---

### PHASE B — Knowledge & Retrieval (Weeks 4–5)

#### Module 4 · Embeddings & Vector Search (Week 4)
- What an embedding is (meaning → numbers), cosine similarity, intuition with a 2-D toy
- `IEmbeddingGenerator<string, Embedding<float>>`
- `Microsoft.Extensions.VectorData` abstractions: collections, record models with `[VectorStoreKey]`, `[VectorStoreData]`, `[VectorStoreVector]` attributes
- Vector stores: InMemory → SQLite → Qdrant / Postgres pgvector / Azure AI Search
- **Lab 4:** Semantic search over 200 product/FAQ entries; compare with keyword search.

#### Module 5 · RAG — Retrieval-Augmented Generation (Week 5)
- RAG pipeline: ingest → chunk → embed → store → retrieve → augment prompt → generate
- Chunking strategies (fixed, sentence, markdown/heading-aware, overlap)
- Reading PDFs / Word / Markdown in .NET; `Microsoft.Extensions.DataIngestion` (if available in your version)
- Hybrid search (vector + keyword), metadata filtering, re-ranking
- Citations & "answer only from context" grounding
- Evaluating RAG quality: relevance, groundedness, faithfulness
- **Lab 5:** Ingestion pipeline with chunk viewer + retrieval debugger.
- 🎯 **Project P3 — "DocChat"**: ASP.NET Core + Blazor app — upload documents, ask questions, get answers with source citations.

---

### PHASE C — AI Agents with Microsoft Agent Framework (Weeks 6–8)

#### Module 6 · Your first agents (Week 6)
- Packages: `Microsoft.Agents.AI`, `Microsoft.Agents.AI.OpenAI`, `Microsoft.Agents.AI.AzureAI` (Foundry), etc.
- `AIAgent` abstraction, `ChatClientAgent`, creating an agent from any `IChatClient`
- Instructions, name, description
- `RunAsync` vs `RunStreamingAsync`, `AgentRunResponse` / updates
- **Threads / sessions**: multi-turn conversations, serializing & resuming a conversation
- **Tools**: C# methods as tools, `[Description]` attributes, parameter design that LLMs understand
- Tool-calling loop — what really happens under the hood (trace it!)
- Hosted/service agents (Azure AI Foundry Agents) vs local chat-client agents
- **Lab 6:** "Weather + Calculator + Time" agent; print every tool call it makes.

#### Module 7 · Agents — intermediate & advanced (Week 7)
- **Structured output** from agents
- **Agent as a tool** (one agent calling another)
- **Human-in-the-loop**: function approval (`ApprovalRequiredAIFunction`) — the agent asks before doing dangerous things
- **Memory**: short-term (thread) vs long-term; context providers (`AIContextProvider`) to inject user profile / facts; persisting threads to a database
- **Middleware**: agent-run middleware & function-call middleware (logging, guardrails, PII redaction, auth checks)
- Background / long-running responses, cancellation, retries, rate limits
- Agent design principles: single responsibility, small toolsets, good tool names, failure handling
- **Lab 7:** Agent that remembers user preferences across app restarts and asks approval before "sending" anything.
- 🎯 **Project P4 — "Personal Ops Agent"**: tool-using assistant over a mock calendar, to-do list and file system with approvals, memory and an audit log.

#### Module 8 · Model Context Protocol (MCP) & the tool ecosystem (Week 8)
- What MCP is and why it matters (USB-C for AI tools)
- MCP architecture: hosts, clients, servers; tools, resources, prompts; stdio vs Streamable HTTP transports
- Consuming MCP servers from a MAF agent (`ModelContextProtocol` C# SDK) — e.g. GitHub, filesystem servers
- **Building your own MCP server in C#** (`[McpServerTool]`), exposing an existing .NET Web API as tools
- Securing MCP: auth, least-privilege tools, tool poisoning risks
- Using your MCP server from Claude Code / VS Code Copilot as well
- 🎯 **Project P5 — "API→MCP Bridge"**: wrap a sample ASP.NET Core API (e.g. a mini dealer inventory / customer API) as an MCP server; an agent answers business questions using it.

---

### PHASE D — Multi-Agent Systems & Workflows (Weeks 9–11)

#### Module 9 · MAF Workflows — fundamentals (Week 9)
- Agents vs workflows: when the LLM decides the path vs when *you* define it
- Workflow building blocks: **Executors**, **Edges**, messages, `WorkflowBuilder`
- Edge types: direct, conditional, switch-case, fan-out / fan-in
- Running workflows: streaming events, `WorkflowOutputEvent`, superstep model
- Mixing plain C# executors with agent executors
- **Checkpointing** & resuming long-running workflows
- **Human-in-the-loop** with request/response ports
- Shared state between executors
- **Lab 9:** Document-approval workflow: classify → route (switch) → summarize → human approve → publish.

#### Module 10 · Multi-agent orchestration patterns (Week 10)
- **Sequential** (pipeline), **Concurrent** (parallel + aggregate), **Handoff** (triage → specialist), **Group chat** (manager-moderated), **Magentic** (planner-led dynamic orchestration)
- Choosing a pattern; cost/latency trade-offs; avoiding agent loops and runaway costs
- Workflow-as-an-agent (nesting)
- Declarative agents/workflows (YAML) — awareness
- 🎯 **Project P6 — "Content Studio"**: Researcher → Writer → Critic/Reviewer loop with handoff to a human editor, checkpointing and a live event UI.

#### Module 11 · Hosting, UI & interoperability (Week 11)
- Hosting agents in **ASP.NET Core** (minimal APIs, DI, `Microsoft.Agents.AI.Hosting`)
- Streaming to the browser: SSE, **AG-UI protocol**, Blazor chat UI
- **A2A (Agent-to-Agent) protocol**: exposing your agent, calling remote agents (even non-.NET ones)
- **DevUI** for local debugging of agents & workflows
- **.NET Aspire**: orchestrating app + vector DB + agents, dashboard with traces
- Testing endpoints with Postman CLI collections
- **Lab 11:** Expose the P4 agent via HTTP + A2A, call it from a second agent in another process.

---

### PHASE E — Production Engineering (Weeks 12–13)

#### Module 12 · Observability, Evaluation, Safety & Cost (Weeks 12–13)
- **Observability:** OpenTelemetry traces/metrics/logs for agents, tool calls and tokens; Aspire dashboard / Application Insights
- **Evaluation:** `Microsoft.Extensions.AI.Evaluation` (relevance, coherence, groundedness, tool-call accuracy), golden datasets, LLM-as-judge, regression evals in CI
- **Testing:** unit-testing agents with fake `IChatClient`s, deterministic tests for tools, snapshot tests for prompts
- **Responsible AI & Security:** prompt injection (direct/indirect), jailbreaks, data exfiltration via tools, content safety filters, PII handling, OWASP Top 10 for LLM apps, least-privilege tools
- **Cost & performance:** token budgeting, caching, model routing (small model first, big model when needed), batching, rate-limit handling with Polly / resilience handlers
- **Deployment:** containerize, Azure Container Apps / App Service, secrets in Key Vault, managed identity, CI/CD with GitHub Actions
- **Lab 12:** Add tracing + an eval suite + a prompt-injection test pack to Project P3 and run it in CI.

#### Module 13 (optional / bonus topics)
- Local & small models (Ollama, ONNX Runtime GenAI, Foundry Local)
- Speech (STT/TTS) & image generation
- Fine-tuning vs RAG vs prompting — decision guide
- Migrating from **Semantic Kernel / AutoGen** to MAF (useful if your company has existing SK code)
- Computer-use / browser agents (awareness)

---

### PHASE F — Capstone (Weeks 14–16)

🎯 **Project P7 — Capstone: "Dealer Service Desk Copilot"** (or a domain of your choice)

A realistic multi-agent system for an automotive dealership (swap the domain if you prefer):

| Agent | Responsibility |
|---|---|
| Triage agent | Understands customer message, hands off to the right specialist |
| Service agent | Books service appointments (MCP tools over a mock DMS API), approval before booking |
| Parts/Inventory agent | Checks stock & pricing via MCP |
| Knowledge agent | RAG over service manuals, warranty policies, FAQs with citations |
| Follow-up workflow | Checkpointed workflow: reminders, human escalation, summary to CRM |

Must include: ASP.NET Core + Blazor chat UI (streaming), threads persisted in DB, long-term customer memory, OpenTelemetry + Aspire dashboard, eval suite in CI, prompt-injection defenses, containerized deployment, README + architecture diagram. This is your **portfolio piece**.

---

## 5. Portfolio projects summary

| # | Project | Key skills | Module |
|---|---|---|---|
| P1 | TermChat | IChatClient, streaming, providers, middleware | 2 |
| P2 | DocExtract | Prompting, structured output, multimodal | 3 |
| P3 | DocChat | Embeddings, vector store, RAG, Blazor | 5 |
| P4 | Personal Ops Agent | MAF agents, tools, memory, approvals, middleware | 7 |
| P5 | API→MCP Bridge | MCP server & client | 8 |
| P6 | Content Studio | Workflows, multi-agent patterns, HITL, checkpoints | 10 |
| P7 | Dealer Service Desk Copilot | Everything, production-ready | 14–16 |

---

## 6. Progress tracker

| Module | Status | Checkpoint score | Notes |
|---|---|---|---|
| 0 · AI & LLM fundamentals | 🟡 in progress | | Started 2026-11-12 |
| 1 · Setup & C# for AI apps | 🟡 code ready | | Walkthrough + smoke test built; token + Lab 1 pending |
| 2 · Microsoft.Extensions.AI | ☐ | | |
| 3 · Prompt engineering | ☐ | | |
| 4 · Embeddings & vector search | ☐ | | |
| 5 · RAG | ☐ | | |
| 6 · First agents (MAF) | ☐ | | |
| 7 · Advanced agents | ☐ | | |
| 8 · MCP | ☐ | | |
| 9 · Workflows | ☐ | | |
| 10 · Multi-agent orchestration | ☐ | | |
| 11 · Hosting, UI, A2A | ☐ | | |
| 12 · Production engineering | ☐ | | |
| 13 · Bonus topics | ☐ | | |
| Capstone | ☐ | | |

---

## 7. Reference resources

- Microsoft Agent Framework docs: https://learn.microsoft.com/agent-framework/
- Agent Framework GitHub (samples in `dotnet/samples`): https://github.com/microsoft/agent-framework
- Microsoft.Extensions.AI docs: https://learn.microsoft.com/dotnet/ai/
- .NET AI samples: https://github.com/dotnet/ai-samples
- MCP C# SDK: https://github.com/modelcontextprotocol/csharp-sdk
- Model Context Protocol spec: https://modelcontextprotocol.io
- OWASP Top 10 for LLM Applications: https://genai.owasp.org

> ⚠️ MAF and MEAI evolve quickly (type and method names have changed between releases). At the start of each module we'll check the current package versions and docs so you always learn the up-to-date API.
