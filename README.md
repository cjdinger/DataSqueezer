# SAS custom task example: Compress SAS data sets
***
This repository contains one of a series of examples that accompany
_Custom Tasks for SAS Enterprise Guide using Microsoft .NET_ 
by [Chris Hemedinger](http://support.sas.com/hemedinger).

This particular example goes with
**Chapter 13: Putting the Squeeze on Your SAS Data Sets**.  It was built using C# 
with Microsoft Visual Studio 2010.  It should run in SAS Enterprise Guide 4.3 and later.

## About this example
This task serves as an example of how to adapt 
a [proven SAS macro program](http://support.sas.com/kb/35/230.html)
into a useful task.  It requires a simple user interface with 
just a few options.  Most of the work within the task lies within the SAS program, 
which is embedded in the .NET assembly as a file resource.

For information about how to use the example in SAS Enterprise Guide, 
see this blog post: 
[Putting the squeeze on your SAS data sets](http://blogs.sas.com/content/sasdummy/2011/02/17/putting-the-squeeze-on-your-sas-data-sets/)