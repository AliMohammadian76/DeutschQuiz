# Contributing to DeutschQuiz

## Branches

| Branch | Role |
|--------|------|
| `develop` | **Default.** Day-to-day work and integration. |
| `master` | Published / release only. Do not push feature work here. |

```text
feature/* or frontend/*  →  PR →  develop  →  PR (when ready) →  master  →  publish
```

## Getting started

1. Clone the repo (default branch on GitHub is `develop`):

   ```bash
   git clone https://github.com/AliMohammadian76/DeutschQuiz.git
   cd DeutschQuiz
   ```

2. Confirm you are on `develop`:

   ```bash
   git checkout develop
   git pull origin develop
   ```

3. Run the app using the steps in [README.md](README.md) (`backend/` and `frontend/`).

## How to work

1. Create a branch from `develop`:

   ```bash
   git checkout develop
   git pull origin develop
   git checkout -b frontend/short-description
   ```

   Prefer prefixes like `frontend/…`, `backend/…`, or `feature/…`.

2. Commit and push your branch, then open a **pull request into `develop`**.

3. After review/merge on `develop`, continue the next task from an updated `develop`.

4. **Release / publish:** open a PR from `develop` → `master` only when you want a stable published version. Direct pushes to `master` should be blocked by branch protection.

## Who works where

- Frontend: primarily `frontend/`
- Backend: primarily `backend/`

Cross-cutting changes (API contracts, README, Docker) are fine on either side; call them out in the PR description.

## Inviting collaborators (maintainers)

GitHub → **Settings → Collaborators** → Add people → choose **Admin** (or Write if you prefer stricter access).

Share with new teammates:

1. Accept the GitHub invite email.
2. Clone and work from `develop` (or branches off it).
3. Never push release work straight to `master`.

## GitHub settings checklist (one-time)

If these are not already configured:

1. **Default branch:** Settings → General → Default branch → `develop`
2. **Protect `master`:** Settings → Branches → Add rule for `master`
   - Enable **Require a pull request before merging**
   - Do not allow direct pushes to `master`
