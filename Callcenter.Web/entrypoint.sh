#!/bin/sh

# Заменяем __API_URL__ на значение переменной окружения
if [ -n "$API_URL" ]; then
    sed -i "s#__API_URL__#$API_URL#g" /usr/share/nginx/html/appsettings.json
else
    echo "WARNING: API_URL environment variable is not set"
fi

exec "$@"