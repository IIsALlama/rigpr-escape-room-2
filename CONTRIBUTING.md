## Naming Conventions

Use the Unity naming conventions.

https://unity.com/how-to/naming-and-code-style-tips-c-scripting-unity

## Version Control

### Commit Messages

We can move Jira tickets forwards when pushing to the GitHub. To do so, simply prefix the commit message with the Jira ticket ID. For example:

```
ESC-123 Feat(Scripts): Added Script.cs
```

To ensure that commit messages are clear and concise, please remember to structure them in the format below.

```
<JIRA-TICKET-ID> <type>(<scope>): <short summary>
```

Example scope:
- Assets/Materials
- Scripts

| Type       | Description                                                                |
| ---------- | -------------------------------------------------------------------------- |
| `feat`     | A new feature (e.g., adding a new system or gameplay mechanic)             |
| `fix`      | A bug fix                                                                  |
| `refactor` | Code refactoring that doesn’t change behavior (e.g., structure or cleanup) |
| `chore`    | Non-code changes (e.g., updating `.editorconfig`, README, or metadata)     |
| `docs`     | Documentation-only changes                                                 |
| `style`    | Code style or formatting changes (no logic changes)                        |
| `test`     | Adding or updating tests                                                   |
| `perf`     | Performance improvements                                                   |
| `build`    | Changes to build scripts or dependencies                                   |
