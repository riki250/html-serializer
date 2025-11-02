# HtmlCrawler

HtmlCrawler is a simple C# project for web scraping HTML content from websites.  
It loads HTML, parses it into a hierarchical structure of elements, and allows querying using CSS selectors.

## Features
- Load HTML content from a URL.
- Parse HTML into a tree structure (`HtmlElement`) with parent-child relationships.
- List all HTML tags and self-closing tags.
- Query HTML elements using CSS selectors (tag, id, class).

## Requirements
- Visual Studio 2019 or 2022
- .NET 6.0

## Installation & Running
1. Open the `HtmlCrawler.sln` solution in Visual Studio.
2. Set `HtmlCrawler` as the startup project.
3. Press F5 to run.
4. Modify the URL in `Program.cs` to scrape different websites.

## Example Usage
```csharp
var selector = Selector.Parse("div#ftritem2");
var matches = elements.Where(e => MatchesSelector(e, selector)).ToList();
