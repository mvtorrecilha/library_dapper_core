FROM mcr.microsoft.com/mssql/server:2022-latest
RUN mkdir -p /opt/scripts
COPY sql/setup.sql /opt/scripts

ENV SA_PASSWORD=Pass@word
ENV ACCEPT_EULA=Y

EXPOSE 1433

RUN ( /opt/mssql/bin/sqlservr --accept-eula & ) | grep -q "Service Broker manager has started" \
    && /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P 'Pass@word' -i /opt/scripts/setup.sql \
    && pkill sqlservr