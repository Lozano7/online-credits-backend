# Sistema de Créditos en Línea

## Descripción
Sistema web que permite a los usuarios solicitar créditos en línea, mientras que los analistas del banco pueden gestionar y evaluar dichas solicitudes de manera eficiente.

## Tecnologías Utilizadas
- Backend: .NET 7 (toda la solución se desarrollará y ejecutará sobre .NET 7)
- Frontend: Angular 18 (próximamente)
- Base de datos: SQLite
- Autenticación: JWT
- Documentación: Swagger

## Requisitos
- .NET 7 SDK (versión 7.0.306)
- Visual Studio 2022 o Visual Studio Code
- Git

## Estructura del Proyecto
```
OnlineCredits.API (Proyecto principal)
├── Controllers
├── Program.cs
├── appsettings.json

OnlineCredits.Core (Proyecto de dominio)
├── Entities
├── Interfaces
├── DTOs
├── Enums
├── Exceptions

OnlineCredits.Infrastructure (Proyecto de infraestructura)
├── Data
│   ├── Configurations
│   ├── Context
│   ├── Repositories
├── Services
│   ├── JwtService
│   ├── FileService
├── Migrations

OnlineCredits.Application (Proyecto de aplicación)
├── Services
├── Mappings
├── Validators
```

## Instalación
1. Clonar el repositorio:
```bash
git clone https://github.com/Lozano7/online-credits-backend.git
```

2. Asegúrate de tener el archivo `global.json` con el siguiente contenido para forzar el uso de .NET 7:
```json
{
  "sdk": {
    "version": "7.0.306"
  }
}
```

3. Restaurar los paquetes NuGet:
```bash
dotnet restore
```

4. Ejecutar las migraciones:
```bash
dotnet ef database update
```

5. Ejecutar el proyecto:
```bash
dotnet run --project OnlineCredits.API
```

## Documentación API
La documentación de la API está disponible a través de Swagger en:
```
https://localhost:7001/swagger
```

## Características
- Autenticación basada en JWT
- CRUD de solicitudes de crédito
- Evaluación automática de solicitudes
- Logs de auditoría
- Exportación de reportes
- Gestión de documentos
- Roles de usuario (Solicitante/Analista)

## Licencia
Este proyecto está bajo la Licencia MIT. 