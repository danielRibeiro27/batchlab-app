# BatchLab – Async Batch Processing Lab

## 1. Overview

BatchLab is a **cloud‑native asynchronous batch processing system** designed to be built and delivered in **4 consecutive days** by two developers.

The project prioritizes:
- real delivery over ideal architecture
- managed cloud services
- explicit trade‑offs
- end‑to‑end functionality

The final artifact is intentionally simple, but architecturally honest, aiming to demonstrate **production‑grade async thinking** to recruiters.

---

## 2. Final Stack (Codespaces‑Friendly)

### Backend
- .NET 8
- ASP.NET Core Minimal API
- AWS SDK for .NET
- Messaging: **AWS SQS (Standard Queue)**
- Persistence: **AWS DynamoDB (On‑Demand)**

### Frontend
- Angular (standalone app)
- Simple form + status view

### Environment
- GitHub Codespaces only
- No local installations required
- Run with:
  - `dotnet run`
  - `ng serve`
- Configuration via environment variables

---

## 3. Explicitly Out of Scope (Deliberate Cuts)

These are **conscious scope decisions**, not omissions:
- Authentication / Authorization
- Redis or caching layers
- WebSockets / real‑time push
- Observability stack
- Infrastructure as Code
- Docker as a development requirement

---

## 4. What the System Does (End‑to‑End)

### High‑Level Consumption Flow

1. **UI** submits a job via `POST /jobs`
2. **API** validates input and creates a Job with status `QUEUED`
3. **API** publishes a minimal message to **SQS**
4. **SQS** stores the message durably
5. **Worker** consumes the message using long polling
6. **Worker** processes the job asynchronously
7. **Worker** updates Job status in **DynamoDB**
8. **UI** polls `GET /jobs/{id}` periodically
9. **API** reads from DynamoDB and returns current status

This full loop must work for the project to be considered delivered.

---

## 5. MVP – Non‑Negotiable Scope

### Backend
- `POST /jobs` – create a batch job
- `GET /jobs/{id}` – retrieve job status
- Publish messages to SQS
- Background worker consuming SQS
- Persist job status in DynamoDB

### Frontend
- Simple form to create a job
- Status view for a single job

### MVP Success Criteria
- Full async flow works end‑to‑end
- No manual intervention required

---

## 6. Architecture Layers

### Layer 1 – Core (Must Not Fail)
- API accepts job requests
- Message published to SQS
- Worker consumes message
- Processing visible via logs

### Layer 2 – Demonstrable Value
- Job status persisted in DynamoDB
- Status retrieval endpoint
- UI connected to API

### Layer 3 – Optional Extras (Only if Time Remains)
- Retry improvements
- Job listing per user
- Better UX polish

---

## 7. Roles & Ownership

### Daniel – Backend & Async Core
- Overall architecture
- .NET API
- SQS integration
- Worker implementation
- DynamoDB persistence
- Owner of Layer 1 (core reliability)

### Gabriel – Frontend & Integration
- Angular UI
- UI ↔ API integration
- User flow and basic UX
- Backend support when needed

**Rule:** each person owns their layer. Cross‑layer changes require alignment.

---

## 8. Scalability – Backend

### Throughput Targets
- Designed to handle **~1,000 requests/second** at peak
- Job creation: up to **~100 jobs/second**
- Status reads (polling): **~20–100 req/s**

### SQS Characteristics
- Queue type: Standard
- Backlog capacity: effectively unlimited
- Delivery model: at‑least‑once
- Horizontal scaling handled by AWS

### Worker Model
- Single‑threaded per instance (simplicity first)
- Scale by running multiple workers
- Long polling (no fixed timers)

---

## 9. Scalability – Frontend

### Polling Strategy
- No WebSockets or push mechanisms
- Polling interval: **~5 seconds**
- Polling is aggregated per user, not per job

### Load Example
- 100 users → ~20 req/s
- 500 users → ~100 req/s

Frontend is not the bottleneck; API and storage are.

---

## 10. Cost Model (Order of Magnitude)

### Assumptions
- 1 job / second / active user
- 100 active users
- 30‑day month

### SQS – Job Submission
- Jobs/month: ~259 million
- Free tier: 1 million
- Billable: ~258 million
- Price: ~$0.40 per million requests

**Estimated cost (no batching):**
- ~103 USD / month

**With batching (10 messages per request):**
- ~26 million requests
- ~10 USD / month

### DynamoDB (On‑Demand)
- Reads: polling driven
- Writes: job creation + updates
- Expected cost: low tens of USD/month

### Cost Insight
- SQS is not the financial bottleneck
- Request efficiency matters more than raw volume

---

## 11. 4‑Day Execution Plan (Parallel Work)

### Day 1 – Feasibility & Skeleton

**Daniel**
- Bootstrap .NET Minimal API
- Configure AWS SDK access
- Create SQS queue
- Publish and consume test message (logs only)

**Gabriel**
- Bootstrap Angular project
- Create basic layout (form + status view)
- Align API contract with backend

**Success:** API → SQS → Worker visible in logs

---

### Day 2 – Functional Core

**Daniel**
- Create DynamoDB table
- Persist job status
- Implement POST /jobs and GET /jobs/{id}

**Gabriel**
- Implement job creation UI
- Connect UI to POST /jobs

**Success:** Full async flow works via API tools

---

### Day 3 – UI Integration & Stabilization

**Daniel**
- Basic error handling
- Minimal idempotency safeguards

**Gabriel**
- Implement polling
- Display job state transitions

**Success:** Demo works end‑to‑end without explanation

---

### Day 4 – Cleanup & Delivery

**Joint**
- Code cleanup
- README and setup instructions
- Demo narrative preparation

**Success:** A third party can run and understand the project

---

## 12. Recruiter Value Proposition

This project demonstrates:
- Async system design
- Cloud‑managed messaging
- Clear ownership boundaries
- Cost awareness
- Scalability reasoning
- Pragmatic scope control

BatchLab is not a tutorial. It is a **real engineering artifact delivered under constraints**.

