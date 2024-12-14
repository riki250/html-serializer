using System;
using System.Collections.Generic;
using System.Linq;

public class HtmlElement
{
    public string Id { get; set; }  // מזהה האלמנט ב-HTML
    public string Name { get; set; }  // שם התג (למשל, div, p)
    public Dictionary<string, string> Attributes { get; set; }  // תכונות האלמנט
    public List<string> Classes { get; set; }  // מחלקות האלמנט
    public string InnerHtml { get; set; }  // תוכן פנימי של האלמנט
    public HtmlElement Parent { get; set; }  // ההורה של האלמנט
    public List<HtmlElement> Children { get; set; }  // ילדים של האלמנט

    // בונה אלמנט חדש עם ערכים ברירת מחדל
    public HtmlElement()
    {
        Attributes = new Dictionary<string, string>();
        Classes = new List<string>();
        Children = new List<HtmlElement>();
    }

    // בונה אלמנט חדש עם ערכים מותאמים אישית
    public HtmlElement(string name, string id = "", string innerHtml = "")
    {
        Name = name;
        Id = id;
        InnerHtml = innerHtml;
        Attributes = new Dictionary<string, string>();
        Classes = new List<string>();
        Children = new List<HtmlElement>();
    }

    // הוספת תכונה לאלמנט
    public void AddAttribute(string key, string value)
    {
        Attributes[key] = value;
    }

    // הוספת מחלקה לאלמנט
    public void AddClass(string className)
    {
        Classes.Add(className);
    }

    // הוספת ילד לאלמנט
    public void AddChild(HtmlElement child)
    {
        child.Parent = this;  // קובעים את ההורה של הילד
        Children.Add(child);  // מוסיפים את הילד לרשימת הילדים
    }

    // מחזיר ייצוג HTML של האלמנט
    public override string ToString()
    {
        var classes = string.Join(" ", Classes);  // מחבר את כל המחלקות
        var attributes = string.Join(" ", Attributes.Select(attr => $"{attr.Key}=\"{attr.Value}\""));  // מחבר את כל התכונות
        var openTag = $"<{Name} id=\"{Id}\" class=\"{classes}\" {attributes}>";  // יצירת תג פתיחה
        var closeTag = $"</{Name}>";  // יצירת תג סגירה
        var childrenHtml = string.Join("", Children.Select(child => child.ToString()));  // יצירת HTML עבור הילדים

        return $"{openTag}{InnerHtml}{childrenHtml}{closeTag}";  // מחזיר את כל ה-HTML של האלמנט
    }

    // מחזיר ייצוג HTML מעוצב של האלמנט עם אינדנטציה
    public string ToFormattedString(int indent = 0)
    {
        var indentString = new string(' ', indent * 2);  // יצירת אינדנטציה
        var classes = string.Join(" ", Classes);  // מחבר את כל המחלקות
        var attributes = string.Join(" ", Attributes.Select(attr => $"{attr.Key}=\"{attr.Value}\""));  // מחבר את כל התכונות
        var openTag = $"{indentString}<{Name} id=\"{Id}\" class=\"{classes}\" {attributes}>";  // יצירת תג פתיחה
        var closeTag = $"{indentString}</{Name}>";  // יצירת תג סגירה
        var childrenHtml = string.Join(Environment.NewLine, Children.Select(child => child.ToFormattedString(indent + 1)));  // יצירת HTML עבור הילדים עם אינדנטציה

        return $"{openTag}{Environment.NewLine}{childrenHtml}{Environment.NewLine}{closeTag}";  // מחזיר את כל ה-HTML של האלמנט
    }

    // מחזיר את כל הצאצאים של האלמנט
    public IEnumerable<HtmlElement> Descendants()
    {
        var queue = new Queue<HtmlElement>();  // יצירת תור לעיבוד האלמנטים
        queue.Enqueue(this);  // הוספת האלמנט הראשון לתור

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

    // מחזיר את כל ההורים של האלמנט
    public IEnumerable<HtmlElement> Ancestors()
    {
        var current = this.Parent;  // מתחילים מההורה של האלמנט
        while (current != null)
        {
            yield return current;  // מחזירים את ההורה
            current = current.Parent;  // עוברים להורה הבא
        }
    }
}
