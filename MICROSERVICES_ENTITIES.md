# Microservices Entities

This document lists the entities managed by each microservice in the BioTech-Backend project.

## AuthService
Responsible for authentication, authorization, and tenant management.
- **Farm**: Represents a farm unit.
- **Permission**: Granular permissions assignable to roles.
- **Role**: User roles (e.g., Admin, User).
- **RolePermission**: Many-to-many relationship between Roles and Permissions.
- **Tenant**: Represents a tenant in the multi-tenant system.
- **User**: System users.
- **UserFarmRole**: Association between Users, Farms, and Roles.

## CommercialService
Handles commercial transactions and financial interactions.
- **CommercialTransaction**: Represents a sale or purchase transaction.
- **ThirdParty**: External entities involved in transactions (suppliers, buyers).
- **TransactionAnimalDetail**: Details of animals involved in a transaction.
- **TransactionProductDetail**: Details of products involved in a transaction.

## FeedingService
Manages feeding operations for the herd.
- **FeedingEvent**: Records a feeding event for animals or batches.

## HealthService
Tracks animal health, diseases, and treatments.
- **Disease**: Catalog of animal diseases.
- **HealthEvent**: Records a health-related event (e.g., vaccination, sickness).
- **HealthEventDetail**: Detailed information for a health event.
- **WithdrawalPeriod**: Tracking withdrawal periods for medications.

## HerdService
Core service for managing the animal population.
- **Animal**: Represents individual animals in the herd.
- **AnimalCategory**: Categories of animals (e.g., calf, heifer).
- **AnimalMovement**: Tracks movement of animals between paddocks or farms.
- **Batch**: Groups of animals managed together.
- **Breed**: Animal breeds.
- **MovementType**: Types of animal movements (e.g., sale, death, transfer).
- **Paddock**: Physical locations or enclosures for animals.

## InventoryService
Manages stock of products and supplies.
- **InventoryItem**: Represents an item in the inventory.
- **InventoryMovement**: Tracks changes in inventory stock.
- **Product**: Catalog of products available.

## ReproductionService
Handles reproduction cycles and events.
- **ReproductionEvent**: Records reproduction-related events (e.g., insemination, birth).

## AIService
- *No specific domain entities identified in the Domain layer.*

## ApiGateway
- Acts as a reverse proxy and aggregator; does not manage its own domain entities.
