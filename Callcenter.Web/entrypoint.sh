﻿#!/bin/sh
set -e

sed -i "s#__API_URL__#${API_URL}#g" /usr/share/nginx/html/appsettings.json

exec "$@"