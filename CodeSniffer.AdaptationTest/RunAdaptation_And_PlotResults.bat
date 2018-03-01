xcopy /Y /I ..\..\CodeSniffer.BBN\VerificationData\MethodTrainingSet_1357_18022018.csv .\Methodtest.csv*
xcopy /Y /I ..\..\CodeSniffer.BBN\VerificationData\ClassTrainingSet_1357_18022018.csv .\Classtest.csv*
call CodeSniffer.AdaptationTest.exe
call python ..\..\PythonScripts\plot_annotations.py Me
call python ..\..\PythonScripts\plot_roc.py RocPlotsMe

call GenerateDatasetUser1.bat
call CodeSniffer.AdaptationTest.exe
call python ..\..\PythonScripts\plot_annotations.py User1
call python ..\..\PythonScripts\plot_roc.py RocPlotsUser1

call GenerateDatasetUser2.bat
call CodeSniffer.AdaptationTest.exe
call python ..\..\PythonScripts\plot_annotations.py User2
call python ..\..\PythonScripts\plot_roc.py RocPlotsUser2

call GenerateDatasetUser3.bat
call CodeSniffer.AdaptationTest.exe
call python ..\..\PythonScripts\plot_annotations.py User3
call python ..\..\PythonScripts\plot_roc.py RocPlotsUser3