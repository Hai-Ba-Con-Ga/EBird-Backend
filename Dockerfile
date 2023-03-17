FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app
COPY EBird-Backend.sln ./
COPY . ./
RUN dotnet publish -c Release -o dist


FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /app/dist ./
EXPOSE 80

ENTRYPOINT ["dotnet", "EBird.Api.dll"]
EXPOSE 7137
