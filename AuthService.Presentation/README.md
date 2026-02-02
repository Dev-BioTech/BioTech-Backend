# Auth Service

## Dependencies
This service is the central identity provider and does not depend on other business services.

## Consumers
All other services depend on this service for JWT validation (though validation is usually local via key, token issuance is here).

## Configuration
This service listens on port `5237` by default in local development.
