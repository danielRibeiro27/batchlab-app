# BatchLab

## Overview

BatchLab is an asynchronous batch processing API. The system receives processing requests (jobs), publishes these tasks to a queue, processes them in the background, and allows you to track the status via a simple API and UI.

The focus is on **functional delivery in 4 days**, with a clear architecture, controlled scope, and exclusive use of **managed services**, compatible with GitHub Codespaces.

---

## Definitive Stack

### Backend

* .NET 8 (ASP.NET Core Minimal API)
* AWS SDK for .NET
* Messaging: AWS SQS (Standard Queue)
* Persistence: AWS DynamoDB

### Frontend

* Angular (standalone, no Nx)
* Communication via HTTP with the API

### Environment

* GitHub Codespaces
* Local execution via `dotnet run` and `ng serve`
* Configuration via environment variables

### Outside the technical scope

* Docker as a development requirement
* Caching (Redis)
* Authentication/authorization
* Advanced observability
* Infrastructure as code

---

## Final Artifact

### What the system does

1. User creates a batch processing job
2. API registers the job as `Queued`
3. API publishes message to SQS
4. Worker consumes The message:

5. Simulated processing is executed
6. Job status is updated
7. UI displays the status

---

## Minimum Acceptable MVP

### Backend

* `POST /jobs` – creates a job
* `GET /jobs/{id}` – queries status
* Publication to SQS
* Worker consuming messages
* Status persistence in DynamoDB

### Frontend

* Simple form for job creation
* Job status visualization

### MVP Success Criteria

* Functional end-to-end flow without manual intervention

---

## Division of Responsibilities

### Daniel

* General architecture
* .NET Backend
* Integration with SQS
* Processing worker
* Persistence in DynamoDB
* Ensuring Layer 1 functionality

### Gabriel

* UI in Angular
* UI ↔ API integration
* Visual structure and user flow
* Occasional support On the backend

Explicit rule: each layer owns its own layer. Cross-layer changes only with alignment.

---

## Layered Organization

### Layer 1 – Mandatory Core

* API receives job
* Message published to SQS
* Worker consumes
* Processing log

### Layer 2 – Demonstrable Value

* Status persistence
* Job query
* Minimal UI

### Layer 3 – Extras (only if there's time left)

* UX improvements
* Simple retry
* Job pagination/listing

---

## Closed Plan per Day

### Day 1 – Proof of Viability

**Objectives**

* Repository created
* API uploaded to Codespaces
* SQS queue created
* Message published and consumed

**Successful Day Criteria**

* Console confirms: API → SQS → Worker

**Cutoff Point**

* If DynamoDB is delayed, status in memory

---

### Day 2 – Functional Core

**Objectives**

* DynamoDB working
* Persistent status
* Complete endpoints

**Successful Day Criteria**

* Complete flow via Postman/curl

**Cutoff Point**

* If UI is delayed, full focus on the backend

---

### Day 3 – UI and Stabilization

**Objectives**

* UI creates job
* UI queries status
* Basic error handling

**Successful Day Criteria**

* Functional demo without technical explanation

**Cutoff Point**

* Ugly UI is acceptable; Broken UI not included

---

### Day 4 – Finalization and Delivery

**Objectives**

* Clear README
* Execution script
* Minor code cleanup
* Demo preparation

**Successful Day Criteria**

* Someone clones the repository and runs it on Codespaces

---

## Expected Result

* A functional and demonstrable MVP
* Clear asynchronous processing architecture
* Real-world learning in messaging and cloud
* Delivery completed in 4 days without overengineering
