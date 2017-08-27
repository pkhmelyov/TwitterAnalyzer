# TwitterAnalyzer
Веб-приложение, подсчитывающее для пользователя сети “Twitter” зависимость между временем публикации твитов и количеством “лайков”, которое они получили.

Чтобы запустить приложение, нужно в локальном IIS создать веб-сайт `http://twitteranalyzer.local/`
и нацелить его на папку с проектом `TwitterAnalyzer/TwitterAnalyzer.WebUI` (не забудьте про права на доступ для IIS к этой папке).

Кроме того, нужно создать в в папке `TwitterAnalyzer/TwitterAnalyzer.WebUI` два файла с подходящими значениями:
1. `ApplicationSettings.config`

```xml
<appSettings>
    <add key="consumerKey" value="" />
    <add key="consumerSecret" value="" />
</appSettings>
```
2. `ConnectionStrings.config`

```xml
<connectionStrings>
    <add name="TwitterAnalyzerConnection"
        providerName="System.Data.SqlClient"
        connectionString="" />
</connectionStrings>
```
