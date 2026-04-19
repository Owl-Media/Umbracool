#!/bin/bash
set -euo pipefail

initialize_database() {
  echo "Waiting for SQL Server to accept connections..."

  for _ in {1..60}; do
    if /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$SQL_SA_PASSWORD" -d master -Q "SELECT 1" -C >/dev/null 2>&1; then
      echo "Ensuring database $UMBRACO_DB_NAME exists..."
      /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$SQL_SA_PASSWORD" -d master -i /setup.sql -v DB_NAME="$UMBRACO_DB_NAME" -C
      echo "Database ready."
      return 0
    fi

    sleep 2
  done

  echo "SQL Server did not become ready in time."
  return 1
}

if [ "${1:-}" = "/opt/mssql/bin/sqlservr" ]; then
  initialize_database &
fi

exec "$@"
