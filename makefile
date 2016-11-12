Help:
	@echo "Run one of these commands:"
	@echo ""
	@echo "  make Help"
	@echo "  make Clean"
	@echo "  make All"
	@echo "  make CoreFire"
	@echo "  make CoreFireConsoleApp"

CoreFire:
	@cd src/CoreFire && dotnet restore && dotnet build

CoreFireConsoleApp:
	@cd src/CoreFireConsoleApp && dotnet restore && dotnet build

All: CoreFire CoreFireConsoleApp

Clean:
	@cd src/CoreFire && dotnet clean
	@cd src/CoreFireConsoleApp && dotnet clean

.PHONY: Help CoreFire CoreFireConsoleApp Clean All