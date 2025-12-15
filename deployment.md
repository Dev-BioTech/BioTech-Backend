# Guía de Despliegue en Railway (Commercial & Inventory Services)

Esta guía explica cómo configurar la comunicación por eventos usando **RabbitMQ** en Railway.

## 1. Crear Servicio RabbitMQ
1. En tu Dashboard de Railway, haz clic en **New**.
2. Selecciona **Database** -> **RabbitMQ**.
3. Espera a que se despliegue.

## 2. Obtener Credenciales
1. Haz clic en el servicio de RabbitMQ recién creado.
2. Ve a la pestaña **Variables**.
3. Copia los valores de:
   - `RABBITMQ_HOST` (o la URL de conexión, pero el código espera host, user, pass).
   - `RABBITMQ_USER`
   - `RABBITMQ_PASSWORD`

> **Nota:** Si Railway te da una sola URL completa (`amqp://user:pass@host:port`), tendrás que extraer el host, user y password manualmente o ajustar el código para usar `cfg.Host(new Uri("amqp://..."))`. 
> *El código actual espera HOST, USER y PASS por separado.*

## 3. Configurar Microservicios
Para **cada** microservicio (`CommercialService` y `InventoryService`) en Railway:

1. Ve a la pestaña **Variables**.
2. Agrega las siguientes variables de entorno:

| Variable | Valor (Ejemplo) |
|----------|-----------------|
| `RABBITMQ_HOST` | `containers-us-west-123.railway.app` (sin el protocolo amqp://) |
| `RABBITMQ_USER` | `railway` |
| `RABBITMQ_PASS` | `tucontraseñasecreta` |

## 4. Verificar Conexión
- Al desplegar, revisa los **Logs** del servicio.
- Deberías ver logs de MassTransit indicando: `Bus started: rabbitmq://...`
- Si las variables no están, el sistema intentará conectar a `localhost` y fallará (o usará In-Memory si así se configuró para dev).

## 5. Eventos Soportados
Actualmente el sistema soporta:
- `TransactionCreatedEvent` (Publicado por Commercial, Consumido por Inventory).
