#!/bin/bash

# PortalApi Docker Management Script

set -e

RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

print_info() {
    echo -e "${GREEN}[INFO]${NC} $1"
}

print_warn() {
    echo -e "${YELLOW}[WARN]${NC} $1"
}

print_error() {
    echo -e "${RED}[ERROR]${NC} $1"
}

show_help() {
    cat << EOF
PortalApi Docker Management Script

Usage: ./docker-start.sh [COMMAND] [OPTIONS]

Commands:
    dev         Start in development mode
    prod        Start in production mode
    stop        Stop all services
    restart     Restart all services
    logs        Show logs (use -f to follow)
    build       Build Docker images
    clean       Stop services and remove volumes
    migrate     Run database migrations
    backup      Backup PostgreSQL database
    restore     Restore PostgreSQL database from backup
    help        Show this help message

Examples:
    ./docker-start.sh dev           # Start in development mode
    ./docker-start.sh prod          # Start in production mode
    ./docker-start.sh logs -f       # Follow logs
    ./docker-start.sh backup        # Backup database to backup.sql

Environment Variables:
    POSTGRES_PASSWORD   PostgreSQL password (required for production)

EOF
}

check_docker() {
    if ! command -v docker &> /dev/null; then
        print_error "Docker is not installed"
        exit 1
    fi

    if ! command -v docker-compose &> /dev/null; then
        print_error "Docker Compose is not installed"
        exit 1
    fi
}

start_dev() {
    print_info "Starting PortalApi in development mode..."
    docker-compose up -d
    print_info "Services started successfully!"
    print_info "API: http://localhost:5000"
    print_info "PostgreSQL: localhost:5432"
    echo ""
    print_info "To view logs: ./docker-start.sh logs -f"
}

start_prod() {
    print_info "Starting PortalApi in production mode..."
    
    if [ -z "$POSTGRES_PASSWORD" ]; then
        print_warn "POSTGRES_PASSWORD environment variable is not set"
        print_warn "Using default password 'postgres' (NOT RECOMMENDED for production)"
        read -p "Continue? (y/N) " -n 1 -r
        echo
        if [[ ! $REPLY =~ ^[Yy]$ ]]; then
            exit 1
        fi
    fi

    docker-compose -f docker-compose.yml -f docker-compose.prod.yml up -d
    print_info "Services started successfully!"
    print_info "API: http://localhost:5000"
}

stop_services() {
    print_info "Stopping all services..."
    docker-compose down
    print_info "Services stopped successfully!"
}

restart_services() {
    print_info "Restarting all services..."
    docker-compose restart
    print_info "Services restarted successfully!"
}

show_logs() {
    docker-compose logs "$@"
}

build_images() {
    print_info "Building Docker images..."
    docker-compose build --no-cache
    print_info "Build completed successfully!"
}

clean_all() {
    print_warn "This will remove all containers, networks, and volumes!"
    read -p "Are you sure? (y/N) " -n 1 -r
    echo
    if [[ $REPLY =~ ^[Yy]$ ]]; then
        print_info "Cleaning up..."
        docker-compose down -v
        print_info "Cleanup completed!"
    fi
}

run_migrations() {
    print_info "Running database migrations..."
    
    if ! docker ps | grep -q portalapi-postgres; then
        print_error "PostgreSQL container is not running. Start services first."
        exit 1
    fi

    print_info "Building migrator image..."
    docker build -f Dockerfile.migrator -t portalapi-migrator .
    
    print_info "Running migrations..."
    docker run --rm --network cicd_template_portalapi-network \
        -e ConnectionStrings__Default="Host=postgres;Port=5432;Database=PortalApiDb;Username=postgres;Password=postgres" \
        portalapi-migrator
    
    print_info "Migrations completed successfully!"
}

backup_database() {
    print_info "Backing up PostgreSQL database..."
    
    if ! docker ps | grep -q portalapi-postgres; then
        print_error "PostgreSQL container is not running"
        exit 1
    fi

    BACKUP_FILE="backup_$(date +%Y%m%d_%H%M%S).sql"
    docker-compose exec -T postgres pg_dump -U postgres PortalApiDb > "$BACKUP_FILE"
    
    print_info "Database backed up to: $BACKUP_FILE"
}

restore_database() {
    if [ -z "$1" ]; then
        print_error "Please specify backup file: ./docker-start.sh restore backup.sql"
        exit 1
    fi

    if [ ! -f "$1" ]; then
        print_error "Backup file not found: $1"
        exit 1
    fi

    print_info "Restoring PostgreSQL database from: $1"
    
    if ! docker ps | grep -q portalapi-postgres; then
        print_error "PostgreSQL container is not running"
        exit 1
    fi

    print_warn "This will overwrite the current database!"
    read -p "Continue? (y/N) " -n 1 -r
    echo
    if [[ $REPLY =~ ^[Yy]$ ]]; then
        docker-compose exec -T postgres psql -U postgres PortalApiDb < "$1"
        print_info "Database restored successfully!"
    fi
}

# Main script logic
check_docker

COMMAND=${1:-help}

case "$COMMAND" in
    dev)
        start_dev
        ;;
    prod)
        start_prod
        ;;
    stop)
        stop_services
        ;;
    restart)
        restart_services
        ;;
    logs)
        shift
        show_logs "$@"
        ;;
    build)
        build_images
        ;;
    clean)
        clean_all
        ;;
    migrate)
        run_migrations
        ;;
    backup)
        backup_database
        ;;
    restore)
        restore_database "$2"
        ;;
    help)
        show_help
        ;;
    *)
        print_error "Unknown command: $COMMAND"
        echo ""
        show_help
        exit 1
        ;;
esac
