# rdclpreview
c# application to preview a SQL report local RDCL file using a dataset xml file
This code is not maintained and is here purely for backup purposes as a sample project

There is an issue with adding SqlServerTypes to git repo not adding dlls
https://github.com/FransBouma/RawDataAccessBencher/issues/37
on clone run the following to reinstall the nuget packages
Update-Package -reinstall

Create a data file for a 408b2 and export it locally
Run the program. You will be prompted to pick this data.xml file on load.
When prompted for the report file choose the Report1.rdlc in the root of the project (or one you created)
It should preview the report