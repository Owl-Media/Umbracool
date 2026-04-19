# Umbracool

[![CI](https://github.com/OWNER/REPO/actions/workflows/ci.yml/badge.svg)](https://github.com/OWNER/REPO/actions/workflows/ci.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

Umbracool is a GitHub starter repository for deploying an empty [Umbraco 17 LTS](https://umbraco.com/products/umbraco-cms/umbraco-17/) site to [Coolify](https://coolify.io/) on Linux with Docker Compose.

It ships with:

- An empty Umbraco app under `src/Umbracool.Web`
- A production multi-stage `Dockerfile`
- A `docker-compose.coolify.yml` stack with Umbraco plus SQL Server
- Unattended Umbraco install driven entirely by environment variables
- Persistent storage for SQL Server data and Umbraco media uploads

This starter intentionally targets SQL Server. Current Umbraco documentation supports SQLite and SQL Server, not PostgreSQL.

Replace `OWNER/REPO` in the CI badge URLs above with your GitHub repository path after the first push.

## Repository Layout

- `src/Umbracool.Web` - the Umbraco web project
- `Dockerfile` - the production build and runtime image for the web app
- `docker-compose.coolify.yml` - the Coolify deployment stack
- `database/` - SQL Server bootstrap and healthcheck scripts
- `.env.example` - the required environment variables for unattended install

## Required Environment Variables

Copy `.env.example` to `.env` for local testing, or add the same keys in Coolify:

```env
SQL_SA_PASSWORD=ChangeThisSqlPassword123!
UMBRACO_DB_NAME=UmbracoDb
UMBRACO_SITE_NAME=Umbracool
UMBRACO_ADMIN_NAME=Admin User
UMBRACO_ADMIN_EMAIL=admin@example.com
UMBRACO_ADMIN_PASSWORD=ChangeThisAdminPassword123!
UMBRACO_TELEMETRY_LEVEL=Basic
```

Notes:

- `SQL_SA_PASSWORD` must satisfy SQL Server password requirements.
- `UMBRACO_ADMIN_PASSWORD` must satisfy Umbraco password policy requirements.
- `UMBRACO_DB_NAME` should stay to simple SQL identifier-style characters.

## Deploy To Coolify

1. Push this repository to GitHub.
2. In Coolify, create a new application from that Git repository.
3. Choose the `Docker Compose` build pack.
4. Point Coolify at `docker-compose.coolify.yml`.
5. Add the environment variables from `.env.example`.
6. Assign a domain to the `web` service in Coolify and include port `8080` in the domain entry, for example `https://cms.example.com:8080`.
7. Deploy the stack.

What happens on first boot:

- Coolify builds the web image from the root `Dockerfile`.
- The SQL Server container starts and creates the target database if it does not exist.
- The Umbraco container waits for SQL Server to become healthy.
- Umbraco completes the unattended install using the environment variables you supplied.
- The backoffice becomes available at `/umbraco`.

## Persistence

The stack uses named Docker volumes for:

- `sql_data` - SQL Server data files
- `umbraco_media` - uploaded media under `wwwroot/media`

This means container rebuilds should not wipe the database or media library.

## Local Run Notes

This repository is optimized for Coolify, but you can still smoke test it locally:

```powershell
Copy-Item .env.example .env
docker compose -f docker-compose.coolify.yml up --build
```

If you want to inspect the web container locally, expose or map port `8080` in `docker-compose.coolify.yml` temporarily for your machine.

## Contribution Workflow

This repository now includes a small GitHub maintainer layer to help keep changes safe and reviewable:

- Issue templates for bugs and feature requests
- A pull request template with validation reminders
- A CI workflow that builds the solution, runs repo-smoke tests, renders the Docker Compose config, and builds the web image

Local checks that mirror CI:

```powershell
dotnet test
dotnet build Umbracool.sln -c Release
docker compose -f docker-compose.coolify.yml config
docker build -f Dockerfile .
```

## Template Repo Suggestion

This repository is meant to work well as a GitHub template repository so other people can click **Use this template** and start from a known-good Coolify deployment base.

## References

- [Umbraco project templates](https://docs.umbraco.com/umbraco-cms/17.latest/fundamentals/setup/install/install-umbraco-with-templates)
- [Umbraco unattended installs](https://docs.umbraco.com/umbraco-cms/fundamentals/setup/install/unattended-install)
- [Umbraco configuration reference](https://docs.umbraco.com/umbraco-cms/reference/configuration)
- [Coolify Docker Compose build pack](https://coolify.io/docs/applications/build-packs/docker-compose)
- [Coolify Docker Compose behavior](https://coolify.io/docs/knowledge-base/docker/compose)
