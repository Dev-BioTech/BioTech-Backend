namespace InventoryService.Domain.Enums;

public enum MovementType
{
    PURCHASE,      // Compra (Entrada)
    SALE,          // Venta (Salida)
    CONSUMPTION,   // Consumo interno (Salida)
    ADJUSTMENT_IN, // Ajuste inventario (Entrada)
    ADJUSTMENT_OUT,// Ajuste inventario (Salida)
    LOSS,          // Pérdida/Merma (Salida)
    RETURN_IN,     // Devolución de cliente (Entrada)
    RETURN_OUT     // Devolución a proveedor (Salida)
}
