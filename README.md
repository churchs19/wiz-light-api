# WiZ Light API

API interface to connect Wiz Lights with Presence Light

## Getting Started

Built with .NET 6.00

- Clone the repo and then run `dotnet restore`. 
- Add your WiZ Home ID to the user secrets with `dotnet user-secrets add WizConfiguration:WizHome <HOME_ID>`.
- Update .\WizLightApi.Service\appsettings.json to add the IP address and MAC addresses of the lights you wish to control.
- To run the app run `dotnet run .\WizLightApi.Service\WizLightApi.Service.csproj`

Swagger documentation will be at https://localhost:7119/swagger/
