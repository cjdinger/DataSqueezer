The "Data Squeezer" task
----------------------------------
Purpose:
 To modify the column attributes of a SAS data set to reduce the
 LENGTH of character variables to just the size that is needed to fit the
 current data values.
 
 This can reduce the record length for each observation, and thus reduce
 the overall size of the data set file.
 
 Also, the task offers an option to COMPRESS the data set by using one
 of the available compression algorithms.

 This is adapted from:
 http://support.sas.com/kb/35/230.html  
 Sample 35230: Shrinking character variables to minimum required length
 
Installing the Custom Task
----------------------------------------
The custom task is in this assembly (DLL): SAS.Tasks.DataSqueezer.dll. 

To install the custom task for use with SAS Enterprise Guide 4.3 or later, 
you simply need to copy the DLL to a designated directory:

- For use by just the current user, copy the DLL to: 
     %appdata%\SAS\EnterpriseGuide\4.3\Custom 
	 or
	 %appdata%\SAS\EnterpriseGuide\5.1\Custom 
  where %appdata% is a Windows environment variable that resolves to your profile area. 
  You might need to create the “Custom” subfolder in this area.

- For use by all users on a machine, copy the DLL to: 
  %programfiles%\SAS\EnterpriseGuide\4.3\Custom 
  or
  %programfiles%\SAS\EnterpriseGuide\5.1\Custom 
  You might need to create the “Custom” subfolder in this area. 
  You might need elevated privileges on your machine to copy content into the Program Files area.

After the file is copied into place, restart SAS Enterprise Guide. 
The task should be available from the Tools->Add-In menu, as "Squeeze SAS Data"