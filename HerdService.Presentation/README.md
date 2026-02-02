# Herd Service

## Dependencies
This service does not explicitly call other microservices via HTTP. It acts as a core data provider for:
- InventoryService
- AIService

## Configuration
This service listens on port `5048` by default in local development.
