FROM microsoft/dotnet:sdk AS build-env
WORKDIR /app

# Copy everything and build
COPY . ./

RUN dotnet restore "./order.api.csproj"
RUN dotnet publish "order.api.csproj" -c Release -o out

FROM microsoft/dotnet:aspnetcore-runtime
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "order.api.dll"]