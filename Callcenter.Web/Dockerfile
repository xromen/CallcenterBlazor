# Этап сборки
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

ENV TZ=Asia/Vladivostok
ENV LANG=ru_RU.UTF-8
ENV LANGUAGE=${LANG}
ENV LC_ALL=${LANG}

# копируем csproj
COPY ["Callcenter.Web/Callcenter.Web.csproj", "Callcenter.Web/"]
COPY ["Callcenter.Shared/Callcenter.Shared.csproj", "Callcenter.Shared/"]
RUN dotnet restore "Callcenter.Web/Callcenter.Web.csproj"

# копируем все остальные файлы и публикуем
COPY . .
WORKDIR "/src/Callcenter.Web"
RUN dotnet publish -c Release -o /app/publish

# Этап запуска — Nginx
FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html

# копируем артефакты билда
COPY --from=build /app/publish/wwwroot .

# заменим дефолтный конфиг nginx, чтобы SPA маршрутизация работала
COPY Callcenter.Web/nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]