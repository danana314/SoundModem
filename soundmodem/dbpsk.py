# -*- coding: utf-8 -*-
from __future__ import division
import numpy as np
from pylab import plot, show, axis
from scipy.io.wavfile import write
from itertools import repeat

_encodedStartedBit = 1
_byteStartBit = 0
_byteStopBit = 1
_evenParity = True

_barkerCode = [1, 1, 1, 1, 1, 0, 0, 1, 1, 0, 1, 0, 1]


def modulate(msg):
    encmsg = _unipolar_to_bipolar(encode(msg))
    _modulate(encmsg, msg)
    return encmsg

def _modulate(encmsg, msg):
    amp = 10000
    freq = 1000
    period = 1/freq    
    periods_per_bit = 20
    duration = periods_per_bit * len(encmsg) / freq
    sample_rate = 44100
    samples_per_period = period*sample_rate
    tone = note(freq, duration, amp)
    
    samples_per_bit = periods_per_bit * samples_per_period
    msg_exp = [repeated for value in encmsg for repeated in repeat(value, int(samples_per_bit))]
    tone_modulated = np.int16([a*b for a,b in zip(tone, msg_exp)])
    
    # writing the sound to a file
    filename = 'F' + str(freq) + \
        'PPB' + str(periods_per_bit) + \
        msg + \
        '.wav'
    write(filename, sample_rate, tone_modulated)


    t = np.linspace(0, duration, duration*sample_rate)
    plot(t, tone_modulated)
    axis([0,10*period*periods_per_bit,amp*1.5,amp*-1.5])
    show()
    


def note(freq, duration, amp=1, rate=44100):
    t = np.linspace(0,duration,duration*rate)
    data = np.sin(2*np.pi*freq*t)*amp
    #scaled_data = np.int16(data/np.max(np.abs(data)) * 32767)
    return data.astype(np.int16) # two byte integers


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