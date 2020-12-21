; Script generated by the Inno Setup Script Wizard.    Version 2 !!!
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

[Setup]
AppName=DATEV-Export f�r EasyCash&Tax
AppVerName=DATEV-Export f�r EasyCash&Tax 0.1
DiskSpanning=no
AppPublisher=tm
AppPublisherURL=http://www.easyct.de
AppSupportURL=http://www.easyct.de
AppUpdatesURL=http://www.easyct.de
DefaultDirName={pf}\EasyCash&Tax\Plugins\Datev-Export
DefaultGroupName=EasyCash
OutputBaseFilename=ECTDatevSetup
OutputDir=.\Setup
MinVersion=5.0         
LicenseFile=.\LIZENZ.txt
; MFC9 greift auf GetFileSizeEx zu, das Win 98 nicht in der kernel.dll hat.  
Compression=bzip   
;SignTool=winsdk81sha1   ; dual sign the 
;SignTool=winsdk81sha256 ; installer

[Files]
Source: ECTDatev\bin\Release\ECTDatev.dll; DestDir: {app}; Flags: ignoreversion        
Source: ECTDatev\logo.jpg; DestDir: {app}; Flags: ignoreversion
Source: .\LIZENZ.TXT; DestDir: {app}; Flags: ignoreversion
; Web-Installer (1,3 MB, von https://www.microsoft.com/de-de/download/details.aspx?id=49977 ):
Source: .\NDP461-KB3102438-Web.exe; DestName: "NetFrameworkInstaller.exe"; DestDir: {tmp}; Flags: deleteafterinstall; AfterInstall: InstallFramework; Check: FrameworkIsNotInstalled
; Offline-Installer (65 MB, von https://www.microsoft.com/de-DE/download/details.aspx?id=49982 ):
;Source: .\NDP461-KB3102436-x86-x64-AllOS-ENU.exe; DestName: "NetFrameworkInstaller.exe"; DestDir: {tmp}; Flags: deleteafterinstall; AfterInstall: InstallFramework; Check: FrameworkIsNotInstalled

[Run]
Filename: "{dotnet40}\RegAsm.exe"; Parameters: ECTDatev.dll; WorkingDir: {app}; StatusMsg: "Registering Controls..."; Flags: runminimized

[Registry]
Root: HKLM; Subkey: Software\Tools; Flags: uninsdeletekeyifempty
Root: HKLM; Subkey: Software\Tools\EasyCash; Flags: uninsdeletekeyifempty
Root: HKLM; Subkey: Software\Tools\EasyCash\Plugins; Flags: uninsdeletekeyifempty
Root: HKLM; Subkey: Software\Tools\EasyCash\Plugins\DATEV-Export; Flags: uninsdeletekey
Root: HKLM; Subkey: Software\Tools\EasyCash\Plugins\DATEV-Export; ValueType: string; ValueName: Aufruf; ValueData: ECTDatev.UserControl1
Root: HKLM; Subkey: Software\Tools\EasyCash\Plugins\DATEV-Export; ValueType: string; ValueName: Beschreibung; ValueData: Daten-Export im DATEV-Format
Root: HKLM; Subkey: Software\Tools\EasyCash\Plugins\DATEV-Export; ValueType: string; ValueName: Bitmap; ValueData: {app}\logo.jpg
Root: HKLM; Subkey: Software\Tools\EasyCash\Plugins\DATEV-Export; ValueType: string; ValueName: Name; ValueData: DATEV-Export
Root: HKLM; Subkey: Software\Tools\EasyCash\Plugins\DATEV-Export; ValueType: string; ValueName: Typ; ValueData: noscroll

[Languages]
Name: "de"; MessagesFile: "compiler:Languages\German.isl"

[Code]
function CheckProcessRunning( aProcName,
                              aProcDesc: string ): boolean;
var
  ShellResult: boolean;
  ResultCode: integer;
  cmd: string;
  sl: TStringList;
  f: string;
  d: string;
begin
  cmd := 'for /f "delims=," %%i ' + 
         'in (''tasklist /FI "IMAGENAME eq ' + aProcName + '" /FO CSV'') ' + 
         'do if "%%~i"=="' + aProcName + '" exit 1'; 
  f := 'CheckProc.cmd';
  d := AddBackSlash( ExpandConstant( '{tmp}' ));
  sl := TStringList.Create;
  sl.Add( cmd );
  sl.Add( 'exit /0' );
  sl.SaveToFile( d + f );
  sl.Free;
  Result := true;
  while ( Result ) do
  begin
    ResultCode := 1;
    ShellResult := Exec( f,
                         '',
                         d, 
                         SW_HIDE, 
                         ewWaitUntilTerminated, 
                         ResultCode );
    Result := ResultCode > 0;
    if Result and 
       ( MsgBox( aProcDesc + ' ist noch aktiv. Das Programm muss beendet werden, um fortzufahren.'#13#10#13#10 + 
                 'Bitte wechseln Sie zu dem Programm, schlie�en Sie es und dr�cken auf OK.', 
                 mbConfirmation, 
                 MB_OKCANCEL ) <> IDOK ) then
      Break;
  end;
  DeleteFile( d + f );
end;
        
var CancelWithoutPrompt: boolean;

// Perform some initializations.  Return False to abort setup
function InitializeSetup: Boolean;
begin
  // Do not use any user defined vars in here such as {app}
  Result := not ( CheckProcessRunning( 'EasyCT.exe', 'EasyCash&Tax' ));

  CancelWithoutPrompt := false;
  result := true;
end;

function InitializeUninstall: Boolean;
var
  ResultCode: Integer;
begin
  Result := not ( CheckProcessRunning( 'EasyCT.exe', 'EasyCash&Tax' ));
  Exec(ExpandConstant('{dotnet40}\regasm.exe'), '/u ECTDatev.dll', '', SW_HIDE, ewWaitUntilTerminated, ResultCode)
end;   

procedure CancelButtonClick(CurPageID: Integer; var Cancel, Confirm: Boolean);
begin
  if CurPageID=wpInstalling then
    Confirm := not CancelWithoutPrompt;
end;

function FrameworkIsNotInstalled: Boolean;
begin
  Result := not RegKeyExists(HKEY_LOCAL_MACHINE, 'SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full');
  if not FileExists(ExpandConstant('{dotnet40}\regasm.exe')) then  // doppel-check wegen Wine-Unterstuetzung
    MsgBox('Das Diensprogramm regasm.exe konnte nicht gefunden werden. Wenn diese Installation auf einem Mac- oder Linux-System mit dem Windows-Emulator Wine ausgef�hrt wird, kann "winetricks dotnet461" evtl. Abhilfe schaffen.', mbError, MB_OK);
end;

procedure InstallFramework;
var
  StatusText: string;
  ResultCode: Integer;
begin
  StatusText := WizardForm.StatusLabel.Caption;
  WizardForm.StatusLabel.Caption := 'Installing .NET framework...';
  WizardForm.ProgressGauge.Style := npbstMarquee;
  try
      if not Exec(ExpandConstant('{tmp}\NetFrameworkInstaller.exe'), '/q /norestart', '', SW_SHOW, ewWaitUntilTerminated, ResultCode) then
  begin
    MsgBox('.NET installation failed with code: ' + IntToStr(ResultCode) + '.',
      mbError, MB_OK);
    CancelWithoutPrompt := true;
    WizardForm.Close;       
  end;
  finally
    WizardForm.StatusLabel.Caption := StatusText;
    WizardForm.ProgressGauge.Style := npbstNormal;
  end;
end;