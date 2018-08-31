# rdclpreview
This is a sample c# application to preview a stand alone Client Report Definition (.rdlc) File or a standalone SQL report using a dataset saved as an xml file.
This code is not maintained and is here purely for backup purposes as a sample project. Use it at your own risk.

The code will read the .rdlc file for defined datasets and import them from the data.xml file with the appropriate names expected by the report.

# Actions to be performed after clone to properly setup the solution
There is an issue with adding SqlServerTypes to git repo not adding dlls
https://github.com/FransBouma/RawDataAccessBencher/issues/37
on clone run the following to reinstall the nuget packages (you will first have to save the solution)

Update-Package -reinstall

# How to use it
Create a data file for a 408b2 and export it locally.
1) Run the program. 
2) You will be prompted to pick this data.xml file on load.
3) You will next be prompted for the report file. Choose the Report1.rdlc in the root of the project (or one you created)

It should preview the report.
