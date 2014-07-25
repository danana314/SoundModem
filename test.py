# -*- coding: utf-8 -*-
from __future__ import division

import scipy.signal as sig
from scipy.io.wavfile import write, read
import numpy as np
from pylab import plot, show, axis
import matplotlib.pyplot as plt

import soundmodem.dbpsk as d

def myplot(data, sample_rate):
    duration = len(data)/sample_rate
    t = np.linspace(0, duration, len(data))
    plot(t,data)
    axis('auto')
#    axis([0,10*period*periods_per_bit,amp*1.5,amp*-1.5])
    show()
    

def gen_filename(msg, freq, periods_per_bit):
    # writing the sound to a file
    filename = 'F' + str(freq) + \
        'PPB' + str(periods_per_bit) + \
        msg + \
        '.wav'
    return filename


if __name__=="__main__":
    #msg = "abcd091234"
    msg = "abc"
    amp = 10000
    freq = 1000
    sample_rate = 44100
    periods_per_bit = 20
    
    modsig = d.modulate(msg, amp, freq, sample_rate, periods_per_bit)
    #write(gen_filename(msg, freq, periods_per_bit), sample_rate, modsig)
    myplot(modsig, sample_rate)