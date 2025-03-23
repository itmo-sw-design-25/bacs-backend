# BaCS

> Данный репозиторий содержит в себе исходный код сервиса BaCS (Booking a Coworking Space)

## Запуск BaCS API

### С помощью compose

Для локального запуска BaCS API с использованием compose, необходимо:

1. Заполнить значения переменных окружения в файле [.env](./.env)

2. Запустить compose файл _из корня проекта_, используя следующую команду

    ```shell
    docker compose -f docker-compose.override.yaml -f docker-compose.yaml -p bacs up -d --force-recreate
    ```

   > `docker-compose.yaml` содержит основную конфигурацию контейнеров, `docker-compose.override.yaml` - открывает порты
   > всех сервисов для локальной разработки

### Без compose

При необходимости запустить BaCS API локально без compose, необходимо:

> Иметь запущенный инстанс PostgreSQL БД, при необходимости поменяв настройки подключения в
> файле [appsettings.Development.json](Source/Presentation/BaCS.Presentation.API/appsettings.Development.json)

1. Установить **SDK** `.NET 9`
2. Выполнить следующие команды:

    ```shell
    cd Source/Presentation/BaCS.Presentation.API
    dotnet restore
    dotnet build -tl --no-restore
    dotnet run
    ```

## Сборка образа BaCS API

Для сборки образа BaCS API необходимо выполнить следующую команду _из корня проекта_:

```shell
docker build --platform linux/amd64 -t lipa44/bacs.api:amd . -f Source/Presentation/BaCS.Presentation.API/Dockerfile
```

> Сборка в примере осуществляется под платформу amd, тк это платформа продакшен-сервера.
