# 📡 BioTech-Backend — Guía Completa de Endpoints

> **Base URL (Gateway):** `http://localhost:5000`  
> **Autenticación:** JWT Bearer Token (salvo login y register)  
> **Header requerido en servicios autenticados:** `Authorization: Bearer <token>`

---

## 1. 🔐 AuthService — Autenticación y Fincas

### 1.1 Auth Controller
**Base:** `/api/v1/auth`

---

#### `POST /api/v1/auth/login`
Iniciar sesión y obtener token JWT.

**Auth:** ❌ No requiere  
**Request Body:**
```json
{
    "email": "usuario@email.com",
    "password": "contraseña123"
}
```
**Response (200):**
```json
{
    "success": true,
    "message": "Login successful",
    "data": {
        "token": "eyJhbGciOi...",
        "userId": 1,
        "fullName": "Juan Pérez",
        "email": "usuario@email.com"
    }
}
```

---

#### `POST /api/v1/auth/register`
Registrar un nuevo usuario.

**Auth:** ❌ No requiere  
**Request Body:**
```json
{
    "fullName": "Juan Pérez",
    "email": "usuario@email.com",
    "password": "contraseña123"
}
```
**Response (201):**
```json
{
    "success": true,
    "message": "User registered successfully",
    "data": 1
}
```

---

### 1.2 Profile Controller
**Base:** `/api/v1/auth/profile`

---

#### `GET /api/v1/auth/profile`
Obtener perfil del usuario autenticado.

**Auth:** ✅ Bearer Token

**Response (200):**
```json
{
    "userId": 1,
    "fullName": "Juan Pérez",
    "email": "usuario@email.com"
}
```

---

#### `PUT /api/v1/auth/profile`
Actualizar perfil del usuario autenticado.

**Auth:** ✅ Bearer Token  
**Request Body:**
```json
{
    "fullName": "Juan Pérez Actualizado",
    "email": "nuevo@email.com"
}
```
**Response:** `204 No Content`

---

### 1.3 Farms Controller
**Base:** `/api/v1/farms`

---

#### `POST /api/v1/farms`
Crear una nueva finca.

**Auth:** ✅ Bearer Token  
**Request Body:**
```json
{
    "name": "Finca El Paraíso",
    "owner": "Juan Pérez",
    "address": "Km 5 vía Villavicencio",
    "geographicLocation": "4.1525,-73.6358"
}
```
**Response (201):**
```json
{
    "success": true,
    "message": "Farm created successfully",
    "data": {
        "id": 1,
        "name": "Finca El Paraíso",
        "owner": "Juan Pérez",
        "address": "Km 5 vía Villavicencio",
        "geographicLocation": "4.1525,-73.6358"
    }
}
```

---

#### `GET /api/v1/farms/{id}`
Obtener finca por ID.

**Auth:** ✅ Bearer Token

---

#### `GET /api/v1/farms/mine`
Obtener las fincas del usuario autenticado.

**Auth:** ✅ Bearer Token  
**Query Params:** `?includeInactive=false`

---

#### `GET /api/v1/farms/tenant/{userId}`
Obtener fincas de un usuario específico.

**Auth:** ✅ Bearer Token  
**Query Params:** `?includeInactive=false`

---

#### `PUT /api/v1/farms/{id}`
Actualizar una finca.

**Auth:** ✅ Bearer Token  
**Request Body:**
```json
{
    "name": "Finca Actualizada",
    "owner": "Nuevo Dueño",
    "address": "Nueva dirección",
    "geographicLocation": "4.1525,-73.6358"
}
```

---

#### `DELETE /api/v1/farms/{id}`
Eliminar una finca.

**Auth:** ✅ Bearer Token

---

## 2. 🐄 HerdService — Gestión de Hato

### 2.1 Animals Controller
**Base:** `/api/v1/animals`

---

#### `POST /api/v1/animals`
Registrar un nuevo animal.

**Auth:** ✅ Bearer Token  
**Request Body:**
```json
{
    "visualCode": "A001",
    "farmId": 1,
    "sex": "Female",
    "birthDate": "2023-01-15",
    "categoryId": 1,
    "breedId": 2,
    "name": "Luna",
    "electronicCode": "EC-001",
    "color": "Negro",
    "purpose": "Leche",
    "origin": "Nacido en finca",
    "initialCost": 1500000.00,
    "motherId": null,
    "fatherId": null,
    "externalMother": null,
    "externalFather": null,
    "batchId": 1,
    "paddockId": 3
}
```

---

#### `GET /api/v1/animals`
Listar animales de una finca.

**Auth:** ✅ Bearer Token  
**Query Params:** `?farmId=1&status=Active&includeInactive=false`

---

#### `GET /api/v1/animals/{id}`
Obtener animal por ID.

**Auth:** ✅ Bearer Token

---

#### `PUT /api/v1/animals/{id}`
Actualizar datos de un animal.

**Auth:** ✅ Bearer Token  
**Request Body:**
```json
{
    "id": 1,
    "name": "Luna Estrella",
    "color": "Negro con blanco",
    "purpose": "Doble propósito",
    "categoryId": 2
}
```

---

#### `DELETE /api/v1/animals/{id}`
Eliminar un animal.

**Auth:** ✅ Bearer Token

---

#### `POST /api/v1/animals/{id}/movements`
Registrar un movimiento del animal (traslado, compra, venta, etc.).

**Auth:** ✅ Bearer Token  
**Request Body:**
```json
{
    "animalId": 1,
    "movementTypeId": 2,
    "destinationPaddockId": 5,
    "date": "2024-06-15",
    "notes": "Traslado a potrero norte"
}
```

---

#### `PUT /api/v1/animals/{id}/weight`
Registrar/actualizar peso del animal.

**Auth:** ✅ Bearer Token  
**Request Body:**
```json
{
    "animalId": 1,
    "weight": 350.5,
    "weightDate": "2024-06-15"
}
```

---

#### `PUT /api/v1/animals/{id}/batch`
Mover animal a un lote.

**Auth:** ✅ Bearer Token  
**Request Body:**
```json
{
    "animalId": 1,
    "batchId": 3
}
```

---

#### `PUT /api/v1/animals/{id}/sell`
Marcar animal como vendido.

**Auth:** ✅ Bearer Token  
**Request Body:**
```json
{
    "animalId": 1,
    "salePrice": 2500000.00,
    "saleDate": "2024-06-15",
    "buyerInfo": "Finca Vecina"
}
```

---

#### `PUT /api/v1/animals/{id}/dead`
Marcar animal como muerto.

**Auth:** ✅ Bearer Token  
**Request Body:**
```json
{
    "animalId": 1,
    "deathDate": "2024-06-15",
    "causeOfDeath": "Enfermedad"
}
```

---

### 2.2 Batches Controller
**Base:** `/api/v1/batches`

---

#### `GET /api/v1/batches`
Listar lotes de una finca.

**Auth:** ✅ Bearer Token  
**Query Params:** `?farmId=1`

---

#### `POST /api/v1/batches`
Crear un lote.

**Auth:** ✅ Bearer Token  
**Request Body:**
```json
{
    "name": "Lote Novillos 2024",
    "farmId": 1,
    "description": "Novillos de engorde"
}
```

---

### 2.3 Breeds Controller
**Base:** `/api/v1/breeds`

---

#### `GET /api/v1/breeds`
Listar todas las razas.

**Auth:** ✅ Bearer Token

---

#### `POST /api/v1/breeds`
Crear una raza.

**Auth:** ✅ Bearer Token  
**Request Body:**
```json
{
    "name": "Brahman",
    "description": "Raza cebuina de origen indio"
}
```

---

### 2.4 Categories Controller
**Base:** `/api/v1/categories`

---

#### `GET /api/v1/categories`
Listar todas las categorías de animales.

**Auth:** ✅ Bearer Token

---

#### `POST /api/v1/categories`
Crear una categoría.

**Auth:** ✅ Bearer Token  
**Request Body:**
```json
{
    "name": "Novillo"
}
```
> ⚠️ **Nota:** El command solo acepta `name`. La categoría no tiene campo `description` ni `sex`.

---

### 2.5 MovementTypes Controller
**Base:** `/api/v1/movement-types`

---

#### `GET /api/v1/movement-types`
Listar todos los tipos de movimiento.

**Auth:** ✅ Bearer Token

---

### 2.6 Paddocks Controller
**Base:** `/api/v1/paddocks`

---

#### `GET /api/v1/paddocks`
Listar potreros de una finca.

**Auth:** ✅ Bearer Token  
**Query Params:** `?farmId=1`

---

#### `POST /api/v1/paddocks`
Crear un potrero.

**Auth:** ✅ Bearer Token  
**Request Body:**
```json
{
    "name": "Potrero Norte",
    "farmId": 1
}
```
> ⚠️ **Nota:** El command solo acepta `name` y `farmId`. El área se configura por separado en la entidad.

---

## 3. 🏥 HealthService — Salud Animal

### HealthEvent Controller
**Base:** `/api/v1/HealthEvent`

> ⚠️ **Nota:** Este controller usa `[controller]` como nombre de ruta, por lo que la ruta base es el nombre de la clase sin "Controller".

---

#### `POST /api/v1/HealthEvent`
Registrar un evento de salud.

**Auth:** ✅ Bearer Token  
**Request Body:**
```json
{
    "farmId": 1,
    "animalId": 5,
    "batchId": null,
    "type": "Treatment",
    "description": "Desparasitación",
    "eventDate": "2024-06-15",
    "observations": "Ivermectina 1ml/50kg",
    "estimatedCost": 25000.00
}
```

---

#### `GET /api/v1/HealthEvent/farm`
Obtener eventos sanitarios de la finca del contexto.

**Auth:** ✅ Bearer Token  
**Query Params:** `?page=1&pageSize=10`

---

#### `GET /api/v1/HealthEvent/animal/{animalId}`
Obtener eventos sanitarios de un animal.

**Auth:** ✅ Bearer Token  
**Query Params:** `?page=1&pageSize=10`

---

#### `GET /api/v1/HealthEvent/batch/{batchId}`
Obtener eventos sanitarios de un lote.

**Auth:** ✅ Bearer Token  
**Query Params:** `?page=1&pageSize=10`

---

#### `GET /api/v1/HealthEvent/type/{type}`
Obtener eventos sanitarios por tipo.

**Auth:** ✅ Bearer Token  
**Query Params:** `?page=1&pageSize=10`

---

#### `GET /api/v1/HealthEvent/dashboard-stats`
Obtener estadísticas del dashboard de salud.

**Auth:** ✅ Bearer Token

---

#### `GET /api/v1/HealthEvent/upcoming`
Obtener próximos eventos de salud programados.

**Auth:** ✅ Bearer Token  
**Query Params:** `?limit=10`

---

#### `GET /api/v1/HealthEvent/recent-treatments`
Obtener tratamientos recientes.

**Auth:** ✅ Bearer Token  
**Query Params:** `?limit=10`

---

#### `PUT /api/v1/HealthEvent/{id}`
Actualizar un evento de salud.

**Auth:** ✅ Bearer Token  
**Request Body:**
```json
{
    "description": "Desparasitación actualizada",
    "observations": "Se aplicó dosis doble",
    "estimatedCost": 50000.00
}
```

---

## 4. 🌾 FeedingService — Alimentación

### FeedingEvents Controller
**Base:** `/api/v1/feeding-events`

---

#### `POST /api/v1/feeding-events`
Crear evento de alimentación.

**Auth:** ✅ Bearer Token  
**Request Body:**
```json
{
    "farmId": 1,
    "supplyDate": "2024-06-15T08:00:00",
    "dietId": null,
    "batchId": 3,
    "animalId": null,
    "productId": 5,
    "totalQuantity": 100.0,
    "animalsFedCount": 25,
    "unitCostAtMoment": 1500.00,
    "observations": "Alimentación matutina",
    "registeredBy": "Juan"
}
```
**Response (201):**
```json
{
    "success": true,
    "message": "Feeding event created successfully",
    "data": {
        "id": 1,
        "farmId": 1,
        "supplyDate": "2024-06-15T08:00:00",
        "totalQuantity": 100.0,
        "totalCost": 150000.00
    }
}
```

---

#### `GET /api/v1/feeding-events/{id}`
Obtener evento de alimentación por ID.

**Auth:** ✅ Bearer Token

---

#### `GET /api/v1/feeding-events/farm/{farmId}`
Listar eventos de alimentación por finca.

**Auth:** ✅ Bearer Token  
**Query Params:** `?fromDate=2024-01-01&toDate=2024-12-31&page=1&pageSize=10`

---

#### `GET /api/v1/feeding-events/batch/{batchId}`
Listar eventos de alimentación por lote.

**Auth:** ✅ Bearer Token  
**Query Params:** `?page=1&pageSize=10`

---

#### `GET /api/v1/feeding-events/product/{productId}`
Listar eventos de alimentación por producto.

**Auth:** ✅ Bearer Token  
**Query Params:** `?page=1&pageSize=10`

---

#### `GET /api/v1/feeding-events/animal/{animalId}`
Listar eventos de alimentación por animal.

**Auth:** ✅ Bearer Token  
**Query Params:** `?page=1&pageSize=10`

---

#### `POST /api/v1/feeding-events/recalculate-cost`
Recalcular costo de un evento de alimentación.

**Auth:** ✅ Bearer Token  
**Request Body:**
```json
{
    "id": 1,
    "newUnitCost": 1800.00
}
```

---

#### `PUT /api/v1/feeding-events/{id}/cancel`
Cancelar un evento de alimentación.

**Auth:** ✅ Bearer Token

---

## 5. 📦 InventoryService — Inventario

### 5.1 Products Controller
**Base:** `/api/v1/Products`

> ⚠️ **Nota:** Usa `[controller]` en la ruta.

---

#### `POST /api/v1/Products`
Crear un producto.

**Auth:** ✅ Bearer Token  
**Request Body:**
```json
{
    "name": "Concentrado Lechero 40kg",
    "farmId": 1,
    "category": "Feed",
    "unitOfMeasure": "Kg",
    "unitPrice": 85000.00,
    "minimumStock": 10
}
```

---

#### `GET /api/v1/Products/{id}`
Obtener producto por ID.

**Auth:** ✅ Bearer Token

---

#### `GET /api/v1/Products`
Listar productos de una finca.

**Auth:** ✅ Bearer Token  
**Query Params:** `?farmId=1`

---

#### `GET /api/v1/Products/farms/{farmId}/low-stock`
Obtener productos con stock bajo.

**Auth:** ✅ Bearer Token

---

#### `PUT /api/v1/Products/{id}`
Actualizar un producto.

**Auth:** ✅ Bearer Token  
**Request Body:**
```json
{
    "name": "Concentrado Actualizado",
    "unitPrice": 90000.00,
    "minimumStock": 15
}
```

---

#### `DELETE /api/v1/Products/{id}`
Eliminar un producto.

**Auth:** ✅ Bearer Token

---

### 5.2 Inventory Controller
**Base:** `/api/v1/inventory`

---

#### `POST /api/v1/inventory`
Crear ítem de inventario.

**Auth:** ✅ Bearer Token  
**Request Body:**
```json
{
    "productId": 1,
    "farmId": 1,
    "quantity": 50,
    "location": "Bodega principal"
}
```

---

#### `GET /api/v1/inventory/farm/{farmId}`
Listar ítems de inventario por finca.

**Auth:** ✅ Bearer Token  
**Query Params:** `?page=1&pageSize=10`

---

### 5.3 InventoryMovements Controller
**Base:** `/api/v1/inventory-movements`

---

#### `POST /api/v1/inventory-movements`
Registrar un movimiento de inventario.

**Auth:** ✅ Bearer Token  
**Request Body:**
```json
{
    "productId": 1,
    "direction": "In",
    "quantity": 20,
    "concept": "Compra",
    "notes": "Compra de concentrado"
}
```

---

#### `GET /api/v1/inventory-movements/product/{productId}`
Obtener Kardex (historial de movimientos) de un producto.

**Auth:** ✅ Bearer Token

---

## 6. 🐣 ReproductionService — Reproducción

### Reproduction Controller
**Base:** `/api/v1/reproduction`

---

#### `POST /api/v1/reproduction`
Registrar un evento reproductivo.

**Auth:** ✅ Bearer Token  
**Request Body:**
```json
{
    "animalId": 5,
    "eventType": "NaturalMating",
    "eventDate": "2024-06-15",
    "farmId": 1,
    "maleId": 10,
    "observations": "Monta natural confirmada",
    "registeredBy": 1
}
```

---

#### `GET /api/v1/reproduction/{id}`
Obtener evento reproductivo por ID.

**Auth:** ✅ Bearer Token

---

#### `GET /api/v1/reproduction/animal/{animalId}`
Listar eventos reproductivos de un animal.

**Auth:** ✅ Bearer Token  
**Query Params:** `?page=1&pageSize=10`

---

#### `GET /api/v1/reproduction/farm`
Listar eventos reproductivos de la finca (del contexto JWT).

**Auth:** ✅ Bearer Token  
**Query Params:** `?fromDate=2024-01-01&toDate=2024-12-31&page=1&pageSize=10`

---

#### `GET /api/v1/reproduction/farm/{farmId}`
Listar eventos reproductivos de una finca específica.

**Auth:** ✅ Bearer Token  
**Query Params:** `?fromDate=2024-01-01&toDate=2024-12-31&page=1&pageSize=10`

---

#### `GET /api/v1/reproduction/type/{type}`
Listar eventos reproductivos por tipo.

**Auth:** ✅ Bearer Token  
**Tipos válidos:** `NaturalMating`, `ArtificialInsemination`, `PregnancyCheck`, `Birth`, `Abortion`, `Weaning`  
**Query Params:** `?page=1&pageSize=10`

---

#### `PUT /api/v1/reproduction/{id}/cancel`
Cancelar un evento reproductivo.

**Auth:** ✅ Bearer Token

---

#### `GET /api/v1/reproduction/pregnancies/farm/{farmId}`
Listar preñeces de una finca.

**Auth:** ✅ Bearer Token

---

#### `GET /api/v1/reproduction/births/farm/{farmId}`
Listar partos de una finca.

**Auth:** ✅ Bearer Token

---

#### `POST /api/v1/reproduction/births`
Registrar un parto.

**Auth:** ✅ Bearer Token  
**Request Body:**
```json
{
    "motherAnimalId": 5,
    "offspringTag": "CRIA-001",
    "weight": 35.5,
    "gender": "Female",
    "birthDate": "2024-06-15"
}
```

---

## 7. 💰 SalesService — Ventas

### Sales Controller
**Base:** `/api/v1/sales`

---

#### `GET /api/v1/sales`
Listar ventas del usuario.

**Auth:** ✅ Bearer Token

---

#### `GET /api/v1/sales/{id}`
Obtener venta por ID.

**Auth:** ✅ Bearer Token

---

#### `POST /api/v1/sales`
Crear una venta.

**Auth:** ✅ Bearer Token  
**Request Body:**
```json
{
    "farmId": 1,
    "animalId": 5,
    "buyerName": "Finca Vecina",
    "saleDate": "2024-06-15",
    "amount": 2500000.00,
    "notes": "Venta de novillo"
}
```
> ⚠️ **Nota:** Cada venta registra un solo animal (opcional). `animalId` puede ser `null` para ventas sin animal asociado.

---

#### `PUT /api/v1/sales/{id}`
Actualizar una venta.

**Auth:** ✅ Bearer Token

---

#### `DELETE /api/v1/sales/{id}`
Eliminar una venta.

**Auth:** ✅ Bearer Token

---

## 8. 🤝 CommercialService — Comercial

### 8.1 ThirdParties Controller
**Base:** `/api/v1/third-parties`

---

#### `POST /api/v1/third-parties`
Crear un tercero (proveedor/cliente).

**Auth:** ✅ Bearer Token  
**Request Body:**
```json
{
    "farmId": 1,
    "name": "Agropecuaria El Sol",
    "documentNumber": "900123456",
    "phone": "3001234567",
    "email": "contacto@elsol.com",
    "address": "Calle 10 #20-30",
    "isSupplier": true,
    "isCustomer": false
}
```

---

#### `GET /api/v1/third-parties`
Listar terceros de la finca.

**Auth:** ✅ Bearer Token  
**Query Params:** `?isSupplier=true&isCustomer=false&page=1&pageSize=10`

---

#### `GET /api/v1/third-parties/{id}`
Obtener tercero por ID.

**Auth:** ✅ Bearer Token

---

#### `PUT /api/v1/third-parties/{id}`
Actualizar tercero.

**Auth:** ✅ Bearer Token  
**Request Body:**
```json
{
    "name": "Agropecuaria El Sol Actualizada",
    "phone": "3009876543"
}
```

---

#### `DELETE /api/v1/third-parties/{id}`
Eliminar tercero.

**Auth:** ✅ Bearer Token

---

### 8.2 Transactions Controller
**Base:** `/api/v1/transactions`

---

#### `POST /api/v1/transactions`
Crear una transacción comercial.

**Auth:** ✅ Bearer Token  
**Request Body:**
```json
{
    "farmId": 1,
    "thirdPartyId": 3,
    "type": "Sale",
    "date": "2024-06-15",
    "totalAmount": 5000000.00,
    "paymentStatus": "Pending",
    "notes": "Venta de 3 novillos",
    "animals": [
        {
            "animalId": 5,
            "unitPrice": 2500000.00
        }
    ],
    "products": []
}
```

---

#### `GET /api/v1/transactions`
Listar transacciones de la finca.

**Auth:** ✅ Bearer Token  
**Query Params:** `?fromDate=2024-01-01&toDate=2024-12-31&type=Sale&page=1&pageSize=10`

---

#### `GET /api/v1/transactions/{id}`
Obtener transacción por ID.

**Auth:** ✅ Bearer Token

---

#### `GET /api/v1/transactions/{id}/animals`
Obtener animales asociados a una transacción.

**Auth:** ✅ Bearer Token

---

#### `GET /api/v1/transactions/{id}/products`
Obtener productos asociados a una transacción.

**Auth:** ✅ Bearer Token

---

## 9. 🤖 AIService — Inteligencia Artificial

### 9.1 Chat Controller
**Base:** `/api/v1/chat`

---

#### `POST /api/v1/chat`
Enviar mensaje al asistente de IA.

**Auth:** ✅ Bearer Token  
**Request Body:**
```json
{
    "message": "¿Cuántos animales tengo en mi finca?"
}
```
**Response (200):**
```json
{
    "response": "Según los registros de tu finca, actualmente cuentas con 45 animales activos..."
}
```

---

### 9.2 Diagnostic Controller
**Base:** `/api/v1/diagnostic`

---

#### `POST /api/v1/diagnostic/analyze-502`
Analizar un error 502.

**Auth:** ✅ Bearer Token  
**Request Body:**
```json
{
    "serviceName": "herd-service",
    "errorMessage": "Bad Gateway",
    "requestUrl": "/api/v1/animals",
    "timestamp": "2024-06-15T10:30:00Z"
}
```

---

#### `GET /api/v1/diagnostic/session/{sessionId}`
Obtener sesión de diagnóstico por ID.

**Auth:** ✅ Bearer Token

---

#### `GET /api/v1/diagnostic/recent`
Obtener diagnósticos recientes.

**Auth:** ✅ Bearer Token  
**Query Params:** `?count=20&serviceName=herd-service`

---

#### `POST /api/v1/diagnostic/resolve/{sessionId}`
Resolver/cerrar una sesión de diagnóstico.

**Auth:** ✅ Bearer Token  
**Request Body:**
```json
{
    "resolutionNotes": "Se reinició el servicio y el error se resolvió"
}
```

---

#### `GET /api/v1/diagnostic/service-status`
Obtener estado de los servicios.

**Auth:** ✅ Bearer Token

---

#### `GET /api/v1/diagnostic/patterns`
Obtener patrones de errores detectados.

**Auth:** ✅ Bearer Token

---

## 📋 Resumen de Rutas por Servicio

| Servicio | Ruta Base | # Endpoints |
|---|---|---|
| **AuthService** | `/api/v1/auth`, `/api/v1/farms` | 8 |
| **HerdService** | `/api/v1/animals`, `/api/v1/batches`, `/api/v1/breeds`, `/api/v1/categories`, `/api/v1/movement-types`, `/api/v1/paddocks` | 16 |
| **HealthService** | `/api/v1/HealthEvent` | 9 |
| **FeedingService** | `/api/v1/feeding-events` | 8 |
| **InventoryService** | `/api/v1/Products`, `/api/v1/inventory`, `/api/v1/inventory-movements` | 8 |
| **ReproductionService** | `/api/v1/reproduction` | 10 |
| **SalesService** | `/api/v1/sales` | 5 |
| **CommercialService** | `/api/v1/third-parties`, `/api/v1/transactions` | 9 |
| **AIService** | `/api/v1/chat`, `/api/v1/diagnostic` | 7 |
| **Total** | | **80** |

---

## 🔑 Ejemplo de Flujo Completo

1. **Registrar usuario:** `POST /api/v1/auth/register`
2. **Iniciar sesión:** `POST /api/v1/auth/login` → obtener token
3. **Crear finca:** `POST /api/v1/farms` (con Bearer Token)
4. **Crear raza:** `POST /api/v1/breeds`
5. **Crear categoría:** `POST /api/v1/categories`
6. **Crear potrero:** `POST /api/v1/paddocks`
7. **Registrar animal:** `POST /api/v1/animals`
8. **Registrar evento de salud:** `POST /api/v1/HealthEvent`
9. **Registrar alimentación:** `POST /api/v1/feeding-events`
10. **Registrar reproducción:** `POST /api/v1/reproduction`
11. **Consultar al asistente IA:** `POST /api/v1/chat`

> 💡 **Tip:** Usa el token JWT obtenido en el paso 2 como header `Authorization: Bearer <token>` en todas las peticiones posteriores.
