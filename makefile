Help:
	@echo "Run one of these commands:"
	@echo ""
	@echo "  make Help"
	@echo "  make CoreFire"
	@echo "  make CoreFireConsoleApp"
	@echo "  make Clean"

CoreFire:
	@cd src/CoreFire && dotnet restore && dotnet build

CoreFireConsoleApp:
	@cd src/CoreFireConsoleApp && dotnet restore && dotnet build

Clean:
	@cd src/CoreFire && dotnet clean
	@cd src/CoreFireConsoleApp && dotnet clean

.PHONY: Help
