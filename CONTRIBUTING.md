# Contributing to DeutschQuiz

This repo uses **GitHub Flow** (best fit for a small team): one long-lived branch, short-lived feature branches, and pull requests.

## Branches

| Branch | Role |
|--------|------|
| `master` | **Default.** Always deployable. All work merges here via PR. |
| `frontend/*`, `backend/*`, `feature/*` | Short-lived work branches. Delete after merge. |

There is no `develop` branch. Do not keep a second long-lived integration branch.

```text
master  →  frontend/short-description  →  PR  →  master
```

**Releases:** tag `master` when you publish (for example `v1.0.0`). You do not need a separate release branch for this project.

## Getting started

```bash
git clone https://github.com/AliMohammadian76/DeutschQuiz.git
cd DeutschQuiz
git checkout master
git pull origin master
```

Run the app using [README.md](README.md) (`backend/` and `frontend/`).

## How to work

1. Update `master` and create a branch:

   ```bash
   git checkout master
   git pull origin master
   git checkout -b frontend/short-description
   ```

2. Push the branch and open a **pull request into `master`**.

3. After merge, delete the feature branch and start the next task from an updated `master`.

Direct pushes to `master` are blocked by branch protection; always use a PR.

## Who works where

- Frontend: primarily `frontend/`
- Backend: primarily `backend/`

Cross-cutting changes (API contracts, README, Docker) are fine either side; mention them in the PR description.

## Inviting collaborators (maintainers)

GitHub → **Settings → Collaborators** → Add people → **Admin** (or Write).

Teammates should:

1. Accept the invite
2. Work on feature branches off `master`
3. Open PRs into `master` (never push straight to `master`)
