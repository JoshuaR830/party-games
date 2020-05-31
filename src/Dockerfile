FROM microsoft/dotnet:2.2-sdk AS build-env
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY "*Chat.csproj" ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Realease -o out

FROM build AS publish


FROM microsoft/dotnet:2.2-aspnetcore-runtime
WORKDIR /app
COPY --from=build-env /app/out .
CMD dotnet AspNetCoreHerokuDocker.dll