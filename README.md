# Stremio Top 10 Addon

A Stremio addon that surfaces Top 10 movies and series from Netflix, Amazon Prime, Disney+, Apple TV+, and HBO Max.

## About

A worker service fetches top 10 lists from the [Streaming Availability API (RapidAPI)](https://rapidapi.com/movie-of-the-night-movie-of-the-night-default/api/streaming-availability) daily and persists them as JSON files. The web API reads those files and serves them to Stremio clients following the [Stremio addon spec](https://github.com/Stremio/stremio-addon-sdk).

## Features

- Top 10 movies and series for 5 providers: Netflix, Prime, Disney+, Apple TV+, HBO Max
- Automatically refreshed daily (08:00 AM Vienna time) via a scheduled job
- Self-hosted — full control over your data

## Tech Stack

- .NET 8, ASP.NET Core (Web API)
- .NET 8 Worker Service + Quartz.NET (scheduler)
- Docker / Docker Compose
- RapidAPI Streaming Availability API

## Prerequisites

- Docker & Docker Compose
- A RapidAPI account with access to the [Streaming Availability API](https://rapidapi.com/movie-of-the-night-movie-of-the-night-default/api/streaming-availability)

## Getting Started

### Clone

```
git clone <repo-url>
cd stremio-addon-top10
```

### Configure environment

Copy `.env.example` to `.env` and fill in values:

```
RAPID_API_KEY=<your_rapidapi_key>
STREMIO_DATA=/stremioData
```

> `STREMIO_DATA` is the path **inside the container** where JSON files are stored. The default `/stremioData` matches the compose.yaml volume mount — no change needed unless customizing.

### Run with Docker Compose

```
docker compose up -d
```

This starts:
- `stremio` — the addon web API on port **420**
- `worker` — the background fetcher (no external port)

On first start the worker runs immediately, then daily at 08:00 AM (Europe/Vienna).

## Adding to Stremio

1. Open Stremio → **Settings** → **Addons**
2. Paste the following URL into the Stremio addon search bar:
   ```
   http://<your-host>:420/manifest.json
   ```
3. The addon "Top 10" will appear with catalogs for each provider.

## Project Structure

```
stremio-addon-top10/
├── StremioAddon/          # ASP.NET Core Web API (serves catalog to Stremio)
│   ├── Controllers/       # ManifestController, CatalogController
│   ├── Models/            # Manifest, Meta, CatalogModel, Stream
│   └── Dockerfile
├── ResourceFetcher/       # .NET Worker Service (fetches & persists top 10 data)
│   ├── CronJobs/          # Quartz job triggered on startup + daily at 08:00
│   ├── Services/          # Per-provider fetch services + orchestrator
│   ├── Clients/           # HTTP client with RapidAPI auth
│   ├── Models/Adapters/   # Converts API response → Stremio Meta format
│   └── Dockerfile
├── UnitTests/             # xUnit tests
├── compose.yaml           # Docker Compose (dev/prod)
├── .env.example           # Environment variable template
└── stremioData/           # Volume mount — persisted JSON catalog files
```

## Environment Variables

| Variable | Required | Description |
|---|---|---|
| `RAPID_API_KEY` | Yes | RapidAPI key for Streaming Availability API |
| `STREMIO_DATA` | Yes | Path inside containers where JSON files are read/written |

## Development (without Docker)

- Fill `.env` as above, set `STREMIO_DATA` to a local path (e.g. `./stremioData`)
- Run `ResourceFetcher` first to populate the data directory
- Then run `StremioAddon` — it reads from `STREMIO_DATA`
- Both projects use `dotenv.net` and load `.env` automatically
