FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app
COPY *.csproj ./
RUN dotnet restore
COPY . ./
RUN dotnet publish -c Release -o dist


FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /app/dist ./
EXPOSE 7137

ENTRYPOINT ["dotnet", "EBird.Api.dll"]
EXPOSE 7001
