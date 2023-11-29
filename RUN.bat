set backendPath=%CD%\RTD_Temperature_Controller_DotnetAPI\RTD_Temperature_Controller_DotnetAPI

set frontendPath=%CD%\RTD_Temperature_Controller_Angular


start "Backend" cmd /k "cd %backendPath% && dotnet build && dotnet run --urls=https://localhost:3000/"

rem Pause to give the backend time to start before launching the frontend
timeout /t 5

rem Open a new command prompt window for the Angular front end
start "Frontend" cmd /k "cd %frontendPath% && npm install && ng serve"

pause