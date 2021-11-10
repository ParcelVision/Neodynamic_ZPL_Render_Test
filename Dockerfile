FROM mcr.microsoft.com/dotnet/sdk:5.0

WORKDIR /app
COPY . .

RUN dotnet restore /app
RUN dotnet test ZplRenderingTests

ENTRYPOINT ["dotnet", "test", "ZplRenderingTests"]
