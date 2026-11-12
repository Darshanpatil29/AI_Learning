# Module 0 — How AI & LLMs actually work

> Concepts first. By the end you should be able to explain to a colleague, without notes, what happens between "user types a question" and "the model answers", and why agents need tools and memory.

---

## Lesson 0.1 · AI, ML, Deep Learning, Generative AI — the family tree

```
┌───────────────────────────────────────────────────────────┐
│ ARTIFICIAL INTELLIGENCE — machines doing "smart" tasks     │
│  e.g. rule engines, chess programs, if/else expert systems │
│  ┌─────────────────────────────────────────────────────┐   │
│  │ MACHINE LEARNING — learns patterns from DATA         │   │
│  │  instead of hand-written rules                      │   │
│  │  e.g. spam filter, price prediction, fraud scoring  │   │
│  │  ┌───────────────────────────────────────────────┐  │   │
│  │  │ DEEP LEARNING — ML with neural networks of     │  │   │
│  │  │  many layers; learns its own features          │  │   │
│  │  │  e.g. image recognition, speech-to-text        │  │   │
│  │  │  ┌─────────────────────────────────────────┐   │  │   │
│  │  │  │ GENERATIVE AI — deep learning that       │   │  │   │
│  │  │  │  CREATES new content (text, image, code) │   │  │   │
│  │  │  │  ┌───────────────────────────────────┐   │   │  │   │
│  │  │  │  │ LLMs — generative models for text │   │   │  │   │
│  │  │  │  │ GPT, Claude, Llama, Phi, Mistral  │   │   │  │   │
│  │  │  │  └───────────────────────────────────┘   │   │  │   │
│  │  │  └─────────────────────────────────────────┘   │  │   │
│  │  └───────────────────────────────────────────────┘  │   │
│  └─────────────────────────────────────────────────────┘   │
└───────────────────────────────────────────────────────────┘
```

**The key shift, explained for a developer:**

| Traditional programming | Machine learning |
|---|---|
| You write **rules**: `if (mileage > 100000 && age > 8) risk = "High";` | You give **examples** (data + answers) and the algorithm *learns* the rules |
| Input + Rules → Output | Input + Output → Rules (a "model") |
| Deterministic, explainable | Probabilistic, "it's usually right" |

- **Model** = the learned rules, stored as millions or billions of numbers (**parameters / weights**).
- **Training** = adjusting those numbers until the model's answers match the examples. Expensive, done once by OpenAI, Meta, Microsoft and so on.
- **Inference** = *using* a trained model to get an answer. That's what **you** will do in every API call.

**Discriminative vs generative.** Classic ML *classifies or predicts* ("is this email spam?", "what will this car sell for?"). Generative AI *produces new content* ("write the email", "summarize this service history"). LLMs can do both, which is a big part of why they're so useful.

> 💡 As a .NET developer you will almost never *train* a model. You'll **consume** models via APIs and build the software around them. That's what "AI engineering" means, and it's what this course teaches.

---

## Lesson 0.2 · What is a Large Language Model?

### One sentence
An LLM is a deep neural network trained on a huge amount of text to do **one thing: predict the next token**.

### Tokens: the units LLMs actually read
Models don't see characters or words. They see **tokens**, which are chunks of text, each mapped to an integer ID.

```
"Darshan services vehicles"  →  ["D","arsh","an"," services"," vehicles"]  →  [35, 19821, 276, 3600, 11583]
```
(IDs are illustrative. Each model family has its own tokenizer.)

Rules of thumb for English with OpenAI-style tokenizers:
- 1 token ≈ 4 characters ≈ ¾ of a word
- 100 tokens ≈ 75 words; 1 page ≈ 500–700 tokens
- Rare words, names, code, JSON and non-English text use **more** tokens

Why you care: **you pay per token, limits are in tokens, and speed depends on tokens.**
Try it: https://platform.openai.com/tokenizer: paste a C# method and a paragraph of English and compare.

### Next-token prediction: the whole trick
```
Input:  "The customer wants to book a service for their"
Model:  probability for EVERY token in its vocabulary (~100k–200k of them)
           " car"      0.41
           " vehicle"  0.22
           " Honda"    0.06
           " BMW"      0.04
           ...
Pick one (" car"), APPEND it to the input, and repeat:
        "The customer wants to book a service for their car" → " next" → ...
```
That loop is called **autoregressive generation**. A 500-word answer means roughly 650 model runs, one per token. That's why answers **stream** in word by word, and why output tokens are slower and pricier than input tokens.

> 🤯 There is no database lookup and no "understanding" module. Everything an LLM appears to know (grammar, facts, C# syntax, reasoning) emerged from learning to predict the next token extremely well over trillions of tokens.

### How an LLM is made (3 stages)

| Stage | What happens | Result |
|---|---|---|
| **1. Pre-training** | Predict the next token on a huge corpus (web, books, code). Months on thousands of GPUs. | "Base model": knows language and facts, but just *continues text*. Ask it a question and it might add more questions. |
| **2. Instruction tuning (SFT)** | Train on curated *question → good answer* examples | Follows instructions and chats |
| **3. Preference tuning (RLHF / similar)** | Humans or AI rank answers; the model learns to prefer helpful, honest, safe ones | The assistant you use via API |

### Transformers: the architecture (intuition only)
LLMs are **transformers** (from Google's 2017 paper *"Attention Is All You Need"*). The key idea is **attention**: when processing each token, the model looks at *all the other tokens* and decides which ones matter.

```
"The mechanic fixed the car because IT was making a noise."
                                     ↑
         attention: "it" → strongly linked to "car", weakly to "mechanic"
```
Stacking dozens of attention layers lets the model track meaning, references and context across long texts. You don't need the maths to build great apps. Just remember this: **everything in the context window can influence every token of the answer.** That's both the power and the danger (prompt injection, Module 12).

### Parameters & model size
- "GPT-4o-mini", "Llama 3.1 **8B**", "Phi-4 **14B**": B = billions of parameters.
- Bigger usually means smarter, but also slower and more expensive. Small models (1–15B) can run on a laptop (Module 13).
- **Choosing a model is an engineering trade-off between quality, latency and cost.** You'll do it in every project.

---

## Lesson 0.3 · Talking to an LLM: messages, context window, parameters

### Chat messages and roles
Every chat API takes a **list of messages**:

| Role | Who writes it | Purpose |
|---|---|---|
| `system` (or `developer`) | **You**, the developer | Persona, rules, output format. "You are a service advisor for a car dealership. Answer in under 3 sentences." |
| `user` | The end user | Their question |
| `assistant` | The model | Its previous answers |
| `tool` | Your code | The result of a function the model asked you to call (Lesson 0.5) |

### ⚠️ LLMs are stateless
The model **remembers nothing between calls**. A "conversation" is an illusion: *your code* re-sends the whole history every time.

```
Call 1:  [system, user₁]                                   → assistant₁
Call 2:  [system, user₁, assistant₁, user₂]                → assistant₂
Call 3:  [system, user₁, assistant₁, user₂, assistant₂, user₃] → assistant₃
```
Consequences: every turn costs more input tokens than the last, and eventually the history won't fit. Managing that history is the job of **threads/sessions and memory** in Agent Framework.

### Context window
The maximum tokens for **input + output combined** in one call. Think of it as the model's working memory. Modern models range from ~8k to 1M+ tokens. GitHub Models' **free tier caps request size well below the model's maximum**; check the limits on each model's page.

### Generation parameters

| Parameter | What it does | Typical use |
|---|---|---|
| `temperature` (0–2) | Randomness when picking the next token. 0 means almost always pick the top token. | **0–0.3** for extraction, classification, code. **0.7–1.0** for creative writing |
| `top_p` | Only sample from the most likely tokens whose probabilities add up to p | Change temperature **or** top_p, not both |
| `max_tokens` (output) | Hard cap on answer length | Cost control; if hit, `finish_reason = "length"` (answer cut off!) |
| `stop` | Strings that end generation | Rarely needed with chat models |
| `seed` | Best-effort reproducibility | Testing |

> Even at temperature 0, outputs aren't *guaranteed* identical. **Never write code that assumes an LLM returns the same text twice.**

### Cost model
`cost = input_tokens × input_price + output_tokens × output_price` (prices are per 1M tokens; output is usually 3–5× the input price). Every API response includes a **usage** block telling you exactly how many tokens were used. In Lab 0 you'll read it.

---

## Lesson 0.4 · Model families & the limits that shape everything

### Model types you'll use in this course

| Type | Input → Output | Example use in our projects |
|---|---|---|
| **Chat / instruct** | text (+ images) → text | Chatbots, extraction, agents |
| **Reasoning** | text → "thinks" internally first, then answers | Hard multi-step problems; slower and pricier |
| **Embedding** | text → vector of numbers (meaning) | Semantic search, RAG (Module 4–5) |
| **Multimodal** | image / audio / text → text | Reading a photo of an invoice or a damaged car |
| **Image generation** | text → image | Bonus module |
| **Speech** | audio ⇄ text | Voice assistants (bonus) |

### The 4 limits, and the fix the industry built for each

| Limit | What it looks like | Fix (and where you learn it) |
|---|---|---|
| **Hallucination**: the model predicts *plausible* text, not *true* text | Invents a warranty policy or an API that doesn't exist, confidently | **Grounding with RAG**: give it the real documents (M4–5) + evaluations (M12) |
| **Knowledge cutoff**: it only knows its training data | Doesn't know today's date, your inventory or your customers | **Tools**: let it call your APIs and databases (M2, M6, M8) |
| **Stateless / limited context** | Forgets earlier conversations; long chats overflow | **Memory & threads** (M6–7) |
| **Can't act**: it only produces text | Can't actually book an appointment | **Function calling → agents** (M6+) |

Read that table twice. **The rest of this course is building those fixes.**

---

## Lesson 0.5 · From LLM to AI Agent

### Plain LLM call vs agent

```
PLAIN LLM CALL                        AI AGENT
──────────────                        ────────
question ──► LLM ──► answer            goal ──► ┌──────────────────────────┐
                                                │  LLM decides next step   │◄─┐
(one shot, only knows its training)             └────────────┬─────────────┘  │
                                                  needs info/action?          │
                                                 yes │            │ no        │
                                                     ▼            ▼           │
                                              call a TOOL     final answer    │
                                           (your C# method)                   │
                                                     │                        │
                                                     └── result added to ─────┘
                                                         context (memory)
```

**Agent = LLM (the brain) + Instructions (the job description) + Tools (hands) + Memory (notebook) + a Loop (keep going until done).**

### How tool / function calling actually works
The model **never executes code**. It only *asks* you to.

1. You send the user's message **plus a list of tool descriptions** (name, description, JSON schema of parameters):
   `GetVehicleServiceHistory(vin: string) — "Returns past service records for a vehicle"`
2. The model replies **not with text**, but with a structured request:
   `{ "tool_call": "GetVehicleServiceHistory", "arguments": { "vin": "1HGCM82633A004352" } }`
3. **Your code** runs the real C# method, then sends back a `tool` message with the result.
4. The model reads the result and either calls another tool or writes the final answer.

Microsoft Agent Framework automates steps 2–4 (the **loop**), which is exactly what you'll build by hand in Module 2 and then let MAF do for you in Module 6.

### Autonomy spectrum (where agents fit)

```
More control, predictable ◄──────────────────────────────────────► More autonomy, flexible
  Plain LLM call  →  LLM + tools  →  Single agent  →  Workflow of agents  →  Autonomous multi-agent
  (Module 2–3)       (Module 2)      (Module 6–8)     (Module 9)             (Module 10)
```
Professional rule: **use the least autonomy that solves the problem.** More autonomy means more cost, more unpredictability and more risk.

---

## Lab 0 · Talk to an LLM with raw HTTP (no SDK)

See `src/M00-Fundamentals/LAB0.md`. You'll do it after Lesson 0.5.

## Checkpoint 0 (answer without notes)

1. Explain the difference between traditional programming and machine learning in one sentence each.
2. What is a token? Roughly how many tokens is a 1,500-word document?
3. Why does an LLM's answer appear word by word?
4. A user says "the bot remembers what I said earlier, so the model has memory". Correct them.
5. You're extracting VIN numbers from emails. What temperature do you use, and why?
6. Your response has `finish_reason: "length"`. What happened, and what do you do?
7. Name the 4 limits of LLMs and the fix for each.
8. In function calling, who executes the function: the model or your code? Describe the 4 steps.
9. What's the difference between an LLM call, an agent and a workflow?
10. Why does a 20-turn conversation cost much more than 20 single questions?
