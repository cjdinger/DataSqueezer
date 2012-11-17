
/* Collect metrics and report */

proc contents data=&outLib..&outMem. 
  out=_afterCols noprint; 
run;

data _summaryCols;
merge _beforeCols(keep=name type length rename=(length=beforeLength type=varType))
	    _afterCols(keep=length rename=(length=afterLength));
format varType typeName.;
run;

data _summarySize(keep=filesizeBefore filesizeAfter);
  set _summarySize;
  merge sashelp.vtable(
		keep=filesize libname memname
		rename=(filesize=filesizeAfter)
		where=(libname="&outLib" and memname="&outMem"));
run;

title "Summary of changes in column length (&inLib..&inMem to &outLib..&outMem)";
proc print data=_summaryCols noobs;
run;

title "Summary of changes in file size (in bytes)";
proc print data=_summarySize noobs;
format filesizeBefore comma12. filesizeAfter comma12.;
run;
title;