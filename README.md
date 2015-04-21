# TransformHelper
Small tool helping with the usage of the [configuration transformations](https://msdn.microsoft.com/en-us/library/vstudio/dd465326(v=vs.100).aspx)

It has two modes:

* Add - passing solution, existing transformation and the new transformation the tool will go through all the projects in the solution and check for configuration files having the existing transformation and will add new transformation copying the files from the existing one
* Apply - passing source file, transformation file and optional target file (if not passed the source file content will be replaced)

# Usage

* Add mode (you may not specify the mode here as the Add mode is the default behavior)
```
TransformHelper.exe [-mode add] -solution C:\Work\MySolution.sln -existing Dev -new Live
```
This will check all the projects in the solution for files like **[fileName].Dev.config** and will add the corresponding **[fileName].Live.config** files copying the content from the existing ones
* Apply mode
```
TransformHelper.exe -mode apply -source "C:\Work\MySolution\WebProject\web.config" -transformFile "C:\Work\MySolution\WebProject\web.Dev.config" -target "C:\Temp\result.web.config"
```
This will apply the transformation file **C:\Work\MySolution\WebProject\web.Dev.config** on **C:\Work\MySolution\WebProject\web.config** and the result content will be saved in **C:\Temp\result.web.config** file
