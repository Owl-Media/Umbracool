#!/bin/bash
set -euo pipefail

database_state="$(
  /opt/mssql-tools18/bin/sqlcmd \
    -S localhost \
    -U sa \
    -P "$SQL_SA_PASSWORD" \
    -d master \
    -Q "SET NOCOUNT ON; SELECT state_desc FROM sys.databases WHERE name = N'$UMBRACO_DB_NAME'" \
    -h -1 \
    -W \
    -C \
    | tr -d '\r'
)"

if [ "$database_state" = "ONLINE" ]; then
  echo "ONLINE"
  exit 0
fi

echo "OFFLINE"
exit 1
