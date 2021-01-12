# ECTDatev
DATEV-Export-Plugini für EasyCash&amp;Tax

## Requirements

- Visual Studio 2017 oder 2019
- Inno Setup

## Debuggen

Um das Plugin sinnvoll debuggen zu können, müssen die benötigten Registry-Einträge zunächst gesetzt sein.
Dafür am Besten den Plugin-Installer einmal ausführen oder die Keys entsprechend der Einträge in der .iss-Datei manuell setzen.
Damit man Visual Studio nicht als Administrator ausführen muss, kann man einmal mit einer Administrator-Kommandozeile die C#-Bibliothek registrieren, z.B.:

	C:\Windows\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe D:\<MeinArbeitsverzeichnisIrgendwo>\ECTDatev\ECTDatev\bin\Debug\ECTDatev.dll

Hinweis: "Register for COM Interop" ist deshalb bewusst ausgeschaltet.

Es ist zu empfehlen, die EC&T-Laufzeitdateien in das Verzeichnis D:\<MeinArbeitsverzeichnisIrgendwo>\ECTDatev\ECTDatev\bin\Debug  zu kopieren. Als da wären:

	- EasyCT.exe
	- EasyCTX.ocx
	- EasyCTXP.dll
	- CrashSender1402.exe
	- crashrpt_lang.ini

Dann den Pfad der EC&T-Exe (z.B. "D:\<MeinArbeitsverzeichnisIrgendwo>\ECTDatev\ECTDatev\bin\Debug\EasyCT.exe") in den Debug-Projekteigenschaften anpassen.
Jetzt F5 drücken und das Plugin im Plugin-Menü von EC&T auswählen.