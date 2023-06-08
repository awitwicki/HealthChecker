# HealthChecker
Simple docker service to check url availability and sends alert to telegram if it's unavailable (response HTTP code is not 200x)


## Configure

Use next environment variables:

* `URL={YOUR_URL}` - URL to check
* `TELEGRAM_TOEN={YOUR_TOKEN}` - telegram bot token
* `CRON_SCHEDULE={YOUR_TOKEN}` - 0 * * * * - every hour
* `CHAT_ID={SPECIFIED_CHAT_ID}` - telegram chat id to send logs


1. Create `.env` file and fill it with that variables.

2. Run command `docker-compose up -d` this will start building and run service in docker.

