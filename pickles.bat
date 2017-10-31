.\packages\NUnit.ConsoleRunner.3.6.1\tools\nunit3-console.exe .\src\bin\Debug\SeleniumAndSpecflow.dll

.\packages\Pickles.CommandLine.2.16.2\tools\pickles.exe^
	--feature-directory=.\src^
	--output-directory=.\documentation^
	--link-results-file=.\TestResult.xml^
	--documentation-format=dhtml^
	--test-results-format=nunit3