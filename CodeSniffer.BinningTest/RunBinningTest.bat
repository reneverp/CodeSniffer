xcopy /Y /I ..\..\CodeSniffer.BBN\TrainingsData\MethodTrainingSet_2319_17022018.csv .\Methodtest.csv*
xcopy /Y /I ..\..\CodeSniffer.BBN\TrainingsData\ClassTrainingSet_2319_17022018.csv .\Classtest.csv*
call CodeSniffer.BinningTest.exe "CodeProjects\ganttproject-2.8.5\ganttproject\ganttproject\src\net\sourceforge\ganttproject"
