@echo off

setlocal enabledelayedexpansion

set protopath=proto
set outputpath=output

set protofilelist=

for /r %protopath% %%s in (*.proto) do (
	rem echo %%~nxs
	
	set protofilelist=!protofilelist!-i:!protopath!\%%~nxs 
	
	rem protogen -i:!protopath!\%%~nxs -o:!outputpath!\%%~ns.cs
)

protogen !protofilelist! -o:!outputpath!\TotalProto.cs

set clientpath=..\..\clienttest\clienttest\proto-msg\
set serverpath=..\..\servertest_1\ServerTest_1\protobuf\protobuf-msg\

if exist !outputpath!\TotalProto.cs (
	if exist !clientpath! (
		copy !outputpath!\TotalProto.cs  !clientpath!
		echo copyto client success
	)
	if exist !serverpath! (
		copy !outputpath!\TotalProto.cs !serverpath!
		echo copyto server success
	)
)

endlocal

echo.
echo complete!

pause