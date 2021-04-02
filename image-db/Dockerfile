FROM mcr.microsoft.com/mssql/server:2019-latest as build

ARG SA_PASSWORD
ENV ACCEPT_EULA=Y

WORKDIR /tmp
COPY ./build.sql ./

USER mssql

RUN ( /opt/mssql/bin/sqlservr & ) | grep -q "Service Broker manager has started" \
    && /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P $SA_PASSWORD -i /tmp/build.sql \
    && pkill sqlservr

FROM mcr.microsoft.com/mssql/server:2019-latest

COPY --from=build /var/opt/mssql/data /var/opt/mssql/data