StealFocus Forecast
===================
"Forecast" is an application that will tear down or create cloud assets on a schedule. For example it can configured to delete a Windows Azure service at certain times on certain days. The principal purpose is reducing running costs, by proactively managing your deployed assets you can control the charges against your subscription(s).

Functionality includes:

- Delete Azure Hosted Services on a schedule. useful for deleting test environments when no longer needed e.g. outside of business hours.
- Create Azure Hosted Services on a schedule, useful for creating test environments as required e.g. ready for business hours.
- Delete Azure Hosted Services not on a "whitelist", useful for prevent organic grow or unplanned instance sprawl.
- Horizontally scale Azure Hosted Services in or out on a schedule, useful for ramping up capacity in time for predicted spikes in demand or winding down capacity to reduce running costs during known periods of low demand.
- Deleting Azure Storage artefacts (Blobs, Queues, Tables) on a schedule, useful for cleaning up Azure Storage accounts used for testing and the like.

It can be run as a console application or installed as a windows service.

Configuration
-------------
An example configuration file is included in the ZIP file available for download (see "Downloading" section below). Please see the "StealFocus.Forecast.exe.config" file.

Testing
-------
To test the application it is easiest to run in console mode first. From a command prompt simply run "StealFocus.Forecast.exe".

Installation
------------
To install and run the application as a windows service, run the following from a command prompt:

    StealFocus.Forecast.exe install -username:domain\user -password:myPassword
    StealFocus.Forecast.exe start

To uninstall the windows service run the following:

    StealFocus.Forecast.exe stop
    StealFocus.Forecast.exe uninstall

Downloading
-----------
You can download the application here: [http://build01.stealfocus.co.uk/Output/StealFocus.Forecast/](http://build01.stealfocus.co.uk/Output/StealFocus.Forecast/)

Help
----
Contact the mailing list:
- <StealFocus-Forecast@yahoogroups.co.uk>
- [http://uk.groups.yahoo.com/group/StealFocus-Forecast](http://uk.groups.yahoo.com/group/StealFocus-Forecast)

You can see build status here: [http://build01.stealfocus.co.uk/ccnet/ViewFarmReport.aspx](http://build01.stealfocus.co.uk/ccnet/ViewFarmReport.aspx)
