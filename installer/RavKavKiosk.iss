; Script generated for the Rav-Kav kiosk.
#define AppName "אוצר החכמה – עמדת רב־קו"
#ifndef AppVersion
  #define AppVersion "1.0.0"
#endif
#define AppPublisher "ישיבת דעת אהרן"
#define AppExeName "RavKavKiosk.WinUI.exe"

[Setup]
AppId={{8E55A43E-21C2-4C9C-9B10-DAA9E8D7A210}
AppName={#AppName}
AppVersion={#AppVersion}
AppPublisher={#AppPublisher}
DefaultDirName={autopf}\DaatAharon\RavKavKiosk
DefaultGroupName={#AppName}
DisableProgramGroupPage=yes
OutputDir=..\artifacts\installer
OutputBaseFilename=RavKavKiosk-Setup-x64
ArchitecturesInstallIn64BitMode=x64
ArchitecturesAllowed=x64
PrivilegesRequired=admin
Compression=lzma2
SolidCompression=yes
WizardStyle=modern
UninstallDisplayIcon={app}\{#AppExeName}
SetupLogging=yes

[Languages]
Name: "hebrew"; MessagesFile: "compiler:Languages\Hebrew.isl"

[Tasks]
Name: "desktopicon"; Description: "יצירת קיצור דרך בשולחן העבודה"; GroupDescription: "קיצורי דרך:"; Flags: unchecked

[Files]
Source: "..\artifacts\publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{autoprograms}\{#AppName}"; Filename: "{app}\{#AppExeName}"
Name: "{autodesktop}\{#AppName}"; Filename: "{app}\{#AppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#AppExeName}"; Description: "הפעל את עמדת רב־קו"; Flags: postinstall nowait skipifsilent
