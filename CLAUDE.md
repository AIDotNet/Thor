# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Thor is an enterprise-grade AI model management gateway that provides unified API access to manage and orchestrate multiple AI models. It's built with .NET 10 and React, supporting 20+ AI models with comprehensive user management, channel management, billing, and monitoring capabilities.

## Build and Development Commands

### Backend (.NET 10)
```bash
# Main service project
dotnet run --project src/Thor.Service/Thor.Service.csproj --urls "http://localhost:5000"

# Build the entire solution
dotnet build Thor.sln

# Run with specific configuration
dotnet run --project src/Thor.Service/Thor.Service.csproj --configuration Release

# Restore dependencies
dotnet restore

# Run tests (if available)
dotnet test

# Generate database migrations (example for SQLite)
dotnet ef migrations add MigrationName --project src/Provider/Thor.Provider.Sqlite/Thor.Provider.Sqlite.csproj --startup-project src/Thor.Service/Thor.Service.csproj --context Thor.Provider.SqliteThorContext --output-dir Thor
```

### Frontend (React + Vite)
```bash
cd lobe

# Install dependencies
npm install

# Start development server
npm run dev

# Build for production
npm run build

# Build with TypeScript checking
npm run build:check

# Run linting
npm run lint

# Preview production build
npm run preview
```

### Full Project Build
```bash
# Windows
build.bat

# Linux/macOS
chmod +x build.sh
./build.sh
```

### Database Migrations
```bash
# Generate migrations for all supported databases
migrations.bat

# View migration logs
migrations.log.bat
```

## Code Architecture

### High-Level Structure
- **src/Thor.Service/** - Main ASP.NET Core web service and entry point
- **src/extensions/** - AI model provider integrations (OpenAI, Claude, Gemini, etc.)
- **src/framework/** - Building blocks for caching, events, and infrastructure
- **src/Provider/** - Database providers (SQLite, PostgreSQL, MySQL, SQL Server, DM)
- **src/Thor.Domain/** - Domain models and business logic
- **src/Thor.Core/** - Core services and application logic
- **src/Thor.Abstractions/** - Shared interfaces and DTOs
- **lobe/** - React frontend application

### AI Model Extensions Architecture
The system uses a plugin-based architecture where each AI provider is implemented as a separate extension:
- Each extension in `src/extensions/` provides services for a specific AI platform
- Extensions implement common interfaces for chat, embeddings, and other capabilities
- Providers are registered in `Program.cs` using extension methods (e.g., `.AddOpenAIService()`)

### Database Support
Multi-database support through provider pattern:
- Database type configured via `DBType` environment variable
- Supported: SQLite (default), PostgreSQL, MySQL, SQL Server, DaMeng
- Each provider implements the same interfaces with database-specific optimizations

### Frontend Architecture
- React 18 with TypeScript and Vite build system
- Ant Design UI components with custom theming
- Responsive design following mobile-first principles
- Internationalization support with i18next

## Key Configuration Files

### Backend Configuration
- **src/Thor.Service/appsettings.json** - Main application configuration
- **Directory.Build.props** - Shared MSBuild properties
- **NuGet.Config** - NuGet package source configuration

### Frontend Configuration  
- **lobe/package.json** - Frontend dependencies and scripts
- **lobe/vite.config.ts** - Vite build configuration
- **lobe/tsconfig.json** - TypeScript configuration

## Environment Variables

Key environment variables for development and deployment:

```bash
# Database Configuration
DBType=sqlite  # sqlite, postgresql, mysql, sqlserver, dm
ConnectionStrings:DefaultConnection=data source=/data/token.db
ConnectionStrings:LoggerConnection=data source=/data/logger.db

# Cache Configuration  
CACHE_TYPE=Memory  # Memory, Redis
CACHE_CONNECTION_STRING=localhost:6379

# Application Settings
RunMigrationsAtStartup=true
HttpClientPoolSize=100
TZ=Asia/Shanghai
```

## Development Practices

### Code Style
- Follow C# conventions and use built-in analyzers
- Use Mapster for object mapping instead of AutoMapper
- Implement proper async/await patterns throughout
- Use dependency injection extensively via built-in container

### Frontend Guidelines
- Follow Ant Design's design system and use theme tokens instead of hardcoded colors
- Use TypeScript strictly with proper type definitions
- Implement responsive design using Ant Design's breakpoint system
- Follow React hooks best practices and functional component patterns

### Database Operations
- Use Entity Framework Core with proper migrations
- Support multiple database providers through abstraction
- Implement proper transaction handling with Unit of Work pattern
- Use the ILoggerDbContext for logging-related operations

### AI Model Integration
- Each AI provider should implement common interfaces
- Handle rate limiting and error scenarios gracefully  
- Support streaming responses where applicable
- Implement proper token counting and billing

## Common Development Tasks

### Adding a New AI Model Provider
1. Create new project in `src/extensions/Thor.{ProviderName}/`
2. Implement required interfaces (chat, embeddings, etc.)
3. Add extension method for service registration
4. Register the service in `Program.cs`
5. Update documentation and model lists

### Database Schema Changes
1. Create migration using appropriate provider context
2. Update all database providers consistently
3. Test migration on different database types
4. Update seed data if necessary

### Frontend Component Development
1. Use Ant Design components as foundation
2. Implement responsive behavior using breakpoints
3. Follow theming system with proper token usage
4. Add proper TypeScript interfaces
5. Test across different viewport sizes

## Testing and Quality

### Running Tests
- Backend tests: `dotnet test` (when test projects exist)
- Frontend linting: `cd lobe && npm run lint`
- Type checking: `cd lobe && npm run build:check`

### Build Verification
- Use `build.bat` or `build.sh` to verify full project builds
- Test database migrations on different providers
- Verify frontend build integration with backend

### Docker Support
- Main service Dockerfile: `src/Thor.Service/Dockerfile`
- Docker Compose configurations available for different scenarios
- Multi-architecture support (AMD64, ARM64)

## Troubleshooting

### Common Issues
- **Frontend build failures**: Clear `lobe/node_modules` and reinstall
- **Database connection issues**: Check `DBType` and connection strings
- **Missing AI provider**: Ensure extension is registered in `Program.cs`
- **CORS issues**: Verify AllowAll CORS policy is properly configured

### Performance Optimization
- Use Redis caching for production deployments
- Enable response compression for static assets
- Configure appropriate HTTP client pool sizes
- Monitor AI provider rate limits and implement fallbacks