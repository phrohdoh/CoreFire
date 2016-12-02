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

## Supporting this project

If you would like to financially support this project please do the following:
* [Become a patron](https://www.patreon.com/Phrohdoh)
* [Tip me on gratipay](https://gratipay.com/~Phrohdoh/)
* [E-mail me](mailto:taryn@phrohdoh.com) for one-time donation information

## Other

Please review `CLA.md` before making contributions.

This CLA is similar to those that Google, the Apache Foundation, Dropbox,

and many others require contributors to sign before accepting contributions.

The purpose of the CLA is to ensure that the project author may use the

resulting works in whatever way they believe most benefits the project.
