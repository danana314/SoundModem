# -*- coding: utf-8 -*-
from __future__ import division
import numpy as np
from itertools import repeat

_encodedStartedBit = 1
_barkerCode = [1, 1, 1, 1, 1, 0, 0, 1, 1, 0, 1, 0, 1]


def modulate(msg, amp, freq, sample_rate, periods_per_bit):
    # Encode message    
    encmsg = _unipolar_to_bipolar(_encode(msg))
    
    # Modulate signal
    period = 1/freq    
    duration = periods_per_bit * len(encmsg) / freq
    samples_per_period = period*sample_rate
    samples_per_bit = periods_per_bit * samples_per_period
    
    carrier = _generate_carrier_signal(freq, duration, amp)
    msg_exp = [repeated for value in encmsg for repeated in repeat(value, int(samples_per_bit))]    # expanded to match signal length
    modulated_signal = np.int16([a*b for a,b in zip(carrier, msg_exp)])
    return modulated_signal
    

def demodulate(data, sample_rate):
    duration = len(data)/sample_rate


def _generate_carrier_signal(freq, duration, amp=1, rate=44100):
    t = np.linspace(0,duration,duration*rate)
    data = np.sin(2*np.pi*freq*t)*amp
    #scaled_data = np.int16(data/np.max(np.abs(data)) * 32767)
    return data.astype(np.int16) # two byte integers


def _encode(message):
    encoded = [_encodedStartedBit]
    for c in list(bytearray(message)):
        binarr = [int(b, 2) for b in format(c, '#010b')[2:]]
        for bit in binarr:
            encoded.append(encoded[-1] ^ bit)
    return encoded


def _decode(message):
    decoded_arr = [yi ^ message[i - 1] for i, yi in enumerate(message)][1:]
    decoded = ""
    for i in range(0, len(decoded_arr), 8):
        decoded += chr(int("0b" + ''.join(str(b) for b in decoded_arr[i:i + 8]), 2))
    return decoded


def _unipolar_to_bipolar(arr):
    return [2 * x - 1 for x in arr]


def _bipolar_to_unipolar(arr):
    return [(x + 1) // 2 for x in arr]


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