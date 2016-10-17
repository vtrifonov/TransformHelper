# TransformHelper
Small tool helping with the usage of the [configuration transformations](https://msdn.microsoft.com/en-us/library/vstudio/dd465326(v=vs.100).aspx)

It has two modes:

* **Add** - passing solution, existing transformation and the new transformation the tool will go through all the projects in the solution and check for configuration files having the existing transformation and will add new transformation copying the files from the existing one
* **Remove** - passing solution, and existing transformation the tool will go through all the projects in the solution and check for configuration files having the existing transformation and will delete them
* **Apply** - passing source file, transformation file and optional target file (if not passed the source file content will be replaced)
* **ApplySLN** - passing solution, and existing transformation the tool will go through all the projects in the solution and apply the transformation on the original file
* **WarnAsErrors** - although enabling TreatWarningsAsErrors is not connected to the Transformations I've added it in the tool as it was hepful, this will enable this settings for all the projects in a given solution

# Usage

* **Add** mode (you may not specify the mode here as the Add mode is the default behavior)
```
TransformHelper.exe [-mode add] -solution C:\Work\MySolution.sln -existing Dev -new Live
```
This will check all the projects in the solution for files like **[fileName].Dev.config** and will add the corresponding **[fileName].Live.config** files copying the content from the existing ones
* **Remove** mode
```
TransformHelper.exe -mode remove -solution C:\Work\MySolution.sln -existing Dev
```
This will check all the projects in the solution for files like **[fileName].Dev.config** and will delete them and remove them from the project
* **Apply** mode
```
TransformHelper.exe -mode apply -source "C:\Work\MySolution\WebProject\web.config" -transformFile "C:\Work\MySolution\WebProject\web.Dev.config" -target "C:\Temp\result.web.config"
```
This will apply the transformation file **C:\Work\MySolution\WebProject\web.Dev.config** on **C:\Work\MySolution\WebProject\web.config** and the result content will be saved in **C:\Temp\result.web.config** file
* **ApplySLN** mode
```
TransformHelper.exe -mode applySLN -solution C:\Work\MySolution.sln -existing Dev
```
This will check all the projects in the solution for files like **[fileName].Dev.config** and if there are files like **[fileName].config** as well in the project they will be transformed using the Dev transformations
* **WarnAsErrors** mode
```
TransformHelper.exe -mode WarnAsErrors -solution C:\Work\MySolution.sln
```
This will enable TreatWarningsAsErrors for all the projects in the solution
