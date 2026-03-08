# Functional Seeding Scripts

This folder is isolated from the API source code and is intended for functional/manual checks and future load-test support.

The script already respects current business constraints:
- `valor_solicitado` is always greater than `100.00`
- creators are generated uniquely per run, avoiding multiple pending requests for the same creator

## Seed 100 random Anticipations

```bash
python tools/functional-seeding/seed_anticipations.py
```

## Options

```bash
python tools/functional-seeding/seed_anticipations.py --count 100 --base-url http://localhost:8080 --sleep-ms 20
```

- `--count`: number of records to insert
- `--base-url`: API base URL
- `--timeout`: timeout per request (seconds)
- `--sleep-ms`: wait between requests (milliseconds)
