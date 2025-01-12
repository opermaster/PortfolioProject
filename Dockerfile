# Используем официальный образ .NET для запуска приложения
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Используем SDK-образ для сборки проекта
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем файлы проекта
COPY . .

# Восстанавливаем зависимости
RUN dotnet restore

# Публикуем приложение в папку /app
RUN dotnet publish -c Release -o /app

# Переносим собранное приложение в финальный образ
FROM base AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "PortfolioProject.dll"]
