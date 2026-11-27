---
name: change-nbi-syntax
description: Add or change NBi XML test-suite elements, attributes, and defaults or GenbiL recipe commands. Use for public syntax changes involving schema, serialization, parsing, or execution.
---

# Change NBi syntax

## Define the contract

Find the closest existing element, attribute, or command and its tests. Identify what callers write, what it means, and which existing forms and defaults need to keep working. Cover an omitted value, explicit default, or invalid form when that distinction matters. Use the target branch as the source of truth for runtime behavior.

## Trace the public entry point

- **XML test suites:** Check the corresponding XSD under [NBi.Xml/Schema](../../../NBi.Xml/Schema), the XML model and serialization attributes under [NBi.Xml](../../../NBi.Xml), and the path from `XmlManager` to the component that consumes the model. Update the schema, model, and runtime where the contract changes. Add or update a representative fixture read through the normal XML load path. For schema changes, validate the representative XML against the schema as well as checking the deserialized values.
- **GenbiL recipes:** Follow the closest command in [NBi.genbiL/Parser](../../../NBi.genbiL/Parser) into [NBi.genbiL/Action](../../../NBi.genbiL/Action). Ensure the command is reachable through the recipe grammar and produces the intended action and state change. Add focused parser and action tests in [NBi.Testing.GenbiL](../../../NBi.Testing.GenbiL) for the behavior that changed.
- If a recipe generates or consumes the changed XML, check the handoff at both entry points. Extend the smallest existing fixture that can verify the full path.

## Validate

Choose the relevant fixture in [NBi.Testing.Xml](../../../NBi.Testing.Xml), [NBi.Testing.GenbiL](../../../NBi.Testing.GenbiL), or the runtime test project. For example, the XML profile fixture can be run with:

```powershell
dotnet test NBi.Testing.Xml/NBi.Xml.Testing.csproj -c Release --filter 'FullyQualifiedName~CsvProfileXmlTest'
```

Update user-facing examples when the syntax or defaults change. The documentation site is maintained on the `gh-pages` branch. Follow [AGENTS.md](../../../AGENTS.md) for the repository workflow.
