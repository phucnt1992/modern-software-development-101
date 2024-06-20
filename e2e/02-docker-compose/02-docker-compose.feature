Feature: Docker Compose serve upload file API

  Scenario: Request to Liveness HealthCheck API should return Healthy state.
    Given I started the Docker application with "docker run docker-app:latest -p 8000:80"
    When I request to 'http://127.0.0.1:8000/api/_healthz/liveness'
    Then I should receive a response with status 200
    And I should receive a response with body 'OK'

  Scenario: Request to Readiness HealthCheck API should return Healthy state.
    Given I started the Docker application with "docker run docker-app:latest -p 8000:80"
    When I request to 'http://127.0.0.1:8000/api/_healthz/readiness'
    Then I should receive a response with status 200
    And I should receive a response with body 'OK'
