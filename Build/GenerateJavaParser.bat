@echo off

SET CURRENTDIR=%~dp0
SET ANTLRDIR=%CURRENTDIR%..\ThirdParty\Antlr

"%ANTLRDIR%\antlr4.bat" -Dlanguage=CSharp "%ANTLRDIR%\Grammars\Java.g4" -o "%CURRENTDIR%..\CodeSniffer.JavaParser\Generated_Parser" -no-visitor