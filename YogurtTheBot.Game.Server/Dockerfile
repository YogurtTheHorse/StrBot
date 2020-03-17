FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

COPY ./ /source
WORKDIR /source/YogurtTheBot.Game.Server
RUN dotnet publish -c Release -f netcoreapp3.1 -o /out

WORKDIR /
RUN rm -rf source && find /out -name "*.pdb" -type f -delete

FROM mcr.microsoft.com/dotnet/core/runtime:3.1 as runtime
WORKDIR /app
COPY --from=build /out ./
ENTRYPOINT ["dotnet", "YogurtTheBot.Game.Server.dll"]