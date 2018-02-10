@echo off
rem echo Test this.
rem This should already be running in the
rem working directory this batch file is in.
rem cd c:\Eric\ClimateModel

rem Property values on the command line override
rem values set in the project file.
rem c:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe  /nologo /p:Configuration=Release /p:DebugSymbols=false /p:DefineDebug=false /p:DefineTrace=false /fileLogger /verbosity:normal CodeAnalysis2.csproj
c:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe  /nologo /fileLogger CodeAnalysis2.csproj

rem Help:
rem c:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe  /help

pause

