StealFocus Forecast
===================
"Forecast" is an application that will tear down or create cloud assets on a schedule. For example it can configured to delete a Windows Azure service at certain times on certain days. This is incredibly useful for automatically deleting test environments to minimise your Azure running costs.

It can be run as a console application or installed as a windows service.

Configuration
-------------
An example configuration file is included in the ZIP file available for download (see "Downloading" section below). Please see the "StealFocus.Forecast.exe.config" file.

Testing
-------
To test the application it is easiest to run in console mode first. From a command prompt simply run "StealFocus.Forecast.exe".

Installation
------------
To install the application as a windows service, run the following from a command prompt:

- StealFocus.Forecast.exe install -username:domain\user -password:myPassword
- StealFocus.Forecast.exe start

To uninstall the windows service run the following:

- StealFocus.Forecast.exe stop
- StealFocus.Forecast.exe uninstall

Downloading
-----------
You can download the application here: [http://build01.stealfocus.co.uk/Output/StealFocus.Forecast/](http://build01.stealfocus.co.uk/Output/StealFocus.Forecast/)

Help
----
You can see build status here: [http://build01.stealfocus.co.uk/ccnet/ViewFarmReport.aspx](http://build01.stealfocus.co.uk/ccnet/ViewFarmReport.aspx)

Contact the mailing list: <StealFocus-Forecast@yahoogroups.co.uk>
