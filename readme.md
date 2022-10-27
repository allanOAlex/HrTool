Code coverage

root directory of project 
install tool-manifest 
    dotnet new tool-manifest --force
install Cake
    dotnet tool install Cake.Tool
run 
    dotnet cake run_tests_and_generate_coverage_report.cake 
