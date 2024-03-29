# Snippets / Libs / Quicktype

This is in reference to the CLI for https://app.quicktype.io/. Install using `npm install -g quicktype`.

---

## Examples

```js
quicktype "input.json" -o "Output.cs" -l cs -s json --no-maps --no-enums --no-uuids --no-date-times --no-combine-classes --alphabetize-properties
```

---

## Help Document

- `-o, --out`
    - **Accepts:** FILE
    - **Description:** The output file. Determines --lang and --top-level.
- `-t, --top-level`
    - **Accepts:** NAME
    - **Description:** The name for the top level type.
- `-l, --lang`
    - **Accepts:** cs|go|rs|cr|cjson|c++|objc|java|ts|js|javascript-prop-types|flow|swift|scala3|Smithy|kotlin|elm|schema|ruby|dart|py|pike|haskell|typescript-zod|php
    - **Description:** The target language.
- `-s, --src-lang`
    - **Accepts:** json|schema|graphql|postman|typescript
    - **Description:** The source language (default is json).
- `--src`
    - **Accepts:** FILE|URL|DIRECTORY
    - **Description:** The file, url, or data directory to type.
- `--src-urls`
    - **Accepts:** FILE
    - **Description:** Tracery grammar describing URLs to crawl.
- `--no-maps`
    - **Description:** Don't infer maps, always use classes.
- `--no-enums`
    - **Description:** Don't infer enums, always use strings.
- `--no-uuids`
    - **Description:** Don't convert UUIDs to UUID objects.
- `--no-date-times`
    - **Description:** Don't infer dates or times.
- `--no-integer-strings`
    - **Description:** Don't convert stringified integers to integers.
- `--no-boolean-strings`
    - **Description:** Don't convert stringified booleans to booleans.
- `--no-combine-classes`
    - **Description:** Don't combine similar classes.
- `--no-ignore-json-refs`
    - **Description:** Treat $ref as a reference in JSON.
- `--graphql-schema`
    - **Accepts:** FILE
    - **Description:** GraphQL introspection file.
- `--graphql-introspect`
    - **Accepts:** URL
    - **Description:** Introspect GraphQL schema from a server.
- `--http-method`
    - **Accepts:** METHOD
    - **Description:** HTTP method to use for the GraphQL introspection query.
- `--http-header`
    - **Accepts:** HEADER
    - **Description:** Header(s) to attach to all HTTP requests, including the GraphQL introspection query.
- `-S, --additional-schema`
    - **Accepts:** FILE
    - **Description:** Register the $id's of additional JSON Schema files.
- `--alphabetize-properties`
    - **Description:** Alphabetize order of class properties.
- `--all-properties-optional`
    - **Description:** Make all class properties optional.
- `--quiet`
    - **Description:** Don't show issues in the generated code.
- `--debug`
    - **Accepts:** OPTIONS or all
    - **Description:** Comma separated debug options: print-graph, print-reconstitution, print-gather-names, print-transformations, print-schema-resolving, print-times, provenance.
- `--telemetry`
    - **Accepts:** enable|disable
    - **Description:** Enable anonymous telemetry to help improve quicktype.
- `-h, --help`
    - **Description:** Get some help.
- `-v, --version`
    - **Description:** Display the version of quicktype.
