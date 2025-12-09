
# RealEstate Backend

## Descripción

Back para la gestión inmobiliarias en **.NET 8** y **SQL Server**, implementando **Arquitectura Limpia**, **Domain-Driven Design (DDD)**, **Unit of Work**, **Repository Pattern**, **MediatR**, **JWT Authentication** para token y autorizaciones con roles y usuarios en DB, validación **FluentValidation**, manejo de errores con el patrón **Result**, **CQRS** para la La segregación de resposabilidades y **Serilog** con sql server para el manejo de Logs.

Este proyecto se diseño para ser **escalable, mantenible y testeable**, siguiendo buenas prácticas de codigo.

---

## Estructura

```
src/
├─ RealEstate.API/                   # API REST
├─ RealEstate.Application/           # Capa de aplicación (Commands, Queries, Handlers, Interfaces, Interfaces UnitOfWork, Validators, Mappings, AssemblyMarker)
├─ RealEstate.Domain/                # Capa de dominio (entidades y reglas)
├─ RealEstate.Contracts              # DTOs
├─ RealEstate.Infrastructure/        # Persistencia (EF Core, migraciones, repositorios, services, Repository UnitOfWork)
├─ RealEstate.SharedKernel/          # Clases compartidas (Result, ValidationBehavior)
└─ tests/RealEstate.Application.Tests/  # Pruebas unitarias
```
---

## Capas y responsabilidades

### 1. Dominio

- Contiene **entidades** (`Property`, `Owner`, `PropertyImage`, `PropertyTrace`) y sus reglas.
- No depende de ninguna otra capa ni de frameworks externos.

### 2. Aplicación

- Implementa **Commands** y **Queries** en el **Handlers** usando **MediatR**.
- Validadores con **FluentValidation**.
- Handlers:
  - `CreatePropertyCommandHandler`
  - `ChangePriceHandler`
  - `UpdatePropertyHandler`
  - `CreateOwnerCommandHandler`
  - `RegisterSaleCommandHandler`
  - `CreatePropertyImageHandler`
  - `LoginHandler`
  - `GetPropertiesHandler`
  - `GetPropertyByIdHandler`

### 3. Infraestructura

- Implementa **persistencia** y **Migration** con EF Core y SQL Server.
- Repositorios y **Unit of Work** (`IUnitOfWork`) para coordinar transacciones.
- Servicio de apoyo `IFileService` para manejo de archivos.

### 4. API

- Endpoints REST que reciben los **Commands** y **Queries** y retornan los resultados mapeados a **DTOs**.
- Seguridad implementada con **JWT Bearer**, incluyendo roles y permisos.

### 5. Contracts

 - DTOs

---

## Principales patrones aplicados

### Arquitectura Limpia

- Separación de responsabilidades: **Test**, **Dominio**, **Aplicación**, **Infraestructura**, **Contracts**, **API**.
- Cada capa depende únicamente de capas internas, evitando acoplamiento con frameworks externos.

### Domain-Driven Design (DDD)

- Entidades y agregados con lógica encapsulada.
- Interfaces y Repositorios que abstraen la persistencia de datos.
- Reglas de negocio.

### Unit of Work y Repository Pattern

- `IUnitOfWork` coordina múltiples repositorios y asegura consistencia.
- Repositorios para cada agregado (`Property`, `Owner`, `PropertyImage`, `PropertyTrace`).

### Result Pattern

- Clases `Result` y `Result<T>` devuelven resultados consistentes con:
  - `IsSuccess` y `Error`
  - Evita lanzar excepciones para casos esperados.

### FluentValidation

- Validadores para cada comando, garantizando que los datos cumplan reglas antes de ejecutar la lógica.
- Ejemplo:
```csharp
RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
RuleFor(x => x.Address).NotEmpty().WithMessage("Address is required");
RuleFor(x => x.Base64Photo).NotEmpty().WithMessage("Photo is required");
```

### AutoMapper

- Mapea entidades a **DTOs**, desacoplando la capa de dominio de la API.

### JWT Authentication

- Endpoints protegidos con tokens JWT.
- Incluye roles y permisos, asegurando autorización a nivel de recurso.

### Logger Serilog

 - Serilog para el manejos de Losg el proyecto
 - Tiene su propia tabla en DB SQL creada con la migracion EF
 - Capta eventos automaticamente y tambien los configurados manual, es decir los del sistema y los programados

---

## Pruebas unitarias

- Implementadas con **NUnit** y **Moq**.
- Prueban tanto **escenarios exitosos** como **errores esperados**.
- CI/CD ejecuta tests automáticamente en GitHub Actions.

---

## CI/CD

- **GitHub Actions** configurado para:
  1. Hacer checkout del repositorio.
  2. Instalar .NET 8.
  3. Restaurar y construir proyectos (`.csproj` directamente).
  4. Ejecutar pruebas unitarias y generar resultados.

---
## Comandos para la Migrations y ejecución del poryecto

**Add-Migration Initial -Context RealEstateDbContext –Project RealEstate.Infrastructure -OutputDir Persistence/Migrations -StartupProject RealEstate.API**

**update-database -Context RealEstateDbContext -Project RealEstate.Infrastructure -StartupProject RealEstate.API**

 - Una vez se realice la migración, abrir el proyecto con el archivo RealEstate.sln en la raíz y ejecutar la api.
 - Registre un usuario en la tabla db User con el rol **User**
 - Ejecute el endpoitn Login y genere un token, swagger esta configurado para jwt por lo tanto no es necesario postman
 - Inserte el token en Authorize de swagger
 - Consuma los diferentes endpoint

---

## Beneficios de esta arquitectura

- Código **mantenible y escalable**.
- **Testable** y desacoplado.
- Cambios en la persistencia no afectan la lógica de negocio.
- Manejo de errores consistente con `Result`.
- Validación centralizada con FluentValidation.
- Seguridad con JWT.
- Separación clara entre dominio, aplicación, infraestructura y API.

---

## Tecnologías utilizadas

- .NET 8
- C#
- SQL Server
- MediatR
- Serilog
- EF Core
- FluentValidation
- AutoMapper
- JWT Bearer Authentication
- NUnit y Moq para pruebas unitarias
- GitHub Actions para CI/CD