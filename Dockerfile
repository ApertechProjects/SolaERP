FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

COPY ./SolaERP.DataAccess/SolaERP.DataAccess.csproj ./SolaERP.DataAccess/SolaERP.DataAccess.csproj
COPY ./SolaERP.Application/SolaERP.Persistence.csproj ./SolaERP.Application/SolaERP.Persistence.csproj
COPY ./SolaERP.Infrastructure/SolaERP.Application.csproj ./SolaERP.Infrastructure/SolaERP.Application.csproj
COPY ./Sola.ERP.Infrastructure/SolaERP.Infrastructure.csproj ./Sola.ERP.Infrastructure/SolaERP.Infrastructure.csproj
COPY ./SolaERP/SolaERP.API.csproj ./SolaERP/SolaERP.API.csproj

RUN dotnet restore "./SolaERP/SolaERP.API.csproj"

COPY . ./

ENV DOTNET_HOSTBUILDER__RELOADCONFIGONCHANGE=false

RUN dotnet publish -c Release -o output
RUN rm -r /app/output/wwwroot
COPY ./SolaERP/wwwroot /app/output/wwwroot

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/output .
ENTRYPOINT ["dotnet", "SolaERP.API.dll"]