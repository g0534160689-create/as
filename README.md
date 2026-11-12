# אוצר החכמה – עמדת רב־קו (WinUI 3)

יישום Windows x64 המבוסס על C#, WinUI 3, XAML ו־WebView2.

## רכיבי הפרויקט
- `RavKavKiosk.WinUI` – אפליקציית Windows.
- `Pages` – מסכי ראשי, עמדת רב־קו, ניהול, יומן והגדרות.
- `Services` – שמירת הגדרות, גיבוי, יומן פעילות ומדיניות אתרים.
- `installer/RavKavKiosk.iss` – מתקין Windows באמצעות Inno Setup.
- `.github/workflows/build-windows.yml` – בנייה, פרסום ואריזת מתקין אוטומטיים.

## יכולות שמומשו
- WebView2 פנימי עם חסימת ניווטים לפי מדיניות.
- רשימה שחורה, רשימה לבנה ואפשרות לאכוף מצב רשימה לבנה בלבד.
- ניהול חלונות קופצים לפי הגדרות המערכת.
- בחירת אלמנט בדף באמצעות מצב בחירה חזותי, שמירת בורר CSS והסתרתו.
- שחזור האלמנטים שהוסתרו בלי למחוק סגנונות display מקוריים שהוגדרו על ידי האתר.
- שמירת הגדרות ב־JSON עם גיבוי מקומי ושחזור מקובץ הגיבוי אם הקובץ הראשי פגום.
- יומן פעילות מקומי עם חיפוש, רענון וניקוי.
- בחירת ערכת נושא כהה או בהירה; הבחירה נשמרת ומוחלת בהפעלה הבאה.
- מתקין Windows x64 וגרסה ניידת שמופקים ב־GitHub Actions.

## בנייה מקומית
נדרשים Windows 10/11, Visual Studio עם רכיבי Windows App SDK / WinUI, ו־.NET 9 SDK.

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

## שמירת נתונים
ההגדרות נשמרות תחת:
`%LOCALAPPDATA%\DaatAharon\RavKavKiosk\settings.json`

יומן הפעילות נשמר בקובץ `activity.jsonl` באותה תיקייה. קובץ גיבוי ההגדרות הוא `settings.json.bak`.

## מגבלות שעדיין דורשות עבודה ובדיקת מחשב
- חיבור פיזי לקורא כרטיסי רב־קו ותקשורת מול הדרייבר שלו עדיין אינם ממומשים.
- מדיניות אתרים וחלונות קופצים ממומשת בתוך WebView2, אך יש לבדוק את כל מסלולי ההתחברות וההפניות החיצוניות של אתר רב־קו בפועל.
- בחירת אלמנטים משתמשת בבוררי CSS. אתרים דינמיים עשויים להחליף מבנה DOM, ולכן בורר מסוים עשוי להפסיק להתאים לאחר שינוי באתר.
- נדרש ניסוי הפעלה במחשב Windows אמיתי, כולל בדיקת WebView2 Runtime, קורא הכרטיסים, מסכי התחברות, חלונות קופצים ושחזור לאחר סגירה.

## GitHub Actions
כל Push ל־`main` מפעיל בנייה. בסיום מוצגים Artifacts של מתקין x64 וגרסה ניידת. יצירת Release קבוע מתבצעת באמצעות תגית `vX.Y.Z`.
