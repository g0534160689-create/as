# אוצר החכמה – עמדת רב־קו (WinUI 3)

יישום Windows חדש המבוסס על C# ו־WinUI 3 עם XAML.

## מבנה
- `RavKavKiosk.WinUI` – אפליקציית WinUI 3.
- `Pages` – מסך ראשי, עמדת רב־קו, ניהול, יומן פעילות והגדרות.
- `Services` – שמירת הגדרות מקומית.
- `.github/workflows/build-windows.yml` – בנייה אוטומטית ב־Windows והעלאת תוצר כ־Artifact.

## בנייה מקומית
נדרשים Windows 10/11, Visual Studio 2022 עם workload של Windows App SDK / WinUI, ו־.NET 9 SDK.

```powershell
dotnet restore RavKavKiosk.WinUI/RavKavKiosk.WinUI.csproj
dotnet build RavKavKiosk.WinUI/RavKavKiosk.WinUI.csproj -c Release -p:Platform=x64
```

## מצב
שלד ממשק ראשוני. שילוב WebView2, קורא הכרטיסים, מדיניות אתרים, חלונות קופצים, יומן פעילות ומתקין Windows יבוצעו בשלבים נפרדים וייבדקו בבנייה.
