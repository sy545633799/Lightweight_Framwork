#!/bin/sh 
ps aux | grep skynet | awk '/config/{print $2}' | xargs kill