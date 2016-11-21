# `CoreFire`

### A Firebase client library written for .NET Core

```csharp
using CoreFire;

var uri = new Uri("https://mydb.firebaseio.com/");
var client = Client.Builder()
    .WithUri(uri)
    .Build();

client.PushSync("/now", DateTime.UtcNow);
```

## NuGet

https://www.nuget.org/packages/CoreFire/0.1.0-prerelease1

`Install-Package CoreFire -Pre`

## Development

You will need:

* .NET Core (https://www.microsoft.com/net/core)
* dotnet-cli (https://github.com/dotnet/cli/) preview3 or later

## Building

On a system with `make` you can run `make All`.

Otherwise you can run `dotnet restore && dotnet build` in `src/CoreFire`.

Read the makefile for more information.

## TODO

* Fix CopyLocal/Private in csproj to copy deps to output directory
  * This has been worked around via an AfterBuild Target in `CoreFireConsoleApp.csproj`

* Publish a `0.1.0` version once `Microsoft.NET.Sdk` is no longer `-alpha`

* Write and auto-publish docs

* Examples

* Auth

* Storage

## LICENSE

MIT

## Other

Please review `CLA.md` before making contributions.