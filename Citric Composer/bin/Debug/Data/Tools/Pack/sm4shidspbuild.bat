@echo off
SetLocal EnableDelayedExpansion

vgmstream -b -l1 -d0 -f0 -o "%~n1.bak.wav" "%~1" > "%~n1.bat"

call "%~n1.bat"

:Doing custom vgmstream build later to replace this. Needed for MKWii/7/8 final lap generation
tempoWrite.py !lstart! !lend! 15 >> "%~n1.bat"	
call "%~n1.bat"
set wdrevBuildFinalLap=

set wdrevBuild=
set /a dsps=!chan!-1
echo %dsps%
if /I %loop% == 1 (
	for /L %%c in (0,1,%dsps%) do vgmstream -l1 -d1 -f0 -1 %%c -o "%~n1.%%c.wav" "%~1" & DSPADPCM -e "%~n1.%%c.wav" "%~n1.%%c.dsp" -l!brstmlstart!-!brstmlend! & set wdrevBuild=!wdrevBuild! "%~n1.%%c.dsp"
) else (
	for /L %%c in (0,1,%dsps%) do vgmstream -l1 -d1 -f0 -1 %%c -o "%~n1.%%c.wav" "%~1" & DSPADPCM -e "%~n1.%%c.wav" "%~n1.%%c.dsp" & set wdrevBuild=!wdrevBuild! "%~n1.%%c.dsp"
)
:uncomment the below lines to build a final lap version - requires soundstretch.exe	
:for /L %%c in (0,1,%dsps%) do soundstretch "%~n1.%%c.wav" "%~n1.%%c_F.wav" -rate=15 -pitch=0 & DSPADPCM -e "%~n1.%%c_F.wav" "%~n1.%%c_F.dsp" -l!flstart!-!flend! & set wdrevBuildFinalLap=!wdrevBuildFinalLap! "%~n1.%%c_F.dsp"

:Wii U
:revb --build-bfstm "%~n1.F.bfstm" !wdrevBuildFinalLap!
:revb --build-bfstm "%~n1.bfstm" !wdrevBuild!

:Smash
revb --build-idsp "%~n1.idsp" !wdrevBuild!

:Hyrule Warriors
:revb --build-g1l "%~n1.g1l" !wdrevBuild!

:Wii
:revb --build-brstm "%~n1.F.brstm" !wdrevBuildFinalLap!
:revb --build-brstm "%~n1.brstm" !wdrevBuild!

:3DS
:revb --build-bcstm "%~n1.F.bcstm" !wdrevBuildFinalLap!
:revb --build-bcstm "%~n1.bcstm" !wdrevBuild!

for /L %%c in (0,1,%dsps%) do DEL "%~n1.%%c.txt" "%~n1.%%c.wav" "%~n1.%%c_F.wav" "%~n1.%%c_F.txt" "%~n1.bak.wav" "%~n1.bat" 
for /L %%c in (0,1,%dsps%) do DEL "%~n1.%%c.dsp" "%~n1.%%c_F.dsp"
