# ModerationService

ModerationService — микросервис для автоматической проверки контента. Слушает очередь RabbitMQ, модерирует посты с помощью библиотеки BogaNet.BWF и возвращает результат.

> **Связанные репозитории:**
> - [BackendService](https://github.com/ElizavetaLevina/BackendService) — основной API для работы с постами
> - [Shared](https://github.com/ElizavetaLevina/Shared) — общие DTO и Enum для RabbitMQ

## Документация

### Быстрый старт
Сервис подключается к RabbitMQ и ожидает сообщения с постами для модерации.

### Доступ к сервисам

| Сервис | Адрес |
|--------|-------|
| RabbitMQ Management | `http://localhost:15672` |

## Основные возможности

- **Модерация текста** — проверка постов на запрещённые слова через BogaNet.BWF
- **Асинхронное взаимодействие** — получение сообщений из RabbitMQ
- **Ответ сервису-отправителю** — отправка результата модерации обратно в очередь

## Используемые технологии

C# / .NET 8 / RabbitMQ (MassTransit) / BogaNet.BWF / Docker