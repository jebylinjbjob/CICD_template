# Docker Deployment Guide

This guide explains how to deploy the PortalApi application using Docker.

## Prerequisites

- Docker 20.10 or later
- Docker Compose 2.0 or later

## Quick Start

### Development Environment

Run the application with PostgreSQL in development mode:

```bash
docker-compose up -d
```

The application will be available at:
- API: http://localhost:5000

PostgreSQL will be available at:
- Host: localhost
- Port: 5432
- Database: PortalApiDb
- Username: postgres
- Password: postgres

### Production Environment

For production deployment with resource limits and production configurations:

```bash
docker-compose -f docker-compose.yml -f docker-compose.prod.yml up -d
```

**Important:** Set the `POSTGRES_PASSWORD` environment variable before running:

```bash
export POSTGRES_PASSWORD=your_strong_password
docker-compose -f docker-compose.yml -f docker-compose.prod.yml up -d
```

## Docker Commands

### Build the application image

```bash
docker-compose build
```

### Start services

```bash
docker-compose up -d
```

### View logs

```bash
# All services
docker-compose logs -f

# Specific service
docker-compose logs -f portalapi
docker-compose logs -f postgres
```

### Stop services

```bash
docker-compose down
```

### Stop services and remove volumes

```bash
docker-compose down -v
```

### Restart a service

```bash
docker-compose restart portalapi
```

## Database Migration

The application will automatically run database migrations on startup. However, you can also run migrations manually using the DbMigrator tool:

```bash
# Build the DbMigrator
docker build -f Dockerfile.migrator -t portalapi-migrator .

# Run migrations
docker run --rm --network portalapi-network \
  -e ConnectionStrings__Default="Host=postgres;Port=5432;Database=PortalApiDb;Username=postgres;Password=postgres" \
  portalapi-migrator
```

## Environment Variables

You can customize the application behavior using environment variables:

| Variable | Description | Default |
|----------|-------------|---------|
| `ASPNETCORE_ENVIRONMENT` | Application environment | Production |
| `ASPNETCORE_URLS` | URLs the app listens on | http://+:8080 |
| `ConnectionStrings__Default` | PostgreSQL connection string | See docker-compose.yml |
| `POSTGRES_DB` | PostgreSQL database name | PortalApiDb |
| `POSTGRES_USER` | PostgreSQL username | postgres |
| `POSTGRES_PASSWORD` | PostgreSQL password | postgres |

### Example: Using custom environment variables

Create a `.env` file in the same directory as docker-compose.yml:

```env
POSTGRES_PASSWORD=my_secure_password
ASPNETCORE_ENVIRONMENT=Staging
```

Then run:

```bash
docker-compose up -d
```

## Volumes

### PostgreSQL Data

Data is persisted in a Docker volume named `postgres_data` (or `postgres_prod_data` in production). This ensures your data survives container restarts.

To backup the database:

```bash
docker-compose exec postgres pg_dump -U postgres PortalApiDb > backup.sql
```

To restore the database:

```bash
docker-compose exec -T postgres psql -U postgres PortalApiDb < backup.sql
```

## Health Checks

Both services include health checks:

- **PostgreSQL**: Checks if the database is ready using `pg_isready`
- **PortalApi**: Checks HTTP endpoint at `/health`

Check service health:

```bash
docker-compose ps
```

## Troubleshooting

### Application won't start

1. Check logs:
   ```bash
   docker-compose logs portalapi
   ```

2. Verify database is healthy:
   ```bash
   docker-compose ps postgres
   ```

3. Check database connection:
   ```bash
   docker-compose exec postgres psql -U postgres -d PortalApiDb -c "SELECT 1;"
   ```

### Database connection issues

Ensure the connection string uses the service name `postgres` as the host (not `localhost`):

```
Host=postgres;Port=5432;Database=PortalApiDb;Username=postgres;Password=postgres
```

### Port conflicts

If ports 5000 or 5432 are already in use, modify the port mappings in docker-compose.yml:

```yaml
ports:
  - "5001:8080"  # Changed from 5000:8080
```

### Reset everything

To completely reset the environment:

```bash
docker-compose down -v
docker-compose build --no-cache
docker-compose up -d
```

## Development with Docker

For development with hot-reload, use the override file:

```bash
docker-compose -f docker-compose.yml -f docker-compose.override.yml up
```

This will:
- Use development environment settings
- Enable debug logging
- Mount source code for faster iteration
- Use different ports to avoid conflicts

## Production Deployment Checklist

- [ ] Change default PostgreSQL password
- [ ] Configure proper connection strings
- [ ] Review and adjust resource limits
- [ ] Set up proper logging and monitoring
- [ ] Configure backup strategy for PostgreSQL data
- [ ] Review security settings
- [ ] Set up HTTPS/TLS certificates
- [ ] Configure reverse proxy (nginx, traefik, etc.)

## Architecture

```
┌─────────────────┐
│   Client        │
└────────┬────────┘
         │
         │ HTTP (5000)
         │
┌────────▼────────┐
│   PortalApi     │
│   Container     │
│   (.NET 8.0)    │
└────────┬────────┘
         │
         │ Port 5432
         │
┌────────▼────────┐
│   PostgreSQL    │
│   Container     │
│   (v15)         │
└─────────────────┘
```

## Additional Resources

- [Docker Documentation](https://docs.docker.com/)
- [Docker Compose Documentation](https://docs.docker.com/compose/)
- [ABP Framework Documentation](https://docs.abp.io/)
