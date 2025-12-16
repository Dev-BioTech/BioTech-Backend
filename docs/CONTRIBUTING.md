# 📌 Development Guidelines

### 🌳 Git Flow - Branch Convention

#### Main Branches

| Branch | Purpose | Description |
|--------|---------|-------------|
| `main` | **Production** | Stable version deployed in production |
| `develop` | **Development** | Integration of new functionalities |

#### Working Branches

| Branch Type | Nomenclature | Purpose |
|-------------|--------------|---------|
| **Feature** | `feature/feature-name` | New backend functionalities |
| **Hotfix** | `hotfix/fix-name` | Critical fixes in production |
| **Release** | `release/vx.x.x` | Version preparation before production |

### 🧩 Commit Convention

**Format**: `<type>(<area>): <brief description>`

**Types**:
- `feat` - New functionality or endpoint
- `fix` - Bug fix
- `docs` - Documentation
- `style` - Formatting without affecting logic
- `refactor` - Code improvement or reorganization
- `test` - Unit / integration tests
- `chore` - Configuration, migrations, dependencies

**Examples**:
```bash
feat(api): add user registration endpoint
fix(database): correct SQL connection issue
test(services): add unit tests for UserService
docs(readme): update installation guide
```

### 📘 Best Practices

- **Code Organization**: One class per file, standard naming conventions.
- **Error Handling**: Custom exceptions, global middleware.
- **Security**: DTOs, Validation, Parameterized queries.
- **Testing**: Unit tests for logic, Integration tests for DB.
