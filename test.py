# -*- coding: utf-8 -*-

import soundmodem.dbpsk as d

if __name__=="__main__":
    msg = "abcd091234"
    d.modulate(msg)