# Sistema de Créditos en Línea

## Descripción
Sistema web que permite a los usuarios solicitar créditos en línea, mientras que los analistas del banco pueden gestionar y evaluar dichas solicitudes de manera eficiente.

## Tecnologías Utilizadas
- **Backend:** .NET 7 ([Descargar .NET 7 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0))
- **Base de datos:** SQLite ([Descargar SQLite](https://www.sqlite.org/download.html))
- **Gestor visual de base de datos:** DB Browser for SQLite ([Descargar DB Browser](https://sqlitebrowser.org/dl/))
- **Autenticación:** JWT
- **Documentación:** Swagger

## Requisitos Previos
- .NET 7 SDK (versión recomendada: 7.0.306)
- Git
- SQLite (CLI o binarios)
- DB Browser for SQLite (opcional, para visualizar la base de datos)
- Visual Studio 2022 o Visual Studio Code

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

## Instalación y Primeros Pasos

1. **Clonar el repositorio:**
   ```bash
   git clone https://github.com/Lozano7/online-credits-backend.git
   cd online-credits-backend
   ```

2. **(Opcional) Verifica el archivo `global.json`:**
   El proyecto ya incluye un archivo `global.json` para asegurar la versión correcta de .NET.

3. **Restaurar los paquetes NuGet:**
   ```bash
   dotnet restore
   ```

4. **(Opcional) Revisar la base de datos:**
   - **IMPORTANTE!** Si se quiere usar la base de datos incluida, puedes omitir este paso debido a que **El repositorio incluye la base de datos SQLite y las migraciones ya aplicadas**.

   - Si deseas limpiar y recrear la base de datos desde cero:
     ```bash
     dotnet ef database drop
     dotnet ef database update
     ```

5. **Ejecutar el proyecto:**
   ```bash
   dotnet run --project OnlineCredits.API
   ```

6. **Acceder a la documentación de la API:**
   - Abre en tu navegador: [http://localhost:5150/swagger]
   - Nota: La API fue adaptada para funcionar por **HTTP** (no HTTPS) para facilitar el desarrollo y pruebas locales.

## Notas sobre la Base de Datos y Migraciones

- **¿Tendré problemas al clonar el repositorio?**
  - No deberías tener problemas, ya que el proyecto incluye tanto las migraciones como la base de datos SQLite (`.db`).
  - Si tienes algún error relacionado con la base de datos, puedes recrearla fácilmente con los comandos de Entity Framework indicados arriba.

- **Asignación de roles Analista:**
  - El rol **Analista** debe ser asignado manualmente desde la base de datos. Puedes hacerlo fácilmente usando DB Browser for SQLite o cualquier editor de SQLite, modificando el campo `Role` del usuario correspondiente a `Analista` en la tabla `Users`.
   - En caso de usar la misma base de datos del proyecto existe un usario ya asignado con el rol **Analista** que es el siguiente:
    ```bash
   userName: analista_prueba
   password: Analista123!
   ```
   - El modelo entidad relacion se encuentra en la raiz del proyecto con el nombre: modelo-entidad-relación.pdf

## Características
- Autenticación basada en JWT
- CRUD de solicitudes de crédito
- Evaluación automática de solicitudes
- Logs de auditoría
- Exportación de reportes
- Gestión de documentos
- Roles de usuario (Solicitante/Analista)

