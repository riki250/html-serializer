using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;

class Program
{
    // הפונקציה הראשית של התוכנית, מריצה את כל הקוד
    static async Task Main(string[] args)
    {
        // יצירת מופע של HtmlHelper כדי לטעון את רשימות התגים והתגים הסגורים באופן אוטומטי
        var htmlHelper = HtmlHelper.GetInstance(@"C:\Users\user\Desktop\פרקטיקוד 2\HtmlCrawler\HtmlCrawler\bin\Debug\net6.0\resources\HtmlTags.json", @"C:\Users\user\Desktop\פרקטיקוד 2\HtmlCrawler\HtmlCrawler\bin\Debug\net6.0\resources\HtmlVoidTags.json");

        // הדפסת כל התגים ב-HTML
        Console.WriteLine("All HTML Tags:");
        foreach (var tag in htmlHelper.AllTags)
        {
            Console.WriteLine(tag);  // הדפסת כל תג
        }

        // הדפסת כל התגים הסגורים באופן אוטומטי (Self-Closing)
        Console.WriteLine("\nSelf-Closing HTML Tags:");
        foreach (var tag in htmlHelper.SelfClosingTags)
        {
            Console.WriteLine(tag);  // הדפסת כל תג סגור אוטומטית
        }

        // טוען את תוכן הדף מ-URL מסוים
        var html = await Load("https://hebrewbooks.org/");

        // המרת תוכן ה-HTML לרשימת אלמנטים מסוג HtmlElement
        var elements = ParseHtmlToElements(html);

        // הדפסת כל האלמנטים
        foreach (var element in elements)
        {
            Console.WriteLine(element);  // הדפסת האלמנט
        }

        // יצירת Selector לפי שאילתה CSS
        var selector = Selector.Parse("div#ftritem2");

        // חיפוש אלמנטים שמתאימים ל-selector
        var matches = elements.SelectMany(e => e.Descendants())  // מוציאים את כל האלמנטים בתור Descendants של כל אלמנט
                              .Where(e => MatchesSelector(e, selector))  // בודקים אם האלמנט תואם ל-selector
                              .ToList();

        // הדפסת האלמנטים שמתאימים לשאילתה
        Console.WriteLine("Matching elements:");
        foreach (var match in matches)
        {
            Console.WriteLine(match.ToFormattedString());  // הדפסת האלמנט בצורה מעוצבת
        }
    }

    // פונקציה שמטענת תוכן HTML מ-URL
    static async Task<string> Load(string url)
    {
        HttpClient client = new HttpClient();  // יצירת לקוח HTTP
        var response = await client.GetAsync(url);  // שליחה של בקשה ל-URL
        var html = await response.Content.ReadAsStringAsync();  // קריאת תוכן ה-HTML מהמענה
        return html;  // מחזיר את ה-HTML
    }

    // פונקציה שמפרקת את תוכן ה-HTML לרשימת אלמנטים מסוג HtmlElement
    static List<HtmlElement> ParseHtmlToElements(string html)
    {
        var elements = new List<HtmlElement>();  // יצירת רשימה לאחסון האלמנטים
        var regex = new Regex("<(.*?)>");  // רגקס למציאת תגיות HTML
        var matches = regex.Matches(html);  // חיפוש כל ההתאמות לפי הרגקס

        HtmlElement currentElement = null;  // משתנה לשמירת האלמנט הנוכחי

        foreach (Match match in matches)
        {
            var tag = match.Groups[1].Value;  // קבלת שם התג

            // אם התג מתחיל ב-"/", מדובר בתג סגירה, לכן נזוז לאלמנט האב
            if (tag.StartsWith("/"))
            {
                currentElement = currentElement.Parent;
            }
            else
            {
                // יצירת אלמנט חדש
                var newElement = new HtmlElement { Name = tag };

                // אם יש לנו אלמנט הורה, נוסיף אותו כרשומה של הילד
                if (currentElement != null)
                {
                    currentElement.AddChild(newElement);
                }

                // נוסיף את האלמנט לרשימה
                elements.Add(newElement);

                // אם התג לא מסתיים ב"/", נעבור לאלמנט החדש כאלמנט הנוכחי
                if (!tag.EndsWith("/"))
                {
                    currentElement = newElement;
                }
            }
        }

        return elements;  // מחזירים את רשימת האלמנטים
    }

    // פונקציה שבודקת אם האלמנט תואם ל-selector
    static bool MatchesSelector(HtmlElement element, Selector selector)
    {
        // אם יש שם תג ב-selector והאלמנט לא תואם, נחזיר false
        if (!string.IsNullOrEmpty(selector.TagName) && element.Name != selector.TagName)
        {
            return false;
        }

        // אם יש מזהה (id) ב-selector והאלמנט לא תואם, נחזיר false
        if (!string.IsNullOrEmpty(selector.Id) && element.Id != selector.Id)
        {
            return false;
        }

        // אם יש מחלקות ב-selector והאלמנט לא תואם לכל המחלקות, נחזיר false
        if (selector.Classes.Any() && !selector.Classes.All(c => element.Classes.Contains(c)))
        {
            return false;
        }

        // אם כל התנאים עברו בהצלחה, נחזיר true
        return true;
    }
}
