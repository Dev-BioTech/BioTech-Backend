# API Data Models & DTOs

This document contains the data models and DTOs used throughout the BioTech API for frontend integration.

## 🔐 Auth Service DTOs

### User
```typescript
interface User {
  id: number;
  email: string;
  firstName: string;
  lastName: string;
  isActive: boolean;
  createdAt: string;
  updatedAt: string;
}

interface LoginRequest {
  email: string;
  password: string;
}

interface LoginResponse {
  token: string;
  user: User;
}

interface RegisterRequest {
  email: string;
  password: string;
  firstName: string;
  lastName: string;
}
```

### Farm
```typescript
interface Farm {
  id: number;
  name: string;
  location: string;
  isActive: boolean;
  tenantUserId: number;
  createdAt: string;
  updatedAt: string;
}

interface CreateFarmRequest {
  name: string;
  location: string;
}
```

---

## 🐄 Herd Service DTOs

### Animal
```typescript
interface Animal {
  id: number;
  tag: string;
  breedId: number;
  categoryId: number;
  batchId?: number;
  paddockId?: number;
  gender: 'Male' | 'Female';
  birthDate: string;
  weight?: number;
  status: 'Active' | 'Sold' | 'Dead' | 'Transferred';
  farmId: number;
  createdAt: string;
  updatedAt: string;
}

interface CreateAnimalRequest {
  tag: string;
  breedId: number;
  categoryId: number;
  batchId?: number;
  paddockId?: number;
  gender: 'Male' | 'Female';
  birthDate: string;
  weight?: number;
  farmId: number;
}

interface UpdateAnimalRequest {
  tag?: string;
  breedId?: number;
  categoryId?: number;
  batchId?: number;
  paddockId?: number;
  weight?: number;
  status?: 'Active' | 'Sold' | 'Dead' | 'Transferred';
}
```

### Breed
```typescript
interface Breed {
  id: number;
  name: string;
  description?: string;
  characteristics?: string;
  origin?: string;
  averageWeight?: number;
  averageMilkProduction?: number;
  farmId: number;
  createdAt: string;
  updatedAt: string;
}

interface CreateBreedRequest {
  name: string;
  description?: string;
  characteristics?: string;
  origin?: string;
  averageWeight?: number;
  averageMilkProduction?: number;
}

interface BreedResponse {
  id: number;
  name: string;
}
```

### Category
```typescript
interface Category {
  id: number;
  name: string;
  description?: string;
  typicalUse?: string;
  farmId: number;
  createdAt: string;
  updatedAt: string;
}

interface CreateCategoryRequest {
  name: string;
  description?: string;
  typicalUse?: string;
}

interface CategoryResponse {
  id: number;
  name: string;
}
```

### Batch
```typescript
interface Batch {
  id: number;
  name: string;
  description?: string;
  breedId: number;
  paddockId?: number;
  animalCount: number;
  averageAge?: number;
  averageWeight?: number;
  farmId: number;
  createdAt: string;
  updatedAt: string;
}

interface CreateBatchRequest {
  name: string;
  description?: string;
  breedId: number;
  paddockId?: number;
  farmId: number;
}

interface BatchResponse {
  id: number;
  name: string;
  breedId: number;
  animalCount: number;
  farmId: number;
}
```

### Paddock
```typescript
interface Paddock {
  id: number;
  name: string;
  description?: string;
  area: number; // in square meters
  capacity: number;
  currentOccupancy: number;
  farmId: number;
  createdAt: string;
  updatedAt: string;
}

interface CreatePaddockRequest {
  name: string;
  description?: string;
  area: number;
  capacity: number;
  farmId: number;
}

interface PaddockResponse {
  id: number;
  name: string;
  area: number;
  capacity: number;
  currentOccupancy: number;
  farmId: number;
}
```

---

## 📦 Inventory Service DTOs

### Product
```typescript
interface Product {
  id: number;
  name: string;
  category: string;
  unitOfMeasure: string;
  currentQuantity: number;
  averageCost: number;
  minimumStock: number;
  farmId: number;
  active: boolean;
  createdAt: string;
  updatedAt: string;
}

interface CreateProductRequest {
  name: string;
  category: string;
  unitOfMeasure: string;
  currentQuantity: number;
  averageCost: number;
  minimumStock: number;
  farmId: number;
}

interface UpdateProductRequest {
  name?: string;
  category?: string;
  unitOfMeasure?: string;
  currentQuantity?: number;
  averageCost?: number;
  minimumStock?: number;
  active?: boolean;
}

interface ProductDto {
  id: number;
  name: string;
  category: string;
  unitOfMeasure: string;
  currentQuantity: number;
  averageCost: number;
  minimumStock: number;
  farmId: number;
  active: boolean;
}

interface LowStockProductDto {
  id: number;
  name: string;
  currentQuantity: number;
  minimumStock: number;
}
```

---

## 🏥 Health Service DTOs

### Health Event
```typescript
interface HealthEvent {
  id: number;
  animalId: number;
  eventType: string;
  eventDate: string; // DateOnly format
  notes?: string;
  treatment?: string;
  veterinarian?: string;
  cost?: number;
  followUpDate?: string;
  farmId: number;
  createdAt: string;
  updatedAt: string;
}

interface CreateHealthEventRequest {
  animalId: number;
  eventType: string;
  eventDate: string;
  notes?: string;
  treatment?: string;
  veterinarian?: string;
  cost?: number;
  followUpDate?: string;
  farmId: number;
}

interface UpdateHealthEventRequest {
  eventType?: string;
  eventDate?: string;
  notes?: string;
  treatment?: string;
  veterinarian?: string;
  cost?: number;
  followUpDate?: string;
}

interface HealthEventResponse {
  id: number;
  animalId: number;
  eventType: string;
  eventDate: string;
  notes?: string;
  farmId: number;
}

interface UpcomingHealthEventDto {
  id: number;
  animalId: number;
  animalTag: string;
  eventType: string;
  eventDate: string;
  notes?: string;
}

interface HealthDashboardStats {
  totalEvents: number;
  upcomingEvents: number;
  recentTreatments: number;
  vaccinationsDue: number;
}
```

---

## 🧬 Reproduction Service DTOs

### Reproduction Event
```typescript
interface ReproductionEvent {
  id: number;
  animalId: number;
  eventType: 'Heat' | 'Insemination' | 'Pregnancy' | 'Birth' | 'Abortion';
  eventDate: string;
  notes?: string;
  sireId?: number;
  result?: string;
  expectedDate?: string;
  farmId: number;
  createdAt: string;
  updatedAt: string;
}

interface CreateReproductionEventRequest {
  animalId: number;
  eventType: 'Heat' | 'Insemination' | 'Pregnancy' | 'Birth' | 'Abortion';
  eventDate: string;
  notes?: string;
  sireId?: number;
  result?: string;
  expectedDate?: string;
  farmId: number;
}
```

### Birth
```typescript
interface BirthDto {
  id: number;
  motherAnimalId: number;
  offspringTag: string;
  birthDate: string;
  weight: number;
  gender: 'Male' | 'Female';
  farmId: number;
}

interface RegisterBirthRequest {
  motherAnimalId: number;
  offspringTag: string;
  birthDate: string;
  weight: number;
  gender: 'Male' | 'Female';
  notes?: string;
  farmId: number;
}

interface PregnancyDto {
  id: number;
  animalId: number;
  animalTag: string;
  breedingDate: string;
  expectedBirthDate: string;
  status: 'Confirmed' | 'Pending' | 'Completed';
  farmId: number;
}
```

---

## 💰 Sales Service DTOs

### Sale
```typescript
interface Sale {
  id: number;
  farmId: number;
  animalId?: number;
  buyerName: string;
  saleDate: string;
  amount: number;
  notes?: string;
  createdAt: string;
  updatedAt: string;
}

interface CreateSaleRequest {
  farmId: number;
  animalId?: number;
  buyerName: string;
  saleDate: string;
  amount: number;
  notes?: string;
}

interface UpdateSaleRequest {
  buyerName?: string;
  saleDate?: string;
  amount?: number;
  notes?: string;
}

interface SaleDto {
  id: number;
  farmId: number;
  animalId?: number;
  buyerName: string;
  saleDate: string;
  amount: number;
  notes?: string;
  createdAt: string;
}
```

---

## 📄 Common Response Formats

### API Response Wrapper
```typescript
interface ApiResponse<T> {
  success: boolean;
  message: string;
  data: T;
  errors?: string[];
}

interface PaginatedResponse<T> {
  success: boolean;
  message: string;
  data: T[];
  pagination: {
    currentPage: number;
    pageSize: number;
    totalCount: number;
    totalPages: number;
  };
}
```

### Error Response
```typescript
interface ErrorResponse {
  success: false;
  message: string;
  data: null;
  errors: string[];
}
```

---

## 🔄 Validation Rules

### Common Validation
```typescript
// Required fields validation
const requiredFields = {
  name: { minLength: 2, maxLength: 100 },
  email: { format: 'email' },
  farmId: { type: 'number', min: 1 },
  animalId: { type: 'number', min: 1 },
  amount: { type: 'number', min: 0 },
  weight: { type: 'number', min: 0 }
};

// Date formats
const dateFormats = {
  dateOnly: 'YYYY-MM-DD', // e.g., "2025-03-02"
  dateTime: 'YYYY-MM-DDTHH:mm:ss' // e.g., "2025-03-02T10:30:00"
};

// Enum values
const genders = ['Male', 'Female'] as const;
const animalStatuses = ['Active', 'Sold', 'Dead', 'Transferred'] as const;
const reproductionEventTypes = ['Heat', 'Insemination', 'Pregnancy', 'Birth', 'Abortion'] as const;
```

---

## 📱 Frontend Type Definitions

### React Props Types
```typescript
// Common component props
interface BaseComponentProps {
  className?: string;
  children?: React.ReactNode;
}

interface DataComponentProps<T> extends BaseComponentProps {
  data: T;
  loading?: boolean;
  error?: string;
  onRefresh?: () => void;
}

// Form component props
interface FormProps<T> extends BaseComponentProps {
  initialData?: Partial<T>;
  onSubmit: (data: T) => Promise<void>;
  onCancel?: () => void;
  loading?: boolean;
}

// Table component props
interface TableProps<T> extends BaseComponentProps {
  data: T[];
  columns: TableColumn<T>[];
  onRowClick?: (item: T) => void;
  onEdit?: (item: T) => void;
  onDelete?: (item: T) => void;
  loading?: boolean;
}

interface TableColumn<T> {
  key: keyof T;
  title: string;
  sortable?: boolean;
  filterable?: boolean;
  render?: (value: any, item: T) => React.ReactNode;
}
```

---

## 🚀 Usage Examples

### TypeScript Integration
```typescript
// Import types
import { Product, CreateProductRequest } from '../types/api';

// Use in components
const ProductForm: React.FC = () => {
  const [formData, setFormData] = useState<CreateProductRequest>({
    name: '',
    category: '',
    unitOfMeasure: '',
    currentQuantity: 0,
    averageCost: 0,
    minimumStock: 0,
    farmId: 1
  });

  const handleSubmit = async (data: CreateProductRequest) => {
    try {
      const response = await apiCall<Product>('/api/v1/Products', {
        method: 'POST',
        body: JSON.stringify(data)
      });
      
      console.log('Product created:', response);
    } catch (error) {
      console.error('Failed to create product:', error);
    }
  };

  return (
    // Form JSX
  );
};
```

---

**Last Updated:** 2025-03-02  
**API Version:** v1
