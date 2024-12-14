using System.Collections.Generic;

public static class HtmlElementExtensions
{
    // מתודה מורחבת שנועדה להחזיר את כל הצאצאים של אלמנט HTML
    // כולל את האלמנט עצמו
    public static IEnumerable<HtmlElement> Descendants(this HtmlElement element)
    {
        var queue = new Queue<HtmlElement>();  // יצירת תור לעיבוד האלמנטים
        queue.Enqueue(element);  // הוספת האלמנט הראשון לתור

        // כל עוד יש אלמנטים בתור
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();  // שליפת אלמנט מהתור
            yield return current;  // מחזירים את האלמנט הנוכחי

            // עבור כל הילד של האלמנט הנוכחי
            foreach (var child in current.Children)
            {
                queue.Enqueue(child);  // הוספת הילד לתור
            }
        }
    }
}
 