from sys import argv


lstart = int(argv[1])
lend = int(argv[2])
tempoChange = float(argv[3])

def loopEndTempoAdjust(loopEnd, tempoCh):
	# 7th Degree Polynomial Fit
	# Coefficient Data:
	a =	9.99972502058E-001
	b =	-9.99729521293E-003
	c =	9.95326197331E-005
	d =	-9.62154038913E-007
	e =	8.35056874072E-009
	f =	-5.65998662116E-011
	g =	2.47431258370E-013
	h =	-4.99190378557E-016

	y=a+b*tempoCh+c*pow(tempoCh,2)+d*pow(tempoCh,3)+e*pow(tempoCh,4)+f*pow(tempoCh,5)+g*pow(tempoCh,6)+h*pow(tempoCh,7)
	
	adjustedLoopEnd = loopEnd * y
	return int(round(adjustedLoopEnd))


def loopStartTempoAdjust(loopStart, tempoCh):
	
	# 7th Degree Polynomial Fit:  y=a+bx+cx^2+dx^3...
	# Coefficient Data:
	a =	9.99934013822E-001
	b =	-9.99688418675E-003
	c =	9.95282911409E-005
	d =	-9.62195332321E-007
	e =	8.35455483476E-009
	f =	-5.66841846351E-011
	g =	2.48190851368E-013
	h =	-5.01714192476E-016
	y=a+b*tempoCh+c*pow(tempoCh,2)+d*pow(tempoCh,3)+e*pow(tempoCh,4)+f*pow(tempoCh,5)+g*pow(tempoCh,6)+h*pow(tempoCh,7)

	adjustedLoopStart = loopStart * y
	return int(round(adjustedLoopStart))


sampleAdd = (lstart % 14336 > 0) * 14336 - lstart % 14336
fixedlstart = lstart + sampleAdd
fixedlend = lend + sampleAdd

flstart = loopStartTempoAdjust(lstart, tempoChange)
fsampleAdd = (flstart % 14336 > 0) * 14336 - flstart % 14336
flstart += fsampleAdd
flend = loopEndTempoAdjust(lend, tempoChange) + fsampleAdd

print "set brstmlstart=" + str(fixedlstart) + "\nset brstmlend=" + str(fixedlend) + "\nset flstart=" + str(flstart) + "\nset flend=" + str(flend)
