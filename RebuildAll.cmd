@REM This script tries to consecutively build all of the solutions specified below.
@REM
@REM In order to build and restore those solutions the following tools must be present:
@REM    NUGET
@REM    DEVENV
@REM
@REM You may override their location in a file called DevTools.ini next to this script file or in your %%LOCALAPPDATA%%/ChaosTechnology folder.
@REM The file next to this script file takes precedence and may override individual or all tools from the %%LOCALAPPDATA%%/ChaosTechnology folder.
@REM These files shall assign the path of the tool to the corresponding key as specified above.
@REM
@REM If no overrides are defined or the defined overrides do not exist the system's %%PATH%% is checked instead.
@REM
@REM Note: If you are using Visual Studio 14, you may want to use devenv.com instead of devenv.exe, as this one prints to STDOUT.

@ECHO OFF
SETLOCAL EnableDelayedExpansion

SET "SOLUTIONS[0]=./ChaosAnalyzers.sln"
SET "SOLUTIONS[1]=./ChaosBuild.sln"
SET "SOLUTIONS[2]=./ChaosFrameworkBuild.sln"
SET "SOLUTIONS[3]=./LD58.sln"
SET /A SOLUTIONS[]=3

CD "%~dp0"

SET ERR_TOOL_NOT_SPECIFIED=Consider specifying one in DevTools.ini as described in the header of this script.
SET ERR_NO_TOOL_OVERRIDE=Register the tool's location in the system's %%PATH%% or create a DevTools.ini file as described in the header of this script.

SET "TOOL_PATH="
CALL :FINDTOOL "DEVENV"
IF "%TOOL_PATH%"=="" GOTO :FAIL
SET "DEVENV_PATH=%TOOL_PATH%"
ECHO Using DEVENV from %DEVENV_PATH%

SET "TOOL_PATH="
CALL :FINDTOOL "NUGET"
IF "%TOOL_PATH%"=="" GOTO :FAIL
SET "NUGET_PATH=%TOOL_PATH%"
ECHO Using NUGET from %NUGET_PATH%

IF NOT '%ERRORLEVEL%'=='0' GOTO :FAIL

for /L %%i in (0,1,%SOLUTIONS[]%) DO (
    ECHO.
    ECHO #############################################################
    ECHO # Cleaning !SOLUTIONS[%%i]!
    ECHO #############################################################
    "%DEVENV_PATH%" "!SOLUTIONS[%%i]!" /Clean || GOTO FAIL
)

for /L %%i in (0,1,%SOLUTIONS[]%) DO (
    ECHO.
    ECHO #############################################################
    ECHO # Restoring !SOLUTIONS[%%i]!
    ECHO #############################################################
    "%NUGET_PATH%" restore "!SOLUTIONS[%%i]!" || (
        ECHO NUGET FAILED.
        ECHO   This may occur if the specified solution does not configure any NuGet packages.
        ECHO   Skipping solution.
    )
)

for /L %%i in (0,1,%SOLUTIONS[]%) DO (
    ECHO.
    ECHO #############################################################
    ECHO # Building !SOLUTIONS[%%i]!
    ECHO #############################################################
    "%DEVENV_PATH%" "!SOLUTIONS[%%i]!" /Build || GOTO FAIL
)

ECHO.
ECHO #############################################################
ECHO # COMPLETE
ECHO #############################################################
GOTO :EOF

:FAIL
    ECHO.
    ECHO #############################################################
    ECHO # FAILED
    ECHO #############################################################
    PAUSE
    GOTO :EOF

:FINDTOOL
    CALL :FINDTOOLINFILE "%~1" "./DevTools.ini"
    IF NOT "%TOOL_PATH%"=="" (
        ECHO Found fallback for "%~1" in project override
        GOTO :EOF
    )

    CALL :FINDTOOLINFILE "%~1" "%LocalAppData%/ChaosTechnology/DevTools.ini"
    IF NOT "%TOOL_PATH%"=="" (
        ECHO Found fallback for "%~1" in user override
        GOTO :EOF
    )

    FOR /F "tokens=* USEBACKQ" %%F IN (`WHERE /Q "%~1"`) DO SET "TOOL_PATH=%%F"
    IF NOT "%TOOL_PATH%"=="" (
        ECHO Found "%~1" in %%PATH%%
        GOTO :EOF
    )

    ECHO No fallback for "%~1" found. %ERR_TOOL_NOT_SPECIFIED%
    GOTO :EOF

:FINDTOOLINFILE
    FOR /f "USEBACKQ delims=" %%l in ("%~2") DO (
        FOR /f "tokens=1,2 delims==" %%b in ("%%l") DO (
            IF NOT '%%c' EQU '' (
                IF '%%b' EQU '%~1' (
                    ECHO Trying fallback for "%~1": "%%c"
                    IF EXIST "%%~c" (
                        SET "TOOL_PATH=%%~c"
                        GOTO :EOF
                    ) ELSE (
                        ECHO "%~1" fallback '%%~c' does not exist
                        GOTO :EOF
                    )
                )
            )
        )
    )
    GOTO :EOF
