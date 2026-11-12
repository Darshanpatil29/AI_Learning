# Azure for .NET Developers — Learning Roadmap & Certification Guide

**Learner:** Darshan · **Instructor:** Claude · **Created:** 2026-11-11
**Target:** From "I've heard of Azure" → designing, building, securing, deploying and operating cloud-native and AI-powered .NET applications on Azure — and proving it with Microsoft certifications.
**Pace:** ~20 weeks at 6–8 hrs/week. Runs **alongside** the AI plan ([LEARNING_PLAN.md](LEARNING_PLAN.md)); the two meet in AI Module 12 and the capstones.

---

## 1. How we will work together

Same loop as the AI plan:

1. **Concept** – plain-language explanation + architecture diagram.
2. **Walkthrough** – I show the Portal → CLI/Bicep → C# SDK way of doing the same thing (in that order, so you see what the automation is actually automating).
3. **Lab** – *you* build it in your own subscription.
4. **Review** – `review azure lab X` → I review code, Bicep, and the security/cost choices.
5. **Checkpoint** – exam-style questions (scenario + "choose the best service").
6. **Tear down** – every lab ends with `az group delete`. Cost discipline is part of the skill.

Commands you can use with me:

| Say | What happens |
|---|---|
| `start azure module N` | Next lesson of Azure module N |
| `review azure lab <folder>` | Code + infra + security review |
| `exam drill <exam code>` | 10 exam-style scenario questions with explanations |
| `which service for <scenario>` | Service-selection reasoning (the #1 exam skill) |
| `estimate cost of <design>` | Rough monthly cost + cheaper alternatives |
| `I'm stuck on <error>` | Debug together |

Folder layout (inside this same repo):

```
AI/
├─ AZURE_LEARNING_PLAN.md     ← this file
├─ notes/azure/               ← your notes + service-comparison cheat sheets
├─ infra/                     ← Bicep / azd templates per lab
└─ src/Azure/
   ├─ AZ01-Fundamentals/
   ├─ AZ02-AppService/
   ├─ ...
   └─ Projects/               ← the 5 portfolio projects
```

---

## 2. The big picture

```
            ┌──────────────────────────────────────────────────┐
  Level 5   │  Architect & operate: Well-Architected, DevOps,  │  Module 12–13
            │  IaC pipelines, cost, reliability                 │
            ├──────────────────────────────────────────────────┤
  Level 4   │  AI on Azure: Foundry, Azure OpenAI, AI Search,  │  Module 10–11
            │  agents, content safety  (← AI plan plugs in here)│
            ├──────────────────────────────────────────────────┤
  Level 3   │  Integration & events: Service Bus, Event Grid,  │  Module 7–8
            │  Event Hubs, API Management                      │
            ├──────────────────────────────────────────────────┤
  Level 2   │  Data & storage: Blob, Cosmos DB, Azure SQL,     │  Module 5–6
            │  Redis   +  Security: Entra ID, Key Vault, MI    │
            ├──────────────────────────────────────────────────┤
  Level 1   │  Compute: App Service, Functions, Containers,    │  Module 2–4
            │  Container Apps, AKS (awareness)                 │
            ├──────────────────────────────────────────────────┤
  Level 0   │  Cloud fundamentals, subscriptions, RGs, CLI,    │  Module 0–1
            │  Bicep basics, cost management                   │
            └──────────────────────────────────────────────────┘
       Cross-cutting at every level:  Identity · Monitoring · IaC · Cost
```

**Key idea:** As a developer you don't need to memorize 200 services. You need to (1) pick the right service for a scenario, (2) connect to it **without secrets** (Managed Identity + `DefaultAzureCredential`), (3) deploy it repeatably (Bicep/azd), and (4) see what it's doing (Application Insights). Every module drills those four.

---

## 3. Certification guidance (read this first)

> ⚠️ **Microsoft reshuffled the developer certifications in 2026.** As of Nov 2026 (verify on Microsoft Learn before booking — dates below come from Microsoft's study-guide notices and third-party trackers):
>
> - **AZ-204 (Azure Developer Associate) retired 31 July 2026.** Its successor is **AI-200 — Developing AI Cloud Solutions on Azure** → *Azure AI Cloud Developer Associate*.
> - **AI-102 (Azure AI Engineer) retired 30 June 2026.** Successor: **AI-103 — Developing AI Apps and Agents on Azure** → *Azure AI Apps and Agents Developer Associate*.
> - **AI-900 retired 30 June 2026**; the *Azure AI Fundamentals* cert continues via **AI-901**.
> - **DP-420** is being reworked into *Azure Cosmos DB **AI** Developer Associate* (from Oct 2026).
> - **AZ-400 (DevOps Engineer Expert)**: at least one tracker reports a retirement announced for 1 Feb 2027 — **unconfirmed**; check before investing in it.
> - Old study material (Udemy courses, practice tests, blog posts) written for AZ-204/AI-102 is still ~60–70% useful for the *skills*, but **do not use it as your exam prep**.

### Recommended path for you (.NET developer, building AI agents)

```
 Week 4           Week 11              Week 16                 Week 20+
┌────────┐      ┌──────────┐        ┌──────────┐          ┌───────────────────┐
│ AZ-900 │ ───▶ │  AI-200  │ ─────▶ │  AI-103  │ ───────▶ │ Choose one:       │
│(option)│      │ AI Cloud │        │ AI Apps &│          │ • AZ-104 → AZ-305 │
└────────┘      │ Developer│        │  Agents  │          │   (Architect Exp.)│
 Fundamentals   └──────────┘        └──────────┘          │ • AZ-400 (DevOps) │
                 ★ core cert         ★ pairs with AI plan │ • Cosmos DB AI Dev│
                                                          └───────────────────┘
```

| Exam | Credential | Level | Take it? | Why |
|---|---|---|---|---|
| **AZ-900** | Azure Fundamentals | Fundamentals | Optional | Cheap confidence boost; skip if you'll go straight to AI-200. Great if your employer pays / free via Virtual Training Days. |
| **AI-901** | Azure AI Fundamentals | Fundamentals | Skip | Your AI plan covers far more than this. |
| **AI-200** | Azure AI Cloud Developer Associate | Associate | ★ **Yes — first priority** | The new "Azure developer" cert. Compute, storage, data, security, messaging + AI integration. Successor to AZ-204. |
| **AI-103** | Azure AI Apps & Agents Developer Associate | Associate | ★ **Yes** | Foundry, Azure OpenAI, RAG, agents — directly matches your MAF learning. |
| **AZ-104** | Azure Administrator Associate | Associate | Later | Needed as the prerequisite for AZ-305. Networking/VM depth. |
| **AZ-305** | Solutions Architect Expert | Expert | Later (career goal) | Requires AZ-104. Best long-term "senior/lead" signal. |
| **AZ-400** | DevOps Engineer Expert | Expert | Maybe | Only if retirement rumor is false and you own CI/CD at work. |
| **Cosmos DB AI Dev** (ex DP-420) | Associate (new) | Associate | Optional | If your products are Cosmos-heavy or you build vector search on Cosmos. |

**Language note:** reports say the new AI-200/AI-103 exams lean on **Python** for code snippets. The concepts are identical in C#; in Module 10 we'll do a short "read Python SDK code fluently" session so code questions don't surprise you. Confirm on the official study guide's "skills measured" when you book.

### Exam strategy (applies to every role-based exam)
- **Official study guide first.** Each exam page on Microsoft Learn has a "Skills measured" PDF/page with domain weights — we'll map every module below to it.
- **Free Practice Assessment** on Microsoft Learn for each exam — take it at the *start* (baseline) and *end* (target ≥ 80%).
- **Exam sandbox** (Learn) to get used to the UI, case studies and question types.
- **Microsoft Learn is open during role-based exams** (not during fundamentals) — practice finding docs fast, but it costs time; use it for 2–3 questions max.
- **Free/discounted vouchers:** Microsoft Virtual Training Days, Cloud Skills Challenges, and employer partner benefits (check if your company is a Microsoft Partner — often includes vouchers).
- **Renewal:** associate/expert certs are valid 1 year; renew **free** with an online, unproctored assessment on Learn in the last 6 months.
- **Book the exam date when you start the module block** — a deadline beats motivation.

---

## 4. Tools & accounts (Module 0)

| Item | Status | Notes |
|---|---|---|
| Azure subscription | ☐ | Free account ($200 credit/30 days + 12 months free services) **or** Visual Studio subscription credits ($50–150/month) **or** a company sandbox subscription |
| Azure CLI (`az`) | ☐ | `winget install Microsoft.AzureCLI` |
| Azure Developer CLI (`azd`) | ☐ | `winget install Microsoft.Azd` — template-based provision + deploy |
| Bicep | ☐ | Comes with `az bicep install`; VS Code Bicep extension |
| VS Code Azure extensions / VS Azure workload | ☐ | |
| Docker Desktop | ☐ | Shared with AI plan Module 5 |
| Azurite + Cosmos DB emulator | ☐ | Local Storage/Cosmos emulators — free labs offline |
| .NET Aspire | ☐ | Shared with AI plan Module 11; `azd` deploys Aspire apps to Container Apps |
| GitHub account + Actions | ✅ (assumed) | CI/CD with OIDC federation (no secrets) |
| Postman CLI | ✅ installed | API testing for APIM / Functions endpoints |

**💰 Cost guardrails — set these up on day 1 (Lab 0):**
1. A **budget** with email alerts at 50% / 80% / 100%.
2. One **resource group per lab**, always tagged `owner=darshan`, `lab=<id>`; delete at end.
3. Prefer free/consumption tiers: Functions Flex/Consumption, Container Apps (scale-to-zero), Cosmos DB free tier, App Service F1/B1, Azure SQL serverless free offer.
4. Never leave running: AKS, APIM non-consumption tiers, AI Search above Free, VMs, Front Door, provisioned OpenAI throughput.

---

## 5. Curriculum

### PHASE A — Cloud Foundations (Weeks 1–3) → *AZ-900 optional at week 4*

#### Module 0 · Setup, cost control & the Azure mental model (Week 1)
- Tenant → management groups → subscriptions → resource groups → resources; regions, availability zones, region pairs
- Azure Resource Manager (ARM): everything is a REST API call — Portal, CLI, Bicep and SDKs are just clients
- Pricing, TCO, Cost Management, budgets, tags, Azure Advisor
- Shared responsibility; IaaS vs PaaS vs SaaS vs serverless
- **Lab 0:** Install tooling, create budget + alerts, create/tag/delete a resource group from Portal, CLI and PowerShell.

#### Module 1 · Infrastructure as Code & identity basics (Week 2–3)
- **Bicep:** resources, parameters, outputs, modules, `existing`, what-if; why not click-ops
- `azd` templates: `azure.yaml`, `infra/`, `azd up` / `azd down`
- **Microsoft Entra ID** fundamentals: users, groups, app registrations vs service principals vs **managed identities**
- **Azure RBAC**: roles, scopes, least privilege; data-plane vs control-plane roles (common exam trap!)
- `DefaultAzureCredential` in .NET — one line that works locally (your `az login`) and in Azure (managed identity)
- Azure Policy (awareness), resource locks
- **Lab 1:** Bicep that deploys a Storage account; a .NET console app uploads a blob using `DefaultAzureCredential` — **zero connection strings**.
- **Checkpoint 1** (+ AZ-900 practice assessment if you're taking it)

---

### PHASE B — Compute for .NET apps (Weeks 4–6)

#### Module 2 · App Service (Week 4)
- Plans & tiers, scaling (up/out, autoscale rules), deployment slots + swap, app settings & Key Vault references
- Deploy ASP.NET Core via `dotnet publish`, GitHub Actions, `azd`; Linux vs Windows
- Custom domains & TLS, authentication ("Easy Auth"), diagnostic logs
- **Lab 2:** Deploy a minimal API with a staging slot; do a zero-downtime swap.

#### Module 3 · Azure Functions & serverless (Week 5)
- **Isolated worker model** (.NET 8/10) — the only model going forward
- Triggers & bindings: HTTP, Timer, Blob, Queue, Service Bus, Cosmos DB change feed, Event Grid
- Hosting plans: Flex Consumption vs Premium vs Dedicated; cold start; scaling
- **Durable Functions**: orchestrations, activities, fan-out/fan-in, human interaction (timers + external events) — compare with MAF workflows!
- Local dev with Core Tools + Azurite
- **Lab 3:** Blob upload → Function trigger → resize/process → write result to Queue; Durable approval orchestration.

#### Module 4 · Containers: ACR, Container Apps, (AKS awareness) (Week 6)
- Dockerfile for .NET (multi-stage, chiseled images, `dotnet publish /t:PublishContainer`)
- **Azure Container Registry**: push, tasks, MI pull
- **Azure Container Apps**: revisions, ingress, scale rules (HTTP, KEDA/queue), Dapr (awareness), jobs, secrets
- Deploying **.NET Aspire** apps with `azd` to Container Apps
- Container Instances (ACI) vs Container Apps vs AKS vs App Service containers — when to choose which
- **Lab 4:** Containerize your AI-plan TermChat/DocChat API → ACR → Container Apps with scale-to-zero.
- 🎯 **Project AZ-P1 — "Service Booking API"**: ASP.NET Core API for dealer service appointments, Bicep/azd infra, deployed to Container Apps, CI/CD from GitHub Actions with **OIDC federated credentials** (no secrets in GitHub).

---

### PHASE C — Data, Storage & Security (Weeks 7–9)

#### Module 5 · Storage & relational data (Week 7)
- **Blob Storage**: tiers (hot/cool/cold/archive), lifecycle policies, SAS (user-delegation SAS!), metadata, leases, soft delete, versioning
- Queue / Table storage — when they're enough
- **Azure SQL Database**: serverless, Entra-only auth with managed identity from EF Core, elastic pools (awareness)
- Azure Cache for **Redis** / Azure Managed Redis: cache-aside, output cache, session state with `IDistributedCache` / HybridCache
- **Lab 5:** Add Azure SQL (EF Core + MI) and Redis HybridCache to AZ-P1.

#### Module 6 · Cosmos DB (Week 8)
- Data modeling for NoSQL: partition keys (the most important decision), embedding vs referencing, RU/s
- Consistency levels (5 of them — exam favorite), global distribution
- .NET SDK: CRUD, queries, transactional batch, **change feed** + Functions trigger
- Provisioned vs autoscale vs serverless; free tier
- **Vector search in Cosmos DB** — bridge to AI plan Modules 4–5
- **Lab 6:** Store vehicle service history in Cosmos; change feed updates a read model.

#### Module 7 · Secure your solutions (Week 9)
- **Key Vault**: secrets, keys, certificates; RBAC model; App Configuration + feature flags
- Managed identity everywhere: system vs user-assigned; one identity → many resources
- Securing APIs: Microsoft Identity Web, **Entra ID app registrations**, scopes vs app roles, OAuth2 flows (auth code + PKCE, client credentials, on-behalf-of)
- Network basics for devs: VNets, private endpoints, service endpoints, NSGs (enough to pass dev exams + talk to ops)
- Defender for Cloud recommendations (awareness)
- **Lab 7:** Lock down AZ-P1: Entra-protected API with scopes, all secrets in Key Vault, storage/SQL via private endpoint (optional—costs a bit).

---

### PHASE D — Integration, Messaging & Observability (Weeks 10–11) → *AI-200 exam at end of week 11*

#### Module 8 · Messaging, events & APIs (Week 10)
- **Service Bus** (queues, topics/subscriptions, sessions, dead-lettering, duplicate detection) vs **Storage Queues**
- **Event Grid** (reactive events, CloudEvents) vs **Event Hubs** (streaming telemetry) — the classic "which one" exam question
- **API Management**: products, subscriptions, policies (rate-limit, validate-jwt, caching, transformation), **AI gateway** policies (token limits, load-balance across OpenAI backends, semantic caching)
- **Lab 8:** Booking created → Service Bus topic → Functions (email + CRM sync) → Event Grid notification; front the API with APIM (consumption tier).

#### Module 9 · Monitor, troubleshoot & optimize (Week 11)
- **Azure Monitor**: metrics, logs, **Log Analytics + KQL** (learn 10 queries you'll use forever)
- **Application Insights** with **OpenTelemetry** (Azure Monitor OpenTelemetry Distro for .NET): distributed traces, dependency tracking, live metrics, availability tests, alerts
- Caching & CDN / Front Door (awareness) for performance
- **Lab 9:** End-to-end trace from APIM → API → Service Bus → Function → Cosmos in App Insights; write KQL alert for failed bookings.
- 🎯 **Project AZ-P2 — "Event-Driven Service Desk Backend"**: AZ-P1 extended with Cosmos, Service Bus, Functions, APIM, Key Vault, full observability. All Bicep, all managed identity.
- 🏁 **CERT MILESTONE: AI-200** — take the Practice Assessment at week 9, book the exam for end of week 11 / start of week 12. One week of `exam drill AI-200` before.

---

### PHASE E — AI on Azure (Weeks 12–16) → *AI-103 exam at end of week 16*
*(Synchronize with AI plan Modules 6–12 — same code, now on Azure infrastructure.)*

#### Module 10 · Microsoft Foundry & Azure OpenAI (Weeks 12–13)
- **Microsoft Foundry** (formerly Azure AI Foundry/Studio): hubs/projects, model catalog, deployments (standard, global, data-zone, provisioned), quotas & TPM/RPM
- Azure OpenAI from .NET via `IChatClient` (`Azure.AI.OpenAI` + MEAI) with **Entra auth (no API keys)**
- Prompt flow / evaluations in Foundry; tracing to App Insights
- **Azure AI Content Safety**, Prompt Shields, groundedness detection; Responsible AI requirements
- Foundry Agent Service: hosted agents, threads, built-in tools (file search, code interpreter, Bing grounding), and using them from MAF (`Microsoft.Agents.AI.AzureAI`)
- Python SDK reading session (for exam code snippets)
- **Lab 10:** Move your MAF agent (AI plan P4) from GitHub Models → Azure OpenAI in Foundry with managed identity + content safety.

#### Module 11 · Azure AI Search, RAG & classic AI services (Weeks 14–15)
- **Azure AI Search**: indexes, indexers, skillsets, integrated vectorization, hybrid + semantic ranker, agentic retrieval
- RAG on Azure end-to-end ("On your data" vs build-it-yourself)
- Classic AI services still on AI-103: **Document Intelligence** (prebuilt + custom models), **Vision**, **Language** (NER, PII, CLU), **Speech** (STT/TTS/translation), **Translator**
- Containers for AI services, private networking, cost control
- **Lab 11:** DocChat (AI plan P3) backed by Azure AI Search + Document Intelligence ingestion.
- 🎯 **Project AZ-P3 — "Service Manual Copilot on Azure"**: Foundry agent + AI Search RAG + Document Intelligence ingestion + Content Safety + App Insights tracing, deployed via `azd`.
- 🏁 **CERT MILESTONE: AI-103** — Practice Assessment at week 14, exam end of week 16.

---

### PHASE F — Architecture & DevOps (Weeks 17–20)

#### Module 12 · DevOps for Azure apps (Weeks 17–18)
- GitHub Actions (and Azure Pipelines awareness): build → test → Bicep what-if → deploy → smoke test
- Environments, approvals, **OIDC federation**, deployment slots/revisions for blue-green & canary
- IaC testing, `azd pipeline config`, policy-as-code
- Release safety: feature flags (App Configuration), health checks, automatic rollback
- **Lab 12:** Multi-environment (dev/prod) pipeline for AZ-P2 with manual approval gate.

#### Module 13 · Architecting solutions (Weeks 19–20)
- **Azure Well-Architected Framework**: reliability, security, cost optimization, operational excellence, performance efficiency
- Cloud design patterns: retry, circuit breaker, queue-based load leveling, CQRS, saga, strangler fig, cache-aside, gateway offloading
- High availability & DR: zones, geo-replication, RTO/RPO, backups
- Multi-tenant SaaS on Azure (very relevant for dealer platforms): tenancy models, deployment stamps, noisy neighbors
- Landing zones & Cloud Adoption Framework (awareness)
- 🎯 **Project AZ-P4 — Architecture review**: write an ADR set + diagram for taking AZ-P2/AZ-P3 multi-tenant and multi-region; cost estimate with the Pricing Calculator.

### Capstone (with AI plan P7)
🎯 **Project AZ-P5 — "Dealer Service Desk Copilot — production on Azure"**: deploy the AI-plan capstone with Container Apps + Foundry agents + AI Search + Cosmos DB + Service Bus + APIM AI gateway + Key Vault + App Insights, fully in Bicep/azd with a CI/CD pipeline and a Well-Architected review. **This is your portfolio piece for both plans.**

---

## 6. Exam ↔ module map

| Exam domain (approx.) | Modules |
|---|---|
| **AZ-900** — cloud concepts, architecture & services, management & governance | 0, 1, (skim 2–6) |
| **AI-200** — compute (App Service, Functions, containers) | 2, 3, 4 |
| **AI-200** — storage & data (Blob, Cosmos DB, SQL, Redis, vector) | 5, 6 |
| **AI-200** — security (Entra, MI, Key Vault, App Config) | 1, 7 |
| **AI-200** — integration & monitoring (Service Bus, Event Grid/Hubs, APIM, App Insights) | 8, 9 |
| **AI-200** — AI integration (Azure OpenAI, AI pipelines) | 10 (early), AI plan 2–5 |
| **AI-103** — plan & manage Azure AI / Foundry solution | 10 |
| **AI-103** — generative AI & agentic solutions (30–35%) | 10, AI plan 6–10 |
| **AI-103** — vision, language, speech, information extraction, knowledge mining | 11 |
| **AZ-305** — identity/governance, data storage, BC/DR, infrastructure | 7, 13 (+ AZ-104 prep) |
| **AZ-400** — source control, CI/CD, IaC, monitoring, security | 1, 9, 12 |

> We'll replace the "approx." weights with the exact percentages from each official study guide at the start of that phase.

---

## 7. Portfolio projects summary

| # | Project | Key Azure skills | Module |
|---|---|---|---|
| AZ-P1 | Service Booking API | Container Apps, ACR, Bicep/azd, GitHub OIDC | 4 |
| AZ-P2 | Event-Driven Service Desk Backend | Cosmos, SQL, Redis, Service Bus, Functions, APIM, Key Vault, App Insights | 9 |
| AZ-P3 | Service Manual Copilot on Azure | Foundry, Azure OpenAI, AI Search, Doc Intelligence, Content Safety | 11 |
| AZ-P4 | Architecture review | Well-Architected, patterns, multi-tenant, cost | 13 |
| AZ-P5 | Copilot in production | Everything | Capstone |

---

## 8. Progress tracker

| Module | Status | Checkpoint | Notes |
|---|---|---|---|
| 0 · Setup & cost control | ☐ | | |
| 1 · IaC & identity basics | ☐ | | |
| 2 · App Service | ☐ | | |
| 3 · Functions & Durable | ☐ | | |
| 4 · Containers / Container Apps | ☐ | | |
| 5 · Storage, SQL, Redis | ☐ | | |
| 6 · Cosmos DB | ☐ | | |
| 7 · Security | ☐ | | |
| 8 · Messaging & APIM | ☐ | | |
| 9 · Monitoring | ☐ | | |
| 10 · Foundry & Azure OpenAI | ☐ | | |
| 11 · AI Search & AI services | ☐ | | |
| 12 · DevOps | ☐ | | |
| 13 · Architecture | ☐ | | |
| Capstone | ☐ | | |

| Certification | Practice assessment | Booked | Result | Renew by |
|---|---|---|---|---|
| AZ-900 (optional) | | | | n/a (no expiry) |
| AI-200 | | | | |
| AI-103 | | | | |
| Next (AZ-104/305 · AZ-400 · Cosmos AI) | | | | |

---

## 9. Reference resources

- Microsoft Learn — browse credentials: https://learn.microsoft.com/credentials/browse/
- Retired exams list (check before booking anything): https://learn.microsoft.com/credentials/support/retired-certification-exams
- Exam sandbox & practice assessments: linked from each exam's page on Learn
- Azure for .NET developers: https://learn.microsoft.com/dotnet/azure/
- Azure SDK for .NET: https://learn.microsoft.com/dotnet/api/overview/azure/
- Bicep docs: https://learn.microsoft.com/azure/azure-resource-manager/bicep/
- `azd` templates gallery: https://azure.github.io/awesome-azd/
- Azure Architecture Center & Well-Architected: https://learn.microsoft.com/azure/architecture/ , https://learn.microsoft.com/azure/well-architected/
- Microsoft Foundry docs: https://learn.microsoft.com/azure/ai-foundry/
- Azure Pricing Calculator: https://azure.microsoft.com/pricing/calculator/

> ⚠️ Azure renames services and Microsoft reshapes exams often (2026 was a big year). At the start of every phase we re-check the official study guide and service names so you always prepare against the current version.
