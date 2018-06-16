call rmdir /s /q RocPlotsUser1
call rmdir /s /q RocPlotsUser2
call rmdir /s /q RocPlotsUser3

call rmdir /s /q annotations

REM ----- Uncomment the below lines to compare JUnit with the annotated verificationset -----
REM xcopy /Y /I ..\..\CodeSniffer.BBN\VerificationData\MethodTrainingSet_1357_18022018.csv .\Methodtest.csv*
REM xcopy /Y /I ..\..\CodeSniffer.BBN\VerificationData\ClassTrainingSet_1357_18022018.csv .\Classtest.csv*
REM call CodeSniffer.AdaptationTest.exe "CodeProjects\junit-4.12\junit4\src\main"
REM call python ..\..\PythonScripts\plot_annotations.py Me
REM call python ..\..\PythonScripts\plot_roc.py RocPlotsMe
REM --------------------------------------------------------------------------------------------

call GenerateDatasetUser1.bat %1
call CodeSniffer.AdaptationTest.exe %1
call python ..\..\PythonScripts\plot_annotations.py User1
call python ..\..\PythonScripts\plot_roc.py RocPlotsUser1

call GenerateDatasetUser2.bat %1
call CodeSniffer.AdaptationTest.exe %1
call python ..\..\PythonScripts\plot_annotations.py User2
call python ..\..\PythonScripts\plot_roc.py RocPlotsUser2

call GenerateDatasetUser3.bat %1
call CodeSniffer.AdaptationTest.exe %1
call python ..\..\PythonScripts\plot_annotations.py User3
call python ..\..\PythonScripts\plot_roc.py RocPlotsUser3