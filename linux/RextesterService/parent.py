#!/usr/bin/env python

import os
import sys
import resource
import subprocess
import time
import signal

def setlimits():
		resource.setrlimit(resource.RLIMIT_CPU, (5, 5))
		resource.setrlimit(resource.RLIMIT_CORE, (0, 0))
		resource.setrlimit(resource.RLIMIT_DATA, (100000000, 100000000))
		resource.setrlimit(resource.RLIMIT_FSIZE, (0, 0))		
		resource.setrlimit(resource.RLIMIT_MEMLOCK, (0, 0))		
		resource.setrlimit(resource.RLIMIT_NOFILE, (20, 20))
		resource.setrlimit(resource.RLIMIT_NPROC, (250, 250))
		resource.setrlimit(resource.RLIMIT_STACK, (100000000, 100000000))

		
		#resource.setrlimit(resource.RLIMIT_AS, (100000000, 100000000))
		#resource.setrlimit(resource.RLIMIT_LOCKS, (10, 10))
		#resource.setrlimit(resource.RLIMIT_MSGQUEUE, (100000000, 100000000))
		#resource.setrlimit(resource.RLIMIT_NICE, (0, 0))

os.setpgrp()
p = subprocess.Popen(sys.argv[1:], preexec_fn=setlimits)

fin_time = time.time() + 10
while p.poll() == None and fin_time > time.time():
        time.sleep(0.1)

if fin_time < time.time():
		sys.stderr.write("Process killed, because it ran longer than 10 seconds.")
		os.killpg(0, signal.SIGKILL)
		
sys.stderr.write('\n')
sys.stderr.write(str(p.returncode))

os.killpg(0, signal.SIGKILL)
