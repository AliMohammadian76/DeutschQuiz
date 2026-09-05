# Contributing to DeutschQuiz

## Branches

| Branch | Role |
|--------|------|
| `develop` | **Default.** Day-to-day work, previews, and integration. Push / open PRs here. |
| `master` | **Launch / publish only.** Stable release. Do not push feature work here. |
| `frontend/*`, `backend/*`, `feature/*` | Optional short-lived work branches. |

```text
feature branch (optional)  →  develop  →  PR when ready to launch  →  master
```

## Getting started

```bash
git clone https://github.com/AliMohammadian76/DeutschQuiz.git
cd DeutschQuiz
git checkout develop
git pull origin develop
```

Run the app using [README.md](README.md) (`backend/` and `frontend/`).

## How to work

1. Work on `develop` (or a branch created from `develop`):

   ```bash
   git checkout develop
   git pull origin develop
   git checkout -b frontend/short-description   # optional
   ```

2. Push to `develop` (or open a PR into `develop`).

3. **Launch / publish:** when the app is ready to release, open a PR from `develop` → `master`.  
   Direct pushes to `master` are blocked by branch protection.

## Who works where

- Frontend: primarily `frontend/`
- Backend: primarily `backend/`

Cross-cutting changes (API contracts, README, Docker) are fine either side; mention them in the PR description.

## Inviting collaborators (maintainers)

GitHub → **Settings → Collaborators** → Add people → **Admin** (or Write).

Teammates should:

1. Accept the invite
2. Clone and use **`develop`** as the main branch
3. Never push launch work straight to `master` — use a PR `develop` → `master`
