# Source Generator :musical_keyboard:

This source generator is dedicated for Shopway application. It is configured to generate strongly typed ids in the form that Shopway requires. 

Nevertheless, feel free to use or/and modify the code for Your own purpose. The IncrementalGeneratorBase and some utilities should be generic enough to be highly
reusable.

# How to use?

The default version is already packed to nuget, but if some updates are required we need to:
1. Increment the version **Version** in project file
2. Use command ```dotnet pack -c Release -o ./..``` at Shopway.SourceGenerator directory. You can specify other output directory and than copy it to SourceGenerators folder.

**IMPORTANT**: remember to use ```-c Release```.

In Shopway.Domain 

Example of auto-generated entity id: