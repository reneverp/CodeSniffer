call GenerateDatasetUser1.bat
call CodeSniffer.AdaptationTest.exe
call python ..\..\PythonScripts\plot_roc.py RocPlotsUser1

call GenerateDatasetUser2.bat
call CodeSniffer.AdaptationTest.exe
call python ..\..\PythonScripts\plot_roc.py RocPlotsUser2

call GenerateDatasetUser3.bat
call CodeSniffer.AdaptationTest.exe
call python ..\..\PythonScripts\plot_roc.py RocPlotsUser3