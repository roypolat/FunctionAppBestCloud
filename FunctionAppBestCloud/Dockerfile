#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/azure-functions/dotnet:4 AS base  #hangi docker image’i kullanılacağını belirttiğim komut.
WORKDIR /home/site/wwwroot # mage içerisinde working directory olarak kullanacağımız yeri belirttiğimikomut
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FunctionAppBestCloud/FunctionAppBestCloud.csproj", "FunctionAppBestCloud/"]# proje dosyalarını local file system’dan image’e kopyalamak için kullanılan komut.
RUN dotnet restore "FunctionAppBestCloud/FunctionAppBestCloud.csproj"
COPY . .
WORKDIR "/src/FunctionAppBestCloud"
RUN dotnet build "FunctionAppBestCloud.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FunctionAppBestCloud.csproj" -c Release -o /app/publish #Uygulamayı yayımlamak için

FROM base AS final
WORKDIR /home/site/wwwroot
COPY --from=publish /app/publish .
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true
ENTRYPOINT ["dotnet", "FunctionAppBestCloud.dll"]#ontainer ayağa kaldırılırken ilk olarak çalışacak olan komut ve parametreleri belirttiğimiz komuttur. Container run edilirken dotnet komutuyla
