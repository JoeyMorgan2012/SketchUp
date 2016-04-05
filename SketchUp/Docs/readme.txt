IBM Data Server EF 6 Provider for Entity Framework 6 applications accessing IBM Data Servers
============================================================================================

As part of this release, IBM Data Server EF 6 Provider will be shipped via a NuGet package, EntityFramework.IBM.DB2 (6.0.3) available on nuget.org.
This package, EntityFramework.IBM.DB2 - is built on MS EF 6.0.0.

Prerequisites:
--------------
1. Download Entity Framework 6 Tools for Visual Studio 2012 & 2013:
   http://www.microsoft.com/en-us/download/details.aspx?id=40762

2. Download DB2 Version 10.5 Fix Pack 5 clients/drivers for Windows:
   http://www-01.ibm.com/support/docview.wss?uid=swg24038828

Limitations:
------------
1. A known issue with MS EF Tooling, if an application targets the x64 platform using our 'IBM Data Server EF 6 Provider' during EDM creation,
   It will see "Your project references the latest version of Entity Framework; however, an Entity Framework database provider compatible with
   this version could not be found for your data connection. If you have already installed a compatible provider, ensure you have rebuilt your project
   before performing this action. Otherwise, exit this wizard, install a compatible provider, and rebuild your project before performing this action" exception.
   
   The following link details about this issue:
   https://entityframework.codeplex.com/workitem/2506
   
   Possible Workarounds:
   i) Move the EF model into its own project that compiles as Any CPU, then add that project as a dependency of the x64 (64-bit) StartUp project.
  ii) Target the application to Any CPU platform.
 
Assistance:
-----------
For questions about using IBM Data Server EF 6 Provider, please post a question on .NET Development with DB2 and IDS forum:
https://www.ibm.com/developerworks/community/forums/html/forum?id=11111111-0000-0000-0000-000000000467  