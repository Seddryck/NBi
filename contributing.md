# Contributing to NBi

Please follow the [Code of Conduct](CODE_OF_CONDUCT.md).

## Report a bug or request a feature

Search the [NBi issues](https://github.com/Seddryck/NBi/issues) for an existing report. Use a descriptive title. For a bug, include the NBi version, the behavior you expected, the behavior you observed, and a minimal example. Share a text example of the `.nbits` file or GenbiL recipe when it helps reproduce the problem. Remove credentials and private data. For a feature request, describe the use case and proposed syntax if applicable.

## Make and validate a change

Create a topic branch from `main` unless the change is intended for a specific release branch. See [AGENTS.md](AGENTS.md) for branch, worktree, and validation conventions.

Run the focused tests for the area you changed. If the change affects public XML or GenbiL syntax, include a representative example and update the relevant tests and documentation. Legacy acceptance and integration tests may require external services.

## Git hooks

Install the .NET SDK selected by [global.json](global.json), Git, and Node.js. To enable the hooks for the checkout, run:

```text
npm install --global @commitlint/cli @commitlint/config-conventional
git config --local core.hooksPath .githooks
dotnet restore NBi.sln
```

The commit hook checks Conventional Commit messages. Before pushing, the analysis hook checks the C# files changed by the push. GitHub Actions runs the same check. To run it before pushing a branch, use Git Bash on Windows or Bash on other platforms:

```bash
bash .githooks/check-code-analysis "$(git merge-base origin/main HEAD)" HEAD
```

If you already use pre-commit, run `pre-commit run csharp-stylecop` to check staged C# files before committing.

## Submit a pull request

Open a pull request against [Seddryck/NBi](https://github.com/Seddryck/NBi) with a Conventional Commit title, a short description, and the validation results. Link the relevant issue if one exists. Documentation fixes can be submitted directly. Check the pull request build in [GitHub Actions](https://github.com/Seddryck/NBi/actions/workflows/ci.yml).
