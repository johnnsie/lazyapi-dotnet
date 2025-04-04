# LazyAPI – Intelligent Parsing API with .NET + Ollama (Mistral)

**LazyAPI** is a lightweight `.NET` backend that leverages a local **Ollama (Mistral)** LLM model to extract structured user data (`firstName`, `lastName`, `birthdate`) from loosely structured or messy input. It's designed to make APIs more flexible, especially when dealing with inconsistent or poorly formatted user input.

---

## What It Does

- Accepts flexible, messy, or freeform input via `POST /api/parse`
- Sends the input as a prompt to a **local Ollama model** (e.g., `prod-brain4`)
- Parses the model’s response and returns clean JSON with:
  - `firstName`
  - `lastName`
  - `birthdate` (in `dd/MM/yyyy` format)
- Fields are set to `null` if not confidently found

---

## Tech Stack

- [.NET 9](https://dotnet.microsoft.com/)
- [Ollama](https://ollama.com/) running a [Mistral](https://mistral.ai/) model
- Custom `Modelfile` for targeted prompting
- `HttpClientFactory` for clean LLM integration
- JSON parsing with error handling and response sanitization

---

## Project Structure

- `Controllers/ParseController.cs` — Main API endpoint for POST input parsing
- `Models/OllamaRequest.cs` — Data contract sent to the LLM
- `Models/NameInfo.cs` — Final parsed result object
- `Helpers/StringHelpers.cs` — Utility to clean backslashes from model responses

---

## Custom Modelfile

This project uses a custom Ollama `Modelfile` to guide the model behavior:

```Dockerfile
FROM mistral

SYSTEM """
You are a .NET api orchestrator.
You have access to an api and you need to determine 3 values: 'firstname', 'lastname', and 'birthdate'.
Return them in this JSON format:

{
  "firstName": "user first name",
  "lastName": "user last name",
  "birthdate": "found birthdate"
}

- Use 'null' for missing or unclear values.
- Handle swapped fields, mixed formats, or partial data.
- Birthdate must be in dd/MM/YYYY format.
"""

```

## API Usage

### Example Request
POST /api/parse Content-Type: application/json

```json
{
  "data": "Hi, I'm Jane Doe, born July 5th, 1990. Please sign me up!"
}
```


### Example Response
```json
{
  "firstName": "Jane",
  "lastName": "Doe",
  "birthdate": "05/07/1990"
}
```

### Getting Started

```bash

git clone https://github.com/your-username/lazyapi-dotnet.git
cd lazyapi-dotnet

ollama run prod-brain4

dotnet build
dotnet run

curl -X POST http://localhost:5000/api/parse \
-H "Content-Type: application/json" \
-d '{"data": "Name: Alex Smith, DOB: 22 Jan 1985"}'

```

## Frontend Companion

Pair this backend with a minimalist React + Vite + Tailwind frontend:

### lazyapi-frontend

[LazyApi-frontend](https://github.com/your-username/lazyapi-react)
