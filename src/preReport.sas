
/* Collect metrics and report */
proc format lib=work;
 value typeName 
  1 = "Numeric"
  2 = "Character";
run;

proc contents data=&inLib..&inMem. 
  out=_beforeCols noprint; 
run;

data _summarySize;
  set sashelp.vtable(
		keep=filesize libname memname
		rename=(filesize=filesizeBefore)
		where=(libname="&inLib" and memname="&inMem"));
run;

