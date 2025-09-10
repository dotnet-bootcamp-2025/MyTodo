# Phase 0 — Bootstrap & Run

**Goal:** Get the solution running and understand the structure.

## What you’ll learn
- Solution layout (Domain, Application, Infrastructure, Console, API, Tests)
- How to run the Console and API projects

## Prereqs
- .NET SDK installed (same as repo)
- Git + an editor (VS 2022 Community Edition / Rider / VS Code)

## Steps
1) **Clone & open**
   - Open a terminal
   - Navigate to your workspace (suggested: `C:\Dev\BootCamp`) 
   - Type the following Git Command to Clone the repo: `git clone https://github.com/dotnet-bootcamp-2025/MyTodo`
   - Open `MyTodo.sln`

2) **Explore the structure**
   - `src/TodoApp.Domain` – entities and contracts
   - `src/TodoApp.Application` – use cases / services
   - `src/TodoApp.Infrastructure` – data access, in-memory store (later)
   - `src/TodoApp.Console` – console UI
   - `src/TodoApp.Api` – Minimal API (optional for this phase)
   - `tests/` – unit tests (later)

3) **Run the Console app**
   ```bash
   cd src/TodoApp.Console
   dotnet run


- From **Visual Studio**:
1. Set `TodoApp.Console` as the startup project and run (right-click).
2. At the top menu, select `Debug > Start Without Debugging` (or press `Ctrl + F5`).

- From **Rider**:
1. Right-click `TodoApp.Console` and select `Run 'TodoApp.Console'`.


4) **Run the API (optional)**
   ```bash
   cd ../TodoApp.Api
   dotnet run

Open the URL shown in the console to see Swagger

- From **Visual Studio**:
1. Set `TodoApp.Api` as the startup project and run (right-click).
2. At the top menu, select `Debug > Start Without Debugging` (or press `Ctrl + F5`).

- From **Rider**:
1. Right-click `TodoApp.Api` and select `Run 'TodoApp.Api'`.

## Acceptance Criteria

- Console app launches without errors.
- (Optional) API launches and shows Swagger UI.

C## ommit message suggestion

`chore: verify local dev setup and run console/api`

## PR checklist (student)

- I can run the console app.
- I read the folder structure notes.
- I pushed a small “bootstrap” commit.