# TransformHelper
Small tool automating addition of config transformations copying files from existing transformation

# Usage
```
TransformHelper.exe -solution C:\Work\MySolution -existing Dev -new Live
```
This will check all the projects in the solution for files like <fileName>.Dev.config and will add the corresponding <fileName>.Live.config files copying the content from the existing ones
