FROM mcr.microsoft.com/mssql/server:2022-latest

ENV ACCEPT_EULA=Y

USER root

COPY setup.sql /setup.sql
COPY startup.sh /startup.sh
COPY healthcheck.sh /healthcheck.sh

RUN chmod +x /startup.sh /healthcheck.sh

ENTRYPOINT ["/bin/bash", "/startup.sh"]
CMD ["/opt/mssql/bin/sqlservr"]
