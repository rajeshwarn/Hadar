# Hadar
World's first attempt at DarkOrbit reverse-engineering.

## Disclaimer
> All the source code on this repository is provided for educational and informational purpose only, and should not be construed as legal advice or as an offer to perform legal services on any subject matter.
> 
> The information is not guaranteed to be correct, complete or current. 
> 
> The author (Alexandro Luongo) makes no warranty (expressed or implied) about the accuracy or reliability of the information at this repository or at any other website to which it is linked.

## Introduction
Hadar was born back in 2012, and has been the world first attempt at DarkOrbit reverse-engineering. <br> <br>
This repository contains refactored code of what used to be the original project: some features are still missing, but I plan to release them soon. <br> <br>
Considering it's an outdated project, I cannot ensure it still works with the latest game builds... but it's a starting point. <br><br>
If you're interested in the subject, feel free to give a look at the original Hadar thread: [Begin hacking on DarkOrbit client](http://www.elitepvpers.com/forum/darkorbit/2210662-p-o-c-begin-hacking-darkorbit-client.html)

## Features:
- [x] Automatically dump game messages
- [x] Deobfuscate mesage OPCodes
- [x] Commands/Handlers relationships (update IDs, remove fake ones...)
- [ ] Reverse structures for both incoming and outgoing messages (soon - code refactoring needed)

## Screenshots:
![alt tag](http://i48.tinypic.com/1hb32g.jpg)

## Special thanks:
- [Robust ABC [Dis-]Assembler (RABCDasm)](https://github.com/CyberShadow/RABCDAsm) 
