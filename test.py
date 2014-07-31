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
    filename = 'F' + str(freq) + \
        'PPB' + str(periods_per_bit) + \
        msg + \
        '.wav'
    return filename

def is_phase_shifted(x, y):
    #if sum of vec mult is < 0, then phase offset > pi/2
    return True if np.sum(np.multiply(x, y)) < 0 else False


def phaseshift(sig, periods_per_bit, freq, sample_rate):
    spb = periods_per_bit*(1/freq)*sample_rate  #samples per bit   
    seg2 = modsigfilt[0:spb]
    phaseOffset = []
    for x in range(0, len(modsigfilt), int(spb)):
        seg1 = list(seg2)
        seg2 = modsigfilt[x:x+spb]
        if len(seg1)==len(seg2): phaseOffset.append(is_phase_shifted(seg1, seg2))
    phaseOffset10 = [1 if b else 0 for b in phaseOffset]
    return phaseOffset10[1:]


def band_pass_filter(signal, numtaps, band_low, band_high, sample_rate):
    #window = sig.get_window('hamming', numtaps)
    h = sig.firwin(numtaps, [band_low, band_high], pass_zero=False, nyq=sample_rate/2)
    sigfilt = sig.lfilter(h, 1.0, signal)
    return sigfilt
    

if __name__=="__main__":
    msg = "abcd091234"
    #msg = "abc"
    amp = 10000
    freq = 1000
    sample_rate = 44100
    periods_per_bit = 20
    
    #modsig = d.modulate(msg, amp, freq, sample_rate, periods_per_bit)
    #write(gen_filename(msg, freq, periods_per_bit), sample_rate, modsig)  
    sample_rate, modsig = read('F1000PPB20abcd091234_recording.wav')
    modsig = modsig[:,1]
    
    t = np.linspace(0, len(modsig)/sample_rate, len(modsig))
    index1 = np.where(t>=2.4)[0][0]
    index2 = np.where(t>=4.1)[0][0]
    
    modsigbp = band_pass_filter(modsig, 10, 900, 1100, sample_rate) #Band pass filter
    modsigfilt = modsigbp[index1:index2]
    #modsigfilt = modsigfilt//np.max(modsigfilt)
    #need to threshold out low (noise mean?) and high values (or just normalize?)
    threshold = 1000
    modsigfilt = [cmp(x,0)*1 if abs(x) > threshold else 0 for x in modsigfilt]
    
    myplot(modsigfilt, sample_rate)
    
    '''
    phaseOffset10 = phaseshift(modsigfilt, periods_per_bit, freq, sample_rate)
    encmsg = d._unipolar_to_bipolar(d._encode(msg))
    encmsgdiff = [abs(x - encmsg[i - 1])//2 for i, x in enumerate(encmsg)][1:]
    decmsg = [1]
    for x in phaseOffset10:
        decmsg.append(decmsg[-1] if x==0 else -1*decmsg[-1])
    
    decmsguni = d._bipolar_to_unipolar(decmsg)
    print "phase offset=", decmsg
    print "enc msg=", encmsg '''