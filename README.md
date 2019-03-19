# UIDtoKeyboard
Read RFID/NFC Tag UID and key-in as a keyboard input (keyboard emulation)

## Quick Overview
This software purpose is to read Contactless Smart Card UID (Unique Identifier) and emulate as a keyboard key-in to the position where the cursor is active.

The software contains 4 selectable mode (as of version 1.1.0) of reading:  
1- 4 Byte UID Mode (e.g. 92A78230)  
2- 4 Byte Reversed Hex mode (e.g. 3082A792)  
3- 7 Byte UID Mode  
4- 7 Byte Reversed Hex mode

## Installing
If you are running a windows base operating system, download the release at https://github.com/tithanayut/UIDtoKeyboard/releases/, extract the zip file and launch the executable file inside. The software should work straight away.

In case that the software can't be launch, check that .Net Framework 4.0 is installed on your PC.

This software doesn't support Android, macOS or Linux.

## Running the software
The configuration is very simple. Simply plug in the PC/SC compatible reader. Launch the software, choose the reader and reading mode. Finally, click on Start Monitor.

To test the software, open notepad or program you wish to test with, put the card on the reader and the software should type in the card UID right away. 

## Version
-(Release 1) Version 1.0.0 : Initial Release  
-(Release 2) Version 1.1.0 : Add support for reading 7 Byte UID Card (i.e. Mifare Ultralight, Mifare DESfire)

## Built With
This application is built using VB.Net on Visual Studio 2017 Community Edition. This software use PC/SC wrapper class for .NET by Daniel Mueller, for more information about this library see: https://github.com/danm-de/pcsc-sharp/

The default framework version is set to .Net Framework 4.0, but able to functioning correctly when newer version of .Net Framework is selected.

The program is tested with ACS ACR122U reader and everything is working as described.
According to the PC/SC Library, this software support all of PC/SC compatible reader.

## Authors
Code/project developed by Thanayut T.  
PC/SC wrapper classes for .NET developed by Daniel Mueller, see https://github.com/danm-de/pcsc-sharp/.

## License
This software and its code license under BSD 2-Clause License. Feel free to use, modify, redistribute under the BSD 2-Clause License. For more information see: LICENSE file.

Note: This software use PC/SC wrapper classes for .NET developed by Daniel Mueller, for license see: https://github.com/danm-de/pcsc-sharp/blob/master/COPYING.


Keyword: Read UID and Keyboard Emulation, Card UID to Keyboard, UID Keyboard Input
