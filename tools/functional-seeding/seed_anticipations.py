#!/usr/bin/env python3
"""Seed Anticipation API with random records for functional checks.

Usage examples:
  python tools/functional-seeding/seed_anticipations.py
  python tools/functional-seeding/seed_anticipations.py --count 100 --base-url http://localhost:8080
"""

from __future__ import annotations

import argparse
import json
import random
import sys
import time
import urllib.error
import urllib.request
import uuid
from datetime import UTC, datetime, timedelta


def build_payload(run_prefix: str) -> dict[str, object]:
    creator_number = random.randint(1, 999_999)
    creator_id = f"user-{run_prefix}-{creator_number:06d}"

    requested_value = round(random.uniform(100.01, 15000.0), 2)

    days_ago = random.randint(0, 30)
    seconds_ago = random.randint(0, 24 * 60 * 60)
    request_date = datetime.now(UTC) - timedelta(days=days_ago, seconds=seconds_ago)

    return {
        "creator_id": creator_id,
        "valor_solicitado": requested_value,
        "data_solicitacao": request_date.isoformat().replace("+00:00", "Z"),
    }


def post_anticipation(url: str, payload: dict[str, object], timeout: float) -> tuple[bool, str]:
    data = json.dumps(payload).encode("utf-8")
    request = urllib.request.Request(url=url, data=data, method="POST")
    request.add_header("Content-Type", "application/json")

    try:
        with urllib.request.urlopen(request, timeout=timeout) as response:
            body = response.read().decode("utf-8")
            return True, f"HTTP {response.status} | {body[:120]}"
    except urllib.error.HTTPError as http_error:
        body = http_error.read().decode("utf-8", errors="ignore")
        return False, f"HTTP {http_error.code} | {body[:180]}"
    except urllib.error.URLError as url_error:
        return False, f"URL error: {url_error.reason}"
    except TimeoutError:
        return False, "Timeout"


def parse_args() -> argparse.Namespace:
    parser = argparse.ArgumentParser(description="Seed Anticipation API with random records.")
    parser.add_argument("--base-url", default="http://localhost:8080", help="API base URL")
    parser.add_argument("--count", type=int, default=100, help="How many records to create")
    parser.add_argument("--timeout", type=float, default=10.0, help="Request timeout in seconds")
    parser.add_argument("--sleep-ms", type=int, default=0, help="Delay between requests in milliseconds")
    return parser.parse_args()


def main() -> int:
    args = parse_args()

    if args.count < 1:
        print("count must be >= 1")
        return 1

    endpoint = args.base_url.rstrip("/") + "/api/v1/anticipations"

    success_count = 0
    failure_count = 0

    print(f"Seeding {args.count} records to {endpoint}")

    run_prefix = uuid.uuid4().hex[:8]

    for index in range(1, args.count + 1):
        payload = build_payload(run_prefix)
        ok, message = post_anticipation(endpoint, payload, args.timeout)

        if ok:
            success_count += 1
            print(f"[{index:03d}/{args.count}] OK - {message}")
        else:
            failure_count += 1
            print(f"[{index:03d}/{args.count}] FAIL - {message}")

        if args.sleep_ms > 0:
            time.sleep(args.sleep_ms / 1000)

    print("\nSummary")
    print(f"success: {success_count}")
    print(f"failed:  {failure_count}")

    return 0 if failure_count == 0 else 2


if __name__ == "__main__":
    sys.exit(main())
