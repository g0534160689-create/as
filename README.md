# אוצר החכמה – עמדת רב־קו (WinUI 3)

יישום Windows x64 המבוסס על C#, WinUI 3 ו־XAML.

## רכיבי הפרויקט
- `RavKavKiosk.WinUI` – אפליקציית Windows.
- `Pages` – מסכי המערכת.
- `Services` – שירותי שמירה מקומית והגדרות.
- `installer/RavKavKiosk.iss` – הגדרת מתקין Windows באמצעות Inno Setup.
- `.github/workflows/build-windows.yml` – בנייה, פרסום ואריזת מתקין אוטומטיים.

## בנייה מקומית
נדרשים Windows 10/11, Visual Studio 2022 עם workload של Windows App SDK / WinUI, ו־.NET 9 SDK.

```powershell
dotnet restore RavKavKiosk.WinUI/RavKavKiosk.WinUI.csproj
dotnet build RavKavKiosk.WinUI/RavKavKiosk.WinUI.csproj -c Release -p:Platform=x64
```

## יצירת מתקין מקומי
1. פרסם את האפליקציה לתיקייה `artifacts/publish`:
```powershell
dotnet publish RavKavKiosk.WinUI/RavKavKiosk.WinUI.csproj -c Release -r win-x64 --self-contained true -p:Platform=x64 -p:WindowsPackageType=None -p:WindowsAppSDKSelfContained=true -o artifacts/publish
```
2. התקן Inno Setup 6.
3. הרץ:
```powershell
iscc /DAppVersion=1.0.0 installer/RavKavKiosk.iss
```
המתקין ייווצר בתיקייה `artifacts/installer`.

## בנייה והורדת מתקין מ־GitHub
- כל Push ל־`main` וכל הפעלה ידנית מפעילים בנייה.
- בסיום מוצגים ב־GitHub Actions שני Artifacts: מתקין x64 וקובצי פרסום ניידים.
- כדי ליצור Release להורדה קבועה, צור תגית גרסה בפורמט `v1.0.0` ודחוף אותה:
```powershell
git tag v1.0.0
git push origin v1.0.0
```
ה־workflow יבנה את המתקין ויצרף אותו ל־GitHub Release.

## שמירת הגדרות
ההגדרות נשמרות מקומית תחת:
`%LOCALAPPDATA%\DaatAharon\RavKavKiosk\settings.json`

## מצב מימוש והערות
- מסך הניהול כולל עריכת רשימות דומיינים לבנה ושחורה ושמירה מקומית.
- מתקין Windows x64 מוגדר להיבנות אוטומטית ב־GitHub Actions.
- יצירת מתקין בפועל תלויה בהצלחת ריצת ה־workflow; אין לראות בקובץ ההתקנה מוכן עד שה־Artifact או ה־Release מופיעים ב־GitHub.
- מדיניות החסימה, טיפול בחלונות קופצים, אינטגרציה לקורא הכרטיסים ובדיקות חומרה דורשים השלמה ואימות נפרדים. הגדרת הרשימות במסך הניהול לבדה אינה אוכפת חסימה בדפדפן.
