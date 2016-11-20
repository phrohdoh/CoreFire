Help:
	@echo "Run one of these commands:"
	@echo ""
	@echo "  make Help"
	@echo "  make Clean"
	@echo "  make All"
	@echo "  make CoreFire"
	@echo "  make CoreFireConsoleApp"

Clean:
	@cd src/CoreFire && dotnet clean
	@cd src/CoreFireConsoleApp && dotnet clean

All: CoreFire CoreFireConsoleApp

CoreFire:
	@cd src/CoreFire && dotnet restore --verbosity=quiet /nologo && dotnet build --verbosity=quiet /nologo

CoreFireConsoleApp:
	@cd src/CoreFireConsoleApp && dotnet restore --verbosity=quiet /nologo && dotnet build --verbosity=quiet /nologo

FixMod:
	@find . -type f -exec chmod u-x {} \;
	@find . -type f -exec chmod  -x {} \;

.PHONY: Help Clean All CoreFire CoreFireConsoleApp