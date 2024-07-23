import { createBdd } from 'playwright-bdd';
import { APIRequestContext, APIResponse, expect } from '@playwright/test';
import { test } from '../fixtures';

const { Given, When, Then, After } = createBdd(test);

let apiContext:APIRequestContext;
let response: APIResponse;

After(async () => {
  apiContext?.dispose();
  response?.dispose();
});

Given('I started the Docker application with "docker run docker-app:latest -p {int}:80"', async ({ playwright }, port:number) => {
  apiContext = await playwright.request.newContext({
    baseURL: `http://127.0.0.1:${port}`
  });
})

When('I request to {string}', async ({}, endpoint: string) => {
  response = await apiContext.get(endpoint);
});

Then('I should receive a response with status {int}', async ({}, expectedStatus: number) => {
  expect(response.status()).toBe(expectedStatus);
});

Then('I should receive a response with body {string}', async ({}, expectedBody: string) => {
  const actualBody = await response.text();
  expect(actualBody).toBe(expectedBody);
});
