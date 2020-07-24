#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.


FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["CheckThisOut.csproj", "CheckThisOut/"]
RUN dotnet restore "CheckThisOut/CheckThisOut.csproj"
COPY . .
WORKDIR "/src/CheckThisOut"
RUN dotnet build "CheckThisOut.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CheckThisOut.csproj" -c Release -o /app/publish

FROM  mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS final
WORKDIR /app
COPY --from=publish /app/publish .
WORKDIR /app
EXPOSE 443
ENTRYPOINT ["dotnet", "CheckThisOut.dll"]