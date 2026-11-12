# גרסה 1.0.1

## תיקונים
- קיבוע .NET SDK לענף 9.0.
- העברת Build ו־Publish ל־Visual Studio MSBuild כדי שטעינת רכיבי WinUI תתבצע מסביבת Visual Studio.
- הסרת הגדרה כפולה של `Grid.ColumnDefinitions` במסך הראשי.
- כיבוי ReadyToRun בפרסום העצמאי, משום שחבילת runtime המתאימה לא שוחזרה בבנייה הקודמת.
- תיקון נתיב PowerShell למהדר Inno Setup.

## בדיקות
- Build Release עבר בהצלחה ב־GitHub Actions.
- Publish עצמאי ל־Windows x64 עבר בהצלחה.
- יצירת מתקין EXE ממתינה לאימות לאחר תיקון נתיב Inno Setup.
