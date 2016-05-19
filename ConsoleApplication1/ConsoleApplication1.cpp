// ConsoleApplication1.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

unsigned char Fn(char cA, int* iB, unsigned int uC) {
	short unsigned int usD = 3;
	uC = uC + *iB;
	uC += cA - usD;
	return uC;
}

void main()
{
	int* i;
	i = new int(3);
	auto x = Fn('o', i, 12);
}
