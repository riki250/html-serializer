using System;
using System.IO;
using System.Text.Json;

public class HtmlHelper
{
    public string[] AllTags { get; private set; }  // כל התגים ב-HTML
    public string[] SelfClosingTags { get; private set; }  // תגים סגורים עצמיים ב-HTML

    private static HtmlHelper _instance;  // מופע בודד של HtmlHelper

    // בונה את המופע של HtmlHelper ומטפל בטעינת התגים מקבצים
    private HtmlHelper(string allTagsFilePath, string selfClosingTagsFilePath)
    {
        AllTags = LoadTags(allTagsFilePath);  // טוען את כל התגים
        SelfClosingTags = LoadTags(selfClosingTagsFilePath);  // טוען את התגים הסגורים עצמיים
    }

    // מחזיר את המופע היחיד של HtmlHelper (Singleton)
    public static HtmlHelper GetInstance(string allTagsFilePath, string selfClosingTagsFilePath)
    {
        if (_instance == null)
        {
            _instance = new HtmlHelper(allTagsFilePath, selfClosingTagsFilePath);  // יוצר את המופע אם הוא לא קיים
        }
        return _instance;
    }

    // טוען את התגים מקובץ JSON ומחזיר אותם כמערך
    private string[] LoadTags(string filePath)
    {
        try
        {
            var json = File.ReadAllText(filePath);  // קורא את תוכן הקובץ
            return JsonSerializer.Deserialize<string[]>(json);  // ממיר את התוכן למערך של תגים
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading tags from {filePath}: {ex.Message}");  // אם יש שגיאה, מציג הודעה
            return new string[0];  // מחזיר מערך ריק במקרה של שגיאה
        }
    }
}
