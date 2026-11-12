# גרסה 1.0.1

## תיקונים
- קיבוע .NET SDK לענף 9.0.
- העברת Build ו־Publish ל־Visual Studio MSBuild כדי שטעינת רכיבי WinUI תתבצע מסביבת Visual Studio.
- הסרת הגדרה כפולה של `Grid.ColumnDefinitions` במסך הראשי.
- כיבוי ReadyToRun בפרסום העצמאי כדי למנוע דרישה לחבילת runtime שלא שוחזרה.
- תיקון נתיב PowerShell למהדר Inno Setup.

## תוצאת בנייה מאומתת
- Build Release: הצליח.
- Publish עצמאי ל־Windows x64: הצליח.
- יצירת מתקין Inno Setup: הצליחה.
- העלאת מתקין Windows x64 וגרסה ניידת ל־GitHub Actions Artifacts: הצליחה.

המתקין נוצר בבניית GitHub Actions מספר 24.
