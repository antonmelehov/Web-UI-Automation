## Reporting

The project uses:
- Serilog for file logging
- Shouldly for assertions
- Allure for test reporting

### Run tests
dotnet test Ehu.UiTests/Ehu.UiTests.csproj

### Generate Allure report
cd Ehu.UiTests
allure generate -o allure-report

allure open allure-report

### Attached artifact
A generated sample report is included in:
- artifacts/allure-report.zip