# -*- coding: utf-8 -*-
import numpy as np
import binascii

_encodedStartedBit = 1
_byteStartBit = 0
_byteStopBit = 1
_evenParity = True

_barkerCode = [1, 1, 1, 1, 1, 0, 0, 1, 1, 0, 1, 0, 1]


def modulate(msg):
    encmsg = encode(msg)

    return encmsg


def sound(fs):
    import numpy as np
    from scipy.io.wavfile import write

    data = np.random.uniform(-1,1,44100) # 44100 random samples between -1 and 1
    scaled = np.int16(data/np.max(np.abs(data)) * 32767)
    write('test.wav', 44100, scaled)


def note(freq, duration, amp=1, rate=44100):
    from numpy import linspace,sin,pi,int16
    t = linspace(0,duration,duration*rate)
    data = sin(2*pi*freq*t)*amp
    return data.astype(int16) # two byte integers

def gentone():
    from scipy.io.wavfile import write
    #from pylab import plot,show,axis
    #from numpy import linspace

    # A tone, 2 seconds, 44100 samples per second
    tone = note(440,2,amp=10000)

    write('440hzAtone.wav',44100,tone) # writing the sound to a file

    #plot(linspace(0,2,2*44100),tone)
    #axis([0,0.4,15000,-15000])
    #show()



def encode(message):
    encoded = [_encodedStartedBit]
    for c in list(bytearray(message)):
        binarr = [int(b, 2) for b in format(c, '#010b')[2:]]
        for bit in binarr:
            encoded.append(encoded[-1] ^ bit)
    return encoded


def decode(message):
    decoded_arr = [yi ^ message[i - 1] for i, yi in enumerate(message)][1:]
    decoded = ""
    for i in range(0, len(decoded_arr), 8):
        decoded += chr(int("0b" + ''.join(str(b) for b in decoded_arr[i:i + 8]), 2))
    return decoded


def _unipolar_to_bipolar(arr):
    return [2 * x - 1 for x in arr]


def _bitCount(int_type):
    count = 0
    while (int_type):
        int_type &= int_type - 1
        count += 1
    return (count)


def _parityOf(int_type):
    parity = 0
    while (int_type):
        parity = ~parity
        int_type = int_type & (int_type - 1)
    return (parity)