# Phase 0 — Bootstrap & Run

**Goal:** Get the solution running and understand the structure.

## What you’ll learn
- Solution layout (Domain, Application, Infrastructure, Console, API, Tests)
- How to run the Console and API projects

## Prereqs
- .NET SDK installed (same as repo)
- Git + an editor (VS 2022 Community Edition / Rider / VS Code)
- Make sure you fetch the feature branches (see Git-101.md)

## Steps

1) ### Change Branch
Change to the  `Phase_0-Bootstrap_read_run_the_repo` branch:
```bash
git checkout Phase_0-Bootstrap_read_run_the_repo
```

2) ### Open the Solution
	From Visual Studio or Rider, open `MyTodo.sln`

3) ### Explore the structure
   - `src/TodoApp.Domain` – entities and contracts
   - `src/TodoApp.Application` – use cases / services
   - `src/TodoApp.Infrastructure` – data access, in-memory store (later)
   - `src/TodoApp.Console` – console UI
   - `src/TodoApp.Api` – Minimal API (optional for this phase)
   - `tests/` – unit tests (later)

4) ### Run the Console app
- **From terminal**:
   ```bash
   cd src/TodoApp.Console
   dotnet run
   ```

- **From Visual Studio**:
1. Set `TodoApp.Console` as the startup project and run (right-click).
2. At the top menu, select `Debug > Start Without Debugging` (or press `Ctrl + F5`).

- **From Rider**:
1. Right-click `TodoApp.Console` and select `Run 'TodoApp.Console'`.


5) ### (OPTIONAL) Run the API

   ```bash
   cd ../TodoApp.Api
   dotnet run
   ```

Open the URL shown in the console to see Swagger

- From **Visual Studio**:
1. Set `TodoApp.Api` as the startup project and run (right-click).
2. At the top menu, select `Debug > Start Without Debugging` (or press `Ctrl + F5`).

- From **Rider**:
1. Right-click `TodoApp.Api` and select `Run 'TodoApp.Api'`.

6) ### Commit dummy changes
   - Add a comment in `Program.cs` at the top like `// Phase 0 setup complete`
   - Commit and push by following these commands:
   ```bash
   git add .
   git commit -m "Phase_0-Bootstrap_read_run_the_repo - FirstName LastName"
   git push --set-upstream origin Phase_0-Bootstrap_read_run_the_repo
   ```

## Acceptance Criteria

- Console app launches without errors.
- (Optional) API launches and shows Swagger UI.

## PR checklist (student)

- I can run the console app.
- I read the folder structure notes.
- I pushed a small “bootstrap” commit.