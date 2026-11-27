# NBi repository guidance

## Branches and worktrees

For a new task, fetch the remote and create a dedicated worktree and branch from the latest `origin/main`. Use a release branch when the change targets that release. For local checkouts, create worktrees with `git worktree add` under `.worktrees/<task-name>` and exclude that directory through `.git/info/exclude`.

Choose a descriptive branch name. Use `fix/<name>` for defects and regressions, `feat/<name>` for new behavior, `docs/<name>` for documentation, or another Conventional Commit type that describes the change.

## Issues and pull requests

- Use natural-language issue titles. When managing existing issues, use the repository's labels, such as `bug`, `feature-request`, `small-improvement`, and `documentation`.
- Use Conventional Commit titles for commits and pull requests. See [the commitlint configuration](.commitlint.yaml) for allowed types and length limits. The optional [commit-msg hook](.commitlint/hooks/commit-msg) requires commitlint on PATH.
- Follow [the contribution guide](contributing.md) when submitting a change. Use `gh` to manage GitHub issues and pull requests. Link the corresponding issue when one exists and include the validation performed.

## Projects and validation

- Read [Directory.Build.props](Directory.Build.props) on the target branch for its SDK-style project frameworks.
- Use `NBi.sln` for the SDK-style build. `NBi.Testing.Core`, `NBi.Testing.Xml`, `NBi.Testing.GenbiL`, and `NBi.Testing.Framework` are its NUnit test projects. The legacy acceptance and integration project under `NBi.Testing` has separate runner and service requirements.
- Run the closest relevant tests first. Run a solution build when changes span projects or affect build configuration. Report the tests run and any validation skipped.
- Keep local connection settings and credentials outside commits and command output.

## Repository skill

Read [change-nbi-syntax](.github/skills/change-nbi-syntax/SKILL.md) for changes to the public XML test-suite format or GenbiL recipe syntax.
