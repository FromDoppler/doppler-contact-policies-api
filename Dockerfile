FROM mcr.microsoft.com/dotnet/sdk:7.0 AS restore
WORKDIR /src
COPY ./*.sln ./
COPY */*.csproj ./
# Take into account using the same name for the folder and the .csproj and only one folder level
RUN for file in $(ls *.csproj); do mkdir -p ${file%.*}/ && mv $file ${file%.*}/; done
RUN dotnet restore

FROM restore AS build
COPY . .
RUN dotnet format --verify-no-changes
RUN dotnet build -c Release

FROM build AS test
RUN dotnet test

FROM build AS publish
RUN dotnet publish "./Doppler.ContactPolicies.Api/Doppler.ContactPolicies.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 80
COPY --from=publish /app/publish .
ARG version=unknown
RUN echo $version > /app/wwwroot/version.txt
ENTRYPOINT ["dotnet", "Doppler.ContactPolicies.Api.dll"]
