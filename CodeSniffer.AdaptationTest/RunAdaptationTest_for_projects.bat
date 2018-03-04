
call RunAdaptation_And_PlotResults.bat "CodeProjects\spring-boot-v2.0.0.RELEASE\spring-boot\spring-boot-project\spring-boot\src\main\java\org\springframework\boot"
xcopy /E /Y /I RocPlotsUser1 RocPlotsUser1_spring
xcopy /E /Y /I RocPlotsUser2 RocPlotsUser2_spring
xcopy /E /Y /I RocPlotsUser3 RocPlotsUser3_spring
xcopy /E /Y /I annotations annotations_spring
xcopy /Y /I Class*.csv data_spring
xcopy /Y /I Method*.csv data_spring


call RunAdaptation_And_PlotResults.bat "CodeProjects\junit-4.12\junit4\src\main"
xcopy /E /Y /I RocPlotsUser1 RocPlotsUser1_junit
xcopy /E /Y /I RocPlotsUser2 RocPlotsUser2_junit
xcopy /E /Y /I RocPlotsUser3 RocPlotsUser3_junit
xcopy /E /Y /I annotations annotations_junit
xcopy /Y /I Class*.csv data_junit
xcopy /Y /I Method*.csv data_junit

call RunAdaptation_And_PlotResults.bat "CodeProjects\ganttproject-2.8.5\ganttproject\ganttproject\src\net\sourceforge\ganttproject"
xcopy /E /Y /I RocPlotsUser1 RocPlotsUser1_ganttproject
xcopy /E /Y /I RocPlotsUser2 RocPlotsUser2_ganttproject
xcopy /E /Y /I RocPlotsUser3 RocPlotsUser3_ganttproject
xcopy /E /Y /I annotations annotations_ganttproject
xcopy /Y /I Class*.csv data_ganttproject
xcopy /Y /I Method*.csv data_ganttproject