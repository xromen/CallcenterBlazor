## Настройка Connection strings

Для изменения подключения к бд на сервере необходимо настроить репозиторий: Settings -> CI/CD -> Variables -> DB_CONN -> Edit

Для установки/изменения локально необходимо в корне создать файл .env со следующим содержимым

```
DB_CONN=Host=<IP>;Database=<Database>;Username=<User>;Password=<password>
```