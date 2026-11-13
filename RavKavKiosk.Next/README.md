# RavKavKiosk Next — clean rebuild

This is the clean WinUI 3 implementation, developed separately from the legacy application until it passes build and runtime validation.

## Stack
- C# / .NET 9
- WinUI 3 + XAML
- WebView2
- Local JSON settings with backup
- Local JSONL activity log
- x64 self-contained Windows publish

## Build
Open `RavKavKiosk.Next/RavKavKiosk.Next.csproj` in Visual Studio 2022 with the Windows App SDK workload, or run:

```powershell
dotnet restore RavKavKiosk.Next/RavKavKiosk.Next.csproj
msbuild RavKavKiosk.Next/RavKavKiosk.Next.csproj /t:Build /p:Configuration=Release /p:Platform=x64 /p:WindowsPackageType=None /p:WindowsAppSDKSelfContained=true
```

## Current foundation
- Main navigation and dashboard
- Embedded WebView2 browser
- Allow-list / block-list policy checks for HTTP and HTTPS navigation
- Popup policy
- Local settings persistence and backup
- Activity log
- Startup error diagnostics

## Validation status
A successful CI build validates compilation and packaging only. It does not prove that the app launches on every Windows machine or that a physical card reader works. Those require runtime and hardware tests.
