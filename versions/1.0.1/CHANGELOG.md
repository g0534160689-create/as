# גרסה 1.0.1

## תיקונים
- קיבוע .NET SDK לענף 9.0.
- העברת Build ו־Publish ל־Visual Studio MSBuild כדי שטעינת רכיבי WinUI תתבצע מסביבת Visual Studio.
- הסרת הגדרה כפולה של `Grid.ColumnDefinitions` במסך הראשי.
- כיבוי ReadyToRun בפרסום העצמאי, משום שחבילת runtime המתאימה לא שוחזרה בבנייה הקודמת.

## בדיקות
- Build Release עבר בהצלחה ב־GitHub Actions.
- בדיקת Publish והפקת מתקין עדיין ממתינות להרצה חדשה.
