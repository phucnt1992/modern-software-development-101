# import pytest
from fastapi.testclient import TestClient
from app.main import app

client = TestClient(app)

def test_healthcheck_endpoint_should_response_ok():
    # Act
    response = client.get("/api/_healthz/readiness/")

    # Assert
    assert response.status_code == 200
    assert response.json() == {"status": "ok"}
