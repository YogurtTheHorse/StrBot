FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

COPY ./ /source
WORKDIR /source/YogurtTheBot.Alice
RUN dotnet publish -c Release -f netcoreapp3.1 -o /out

WORKDIR /
RUN rm -rf source && find /out -name "*.pdb" -type f -delete

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 as runtime
WORKDIR /app
COPY --from=build /out ./
ENTRYPOINT ["dotnet", "YogurtTheBot.Alice.dll"]