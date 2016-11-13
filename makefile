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
	@cd src/CoreFire && dotnet restore && dotnet build

CoreFireConsoleApp:
	@cd src/CoreFireConsoleApp && dotnet restore && dotnet build

.PHONY: Help Clean All CoreFire CoreFireConsoleApp