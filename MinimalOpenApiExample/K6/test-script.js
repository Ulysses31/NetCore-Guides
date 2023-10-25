import http from "k6/http";
import { sleep, check } from "k6";

export const options = {
  vus: 10,
  iterations: 10,
};

export default function () {
  const params = {
    cookies: {},
    headers: { "x-api-version": "1.0" },
  };

  const res = http.get(
    "https://localhost:7000/api/orders/1?x-api-version=1.0",
    params
  );

  check(res, {
    "is status 200": (r) => r.status === 200,
    "is status 429": (r) => r.status === 429,
  });

  sleep(1);
}
