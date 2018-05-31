FROM microsoft/aspnetcore:2.0
ARG source
WORKDIR /app
EXPOSE 44315
COPY publish .
ENTRYPOINT ["dotnet", "NetCoreIdentity.dll"]
