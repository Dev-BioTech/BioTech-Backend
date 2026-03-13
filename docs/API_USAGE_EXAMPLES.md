# API Usage Examples

This document provides practical examples of how to use the BioTech API endpoints for frontend integration.

## 🔐 Authentication

### Login
```javascript
// POST /api/Auth/login
const loginResponse = await fetch('http://localhost:5000/api/Auth/login', {
  method: 'POST',
  headers: {
    'Content-Type': 'application/json'
  },
  body: JSON.stringify({
    email: 'user@example.com',
    password: 'password123'
  })
});

const { token, user } = await loginResponse.json();
// Store token for future requests
localStorage.setItem('authToken', token);
```

### Authenticated Request
```javascript
// Include token in all authenticated requests
const headers = {
  'Content-Type': 'application/json',
  'Authorization': `Bearer ${localStorage.getItem('authToken')}`
};
```

---

## 🐄 Herd Service Examples

### Get All Breeds
```javascript
const getBreeds = async () => {
  const response = await fetch('http://localhost:5000/api/v1/breeds', {
    headers
  });
  
  if (response.ok) {
    const breeds = await response.json();
    return breeds.data;
  }
};
```

### Create Animal
```javascript
const createAnimal = async (animalData) => {
  const response = await fetch('http://localhost:5000/api/v1/animals', {
    method: 'POST',
    headers,
    body: JSON.stringify(animalData)
  });
  
  if (response.status === 201) {
    const animal = await response.json();
    return animal.data;
  }
};
```

### Get Animals by Farm
```javascript
const getAnimalsByFarm = async (farmId, status = 'active') => {
  const response = await fetch(`http://localhost:5000/api/v1/animals?farmId=${farmId}&status=${status}`, {
    headers
  });
  
  if (response.ok) {
    const animals = await response.json();
    return animals.data;
  }
};
```

---

## 📦 Inventory Service Examples

### Get Products
```javascript
const getProducts = async (farmId) => {
  const response = await fetch(`http://localhost:5000/api/v1/Products?farmId=${farmId}`, {
    headers
  });
  
  if (response.ok) {
    const products = await response.json();
    return products.data;
  }
};
```

### Get Low Stock Products
```javascript
const getLowStockProducts = async (farmId) => {
  const response = await fetch(`http://localhost:5000/api/v1/Products/farms/${farmId}/low-stock`, {
    headers
  });
  
  if (response.ok) {
    const products = await response.json();
    return products.data;
  }
};
```

### Create Product
```javascript
const createProduct = async (productData) => {
  const response = await fetch('http://localhost:5000/api/v1/Products', {
    method: 'POST',
    headers,
    body: JSON.stringify(productData)
  });
  
  if (response.status === 201) {
    const product = await response.json();
    return product.data;
  }
};

// Example usage:
const newProduct = {
  name: "Premium Feed",
  category: "Feed",
  unitOfMeasure: "kg",
  currentQuantity: 1000,
  averageCost: 25.50,
  minimumStock: 100,
  farmId: 1
};

const created = await createProduct(newProduct);
```

---

## 🏥 Health Service Examples

### Get Upcoming Health Events
```javascript
const getUpcomingEvents = async (limit = 10) => {
  const response = await fetch(`http://localhost:5000/api/v1/HealthEvent/upcoming?limit=${limit}`, {
    headers
  });
  
  if (response.ok) {
    const events = await response.json();
    return events.data;
  }
};
```

### Register Health Event
```javascript
const registerHealthEvent = async (eventData) => {
  const response = await fetch('http://localhost:5000/api/v1/HealthEvent', {
    method: 'POST',
    headers,
    body: JSON.stringify(eventData)
  });
  
  if (response.status === 201) {
    const event = await response.json();
    return event.data;
  }
};

// Example usage:
const healthEvent = {
  animalId: 123,
  eventType: "Vaccination",
  eventDate: "2025-03-02",
  notes: "Annual vaccination",
  farmId: 1
};

const created = await registerHealthEvent(healthEvent);
```

---

## 🧬 Reproduction Service Examples

### Get Pregnancies
```javascript
const getPregnancies = async (farmId) => {
  const response = await fetch(`http://localhost:5000/api/v1/Reproduction/pregnancies?farmId=${farmId}`, {
    headers
  });
  
  if (response.ok) {
    const pregnancies = await response.json();
    return pregnancies.data;
  }
};
```

### Register Birth
```javascript
const registerBirth = async (birthData) => {
  const response = await fetch('http://localhost:5000/api/v1/Reproduction/register-birth', {
    method: 'POST',
    headers,
    body: JSON.stringify(birthData)
  });
  
  if (response.status === 201) {
    const birth = await response.json();
    return birth.data;
  }
};

// Example usage:
const birthData = {
  motherAnimalId: 123,
  offspringTag: "B2025001",
  birthDate: "2025-03-02",
  weight: 35.5,
  gender: "Male",
  farmId: 1
};

const birth = await registerBirth(birthData);
```

---

## 💰 Sales Service Examples

### Get Sales
```javascript
const getSales = async () => {
  const response = await fetch('http://localhost:5000/api/v1/Sales', {
    headers
  });
  
  if (response.ok) {
    const sales = await response.json();
    return sales.data;
  }
};
```

### Create Sale
```javascript
const createSale = async (saleData) => {
  const response = await fetch('http://localhost:5000/api/v1/Sales', {
    method: 'POST',
    headers,
    body: JSON.stringify(saleData)
  });
  
  if (response.status === 201) {
    const sale = await response.json();
    return sale.data;
  }
};

// Example usage:
const saleData = {
  farmId: 1,
  animalId: 123,
  buyerName: "John Doe",
  saleDate: "2025-03-02",
  amount: 1500.00,
  notes: "Premium cattle sale"
};

const sale = await createSale(saleData);
```

---

## 🔄 Error Handling

### Global Error Handler
```javascript
const apiCall = async (url, options = {}) => {
  try {
    const response = await fetch(url, {
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${localStorage.getItem('authToken')}`,
        ...options.headers
      },
      ...options
    });

    if (!response.ok) {
      const error = await response.json();
      throw new Error(error.message || `HTTP error! status: ${response.status}`);
    }

    return await response.json();
  } catch (error) {
    console.error('API Error:', error);
    throw error;
  }
};

// Usage:
const products = await apiCall('http://localhost:5000/api/v1/Products?farmId=1');
```

### Response Validation
```javascript
const validateResponse = (response) => {
  if (!response.success) {
    throw new Error(response.message || 'API request failed');
  }
  return response.data;
};

// Usage:
try {
  const response = await apiCall('http://localhost:5000/api/v1/Products?farmId=1');
  const products = validateResponse(response);
  console.log('Products:', products);
} catch (error) {
  console.error('Failed to fetch products:', error.message);
}
```

---

## 📱 React Hook Example

```javascript
import { useState, useEffect } from 'react';

const useApi = (url, dependencies = []) => {
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchData = async () => {
      try {
        setLoading(true);
        const response = await apiCall(url);
        setData(validateResponse(response));
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, dependencies);

  return { data, loading, error };
};

// Usage in component:
const ProductList = ({ farmId }) => {
  const { data: products, loading, error } = useApi(
    `http://localhost:5000/api/v1/Products?farmId=${farmId}`,
    [farmId]
  );

  if (loading) return <div>Loading...</div>;
  if (error) return <div>Error: {error}</div>;

  return (
    <div>
      {products?.map(product => (
        <div key={product.id}>
          <h3>{product.name}</h3>
          <p>Stock: {product.currentQuantity}</p>
        </div>
      ))}
    </div>
  );
};
```

---

## 🚀 Best Practices

1. **Always validate responses** before using the data
2. **Handle network errors** gracefully
3. **Use loading states** during API calls
4. **Implement retry logic** for failed requests
5. **Cache responses** when appropriate
6. **Use environment variables** for API URLs
7. **Implement request cancellation** for unmounted components
8. **Log errors** for debugging purposes

---

**Last Updated:** 2025-03-02  
**API Version:** v1
