# App Layer (User Interface (Web/Api) Layer)

In this project we store components connected to :

- Registering services
- Middlewares
- Options
- Settings

## Quartz background jobs

To configure Quartz background jobs we need Quartz.Extensions.Hosting NuGet Package. This integrates with background service in ASP.NET.

## Spin the Docker Container with Redis

Create and run the docker container from the redis images in the detached mode

```cmd
docker run -p 6379:6379 --name redis -d redis
```