# SQMGagagu
A small addin for Arma2Net to Import and Export SQM Mission files over arma script.

Official web site: [www.gagagu.de](http://www.gagagu.de)

## Features: 

- create (Export) and read (import) Arma Mission files over script


## Installation & Config
1) Install Arma2Net (with .net dependencies). See details on Arma2Net Homepage
2) copy all files/folders from @Arma2NET directory to <your arma mod directory>\@Arma2NET
3) copy all files/folders from missions directory to your missions directory in your arma3 user profile (the missions folder where your custom mission files (from editor) will be saved)
4) open the SQMExport.sqf file with an Text Editor (like Notepad++, set language to c#) and change the line
"Arma2Net.Unmanaged" callExtension "SQMExporter [InitExport, Stratis, SQMExport, gagagu]";
to your Arma profile name "Arma2Net.Unmanaged" callExtension "SQMExporter [InitExport, Stratis, SQMExport, <arma profile name>]";
5) save your changes.
6) Start the Arma 3 Editor and load the SQMGagagu (Stratis) mission
7) Preview it, use the middle mouse button and select the export function.
8) A New Mission called "SQMExport" (Stratis) will be created.
9) Preview it, use the middle mouse button and select the import function.

## Documentation
Open the SQMExport.sqf for detailed description

## Remarks
- It an Beta Version so errors are possible.
- I'm not responsible for data loose. Please backup your data !!!

## Special Thanks to
- HeliJunkie, for supporting me
- Scott_NZ for the great Arma2Net

## Licensing
 
MIT
