using System;
using System.Collections.Generic;

public class Selector
{
    public string TagName { get; set; }  // שם התג (למשל: div, p)
    public string Id { get; set; }  // מזהה (id) של האלמנט ב-HTML
    public List<string> Classes { get; set; }  // רשימת המחלקות (classes) של האלמנט
    public Selector Parent { get; set; }  // אלמנט האב של ה-selector (שימוש בהיררכיה של selectors)
    public Selector Child { get; set; }  // אלמנט הילד של ה-selector (שימוש בהיררכיה של selectors)

    // בונה את ה-selector ומאתחל את רשימת המחלקות
    public Selector()
    {
        Classes = new List<string>();  // רשימת מחלקות חדשה, ריקה בהתחלה
    }

    // פונקציה סטטית שמבצעת Parsing של שאילתת CSS לפורמט של Selector
    // לדוגמה: "div#container.my-class" -> Selector עם שם tag="div", id="container", ו-class="my-class"
    public static Selector Parse(string query)
    {
        var root = new Selector();  // יצירת Selector חדש לשורש (ה-Selector הראשי)
        var current = root;  // מתחילים מהשורש

        // מפרידים את השאילתה לפי רווחים, כך שנוכל לטפל ב-selector עם מספר חלקים (למשל: div#id .class)
        var parts = query.Split(' ');
        foreach (var part in parts)
        {
            // מפרידים את ה-part של השאילתא לפי "#", "." (למשל div#id.my-class)
            var subParts = part.Split(new char[] { '#', '.' });

            // אם החלק הראשון של ה-part (לפני # או .) לא ריק, זהו שם התג
            if (subParts[0].Length > 0)
            {
                current.TagName = subParts[0];  // שמירת שם התג
            }

            // עבור כל חלק נוסף ב-subParts
            for (int i = 1; i < subParts.Length; i++)
            {
                // אם החלק מתחיל ב-# אז מדובר במזהה (id)
                if (part[i - 1] == '#')
                {
                    current.Id = subParts[i];  // שמירת ה-id
                }
                // אם החלק מתחיל ב-. אז מדובר במחלקה (class)
                else if (part[i - 1] == '.')
                {
                    current.Classes.Add(subParts[i]);  // הוספת מחלקה לרשימה
                }
            }

            // לאחר עיבוד ה-part הנוכחי, אנחנו יוצרים ילד חדש ל-selector הנוכחי
            var child = new Selector { Parent = current };  // אלמנט child עם ההורה הנוכחי
            current.Child = child;  // הקישור בין האב לילד
            current = child;  // נעבור ל-child החדש לעיבוד הבא
        }

        // בסופו של תהליך מחזירים את השורש, שמייצג את ה-selector המלא
        return root;
    }
}
