# `CoreFire`

### A Firebase client library written for .NET Core

This section to be filled out with examples and documentation links.

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

* Write docs and examples

* Auth

* Storage

## LICENSE

MIT

## Other

Please review `CLA.md` before making contributions.