# -*- coding: utf-8 -*-

import soundmodem.dbpsk as d

if __name__=="__main__":
    enc = d.encode("abcd091234")
    dec = d.decode(enc)

    d.gentone()
