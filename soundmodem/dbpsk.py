# -*- coding: utf-8 -*-
import numpy as np
import binascii

_encodedStartedBit = 1
_byteStartBit = 0
_byteStopBit = 1
_evenParity = True

def encode(message):
    encoded = [_encodedStartedBit]
    for c in list(bytearray(message)):
        bini = [int(b,2) for b in format(c, '#010b')[2:]]
        print bini
        #xord = [ord(a)^ord(b) for a,b in zip(binc, binc)]
        #print xord

def _bitCount(int_type):
    count = 0
    while (int_type):
        int_type &= int_type - 1
        count += 1
    return(count)

def _parityOf(int_type):
    parity = 0
    while (int_type):
        parity = ~parity
        int_type = int_type & (int_type - 1)
    return(parity)