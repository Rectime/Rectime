;Plugin required!
;https://code.google.com/p/inno-download-plugin/
#include <idp.iss>
#define MyAppName "RecTime"
#define MyAppVersion "1.2.4.0"
#define MyAppVerName "RecTime v1.2.4.0"
#define MyCompany "rectime.se"
#define MyAppURL "http://rectime.se"
#define MyAppLocation "..\RecTime"
#define MyAppExeName "RecTime.exe"

[Setup]
AppName={#MyAppName}
AppVerName={#MyAppVerName}
AppPublisher={#MyCompany}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={pf}\{#MyAppName}
DefaultGroupName={#MyAppName}
DisableProgramGroupPage=yes
OutputBaseFilename=RecTime-{#MyAppVersion}WinSetup
VersionInfoVersion={#MyAppVersion}
AppVersion={#MyAppVersion}
Compression=lzma
SolidCompression=yes
SetupIconFile =..\gfx\icon.ico
UninstallDisplayIcon={app}\{#MyAppExeName}

[Languages]
Name: english; MessagesFile: compiler:Default.isl

[Tasks]
Name: desktopicon; Description: {cm:CreateDesktopIcon}; GroupDescription: {cm:AdditionalIcons}; Flags: unchecked

[Files]
Source: {#MyAppLocation}\bin\Release\{#MyAppExeName}; DestDir: {app}; Flags: ignoreversion
Source: {#MyAppLocation}\bin\Release\{#MyAppExeName}.config; DestDir: {app}; Flags: ignoreversion
Source: {#MyAppLocation}\bin\Release\ffmpeg.exe; DestDir: {app}; Flags: ignoreversion
Source: {#MyAppLocation}\bin\Release\*.dll; DestDir: {app}; Flags: ignoreversion

[Icons]
Name: {group}\{#MyAppName}; Filename: {app}\{#MyAppExeName}
Name: {group}\{cm:UninstallProgram,{#MyAppName}}; Filename: {uninstallexe}
Name: {commondesktop}\{#MyAppName}; Filename: {app}\{#MyAppExeName}; Tasks: desktopicon

[Run]
Filename: {app}\{#MyAppExeName}; Description: {cm:LaunchProgram,{#MyAppName}}; Flags: nowait postinstall skipifsilent

[Code]
function Framework45IsNotInstalled(): Boolean;
var
  bSuccess: Boolean;
  regVersion: Cardinal;
begin
  Result := True;

  bSuccess := RegQueryDWordValue(HKLM, 'Software\Microsoft\NET Framework Setup\NDP\v4\Full', 'Release', regVersion);
  if (True = bSuccess) and (regVersion >= 378389) then begin
    Result := False;
  end;
end;

procedure InitializeWizard;
begin
  if Framework45IsNotInstalled() then
  begin
    idpAddFile('http://go.microsoft.com/fwlink/?LinkId=397707', ExpandConstant('{tmp}\NetFrameworkInstaller.exe'));
    idpDownloadAfter(wpReady);
  end;
end;

procedure InstallFramework;
var
  StatusText: string;
  ResultCode: Integer;
begin
  StatusText := WizardForm.StatusLabel.Caption;
  WizardForm.StatusLabel.Caption := 'Installing .NET Framework 4.5.2. This might take a few minutes…';
  WizardForm.ProgressGauge.Style := npbstMarquee;
  try
    if not Exec(ExpandConstant('{tmp}\NetFrameworkInstaller.exe'), '/passive /norestart', '', SW_SHOW, ewWaitUntilTerminated, ResultCode) then
    begin
      MsgBox('.NET installation failed with code: ' + IntToStr(ResultCode) + '.', mbError, MB_OK);
    end;
  finally
    WizardForm.StatusLabel.Caption := StatusText;
    WizardForm.ProgressGauge.Style := npbstNormal;

    DeleteFile(ExpandConstant('{tmp}\NetFrameworkInstaller.exe'));
  end;
end;

procedure CurStepChanged(CurStep: TSetupStep);
begin
  case CurStep of
    ssPostInstall:
      begin
        if Framework45IsNotInstalled() then
        begin
          InstallFramework();
        end;
      end;
  end;
end;
