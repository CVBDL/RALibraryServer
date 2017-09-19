using RaLibrary.Data.Context;
using RaLibrary.Data.Entities;
using System;
using System.Data.Entity.Migrations;

namespace RaLibrary.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<RaLibraryContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(RaLibraryContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            context.Books.AddOrUpdate(x => x.Id,
                new Book()
                {
                    Id = 1,
                    Code = "P001",
                    ISBN10 = "0596008031",
                    ISBN13 = "9780596008031",
                    Title = "Designing Interfaces",
                    Subtitle = "Patterns for Effective Interaction Design",
                    Publisher = "\"O'Reilly Media, Inc.\"",
                    Authors = "Jenifer Tidwell",
                    PublishedDate = "2005-11-21",
                    Description = "Provides information on designing easy-to-use interfaces.",
                    PageCount = 331,
                    ThumbnailLink = "http://books.google.com/books/content?id=1D2bAgAAQBAJ&printsec=frontcover&img=1&zoom=1&edge=curl&source=gbs_api",
                    CreatedDate = DateTime.Now
                },
                new Book()
                {
                    Id = 2,
                    Code = "P002",
                    ISBN10 = "7111348664",
                    ISBN13 = "9787111348665",
                    Title = "以用户为中心的产品设计",
                    Subtitle = "",
                    Authors = "Jesse James Garrett",
                    PublishedDate = "2011",
                    PageCount = 191,
                    CreatedDate = DateTime.Now
                },
                new Book()
                {
                    Id = 3,
                    Code = "P003",
                    ISBN10 = "7115313083",
                    ISBN13 = "9787115313089",
                    Title = "Designing Interfaces",
                    Subtitle = "Patterns for Effective Interaction Design",
                    Authors = "[美] Susan Weinschenk",
                    Publisher = "人民邮电出版社",
                    PublishedDate = "2013",
                    Description = "本书出自国际知名的设计心理学专家之手,讨论了设计师必须知道的100个心理学问题.",
                    PageCount = 236,
                    CreatedDate = DateTime.Now
                },
                new Book()
                {
                    Id = 4,
                    Code = "P004",
                    ISBN10 = "7513902062",
                    ISBN13 = "9787513902069",
                    Title = "全球英语写作经典",
                    Subtitle = "",
                    Authors = "William Strunk, Jr.",
                    Publisher = "民主与建设出版社",
                    PublishedDate = "2013",
                    Description = "本书阐述了英语写作风格的基本构成要素以及写作时容易误用的规则和用法. 全书共分为六大章, 其中以第二章(英语用法的八个基本规则)和第三章(英语写作的十个基本原理)共18个规则为最重要的写作原则, 读者在阅读本书时可以按照这个顺序循序渐进地进行学习.",
                    PageCount = 187,
                    CreatedDate = DateTime.Now
                },
                new Book()
                {
                    Id = 5,
                    Code = "P005",
                    ISBN10 = "0596554877",
                    ISBN13 = "9780596554873",
                    Title = "JavaScript: The Good Parts",
                    Subtitle = "The Good Parts",
                    Authors = "Douglas Crockford",
                    Publisher = "\"O'Reilly Media, Inc.\"",
                    PublishedDate = "2008-05-08",
                    Description = "Most programming languages contain good and bad parts, but JavaScript has more than its share of the bad, having been developed and released in a hurry before it could be refined. This authoritative book scrapes away these bad features to reveal a subset of JavaScript that's more reliable, readable, and maintainable than the language as a whole—a subset you can use to create truly extensible and efficient code. Considered the JavaScript expert by many people in the development community, author Douglas Crockford identifies the abundance of good ideas that make JavaScript an outstanding object-oriented programming language-ideas such as functions, loose typing, dynamic objects, and an expressive object literal notation. Unfortunately, these good ideas are mixed in with bad and downright awful ideas, like a programming model based on global variables. When Java applets failed, JavaScript became the language of the Web by default, making its popularity almost completely independent of its qualities as a programming language. In JavaScript: The Good Parts, Crockford finally digs through the steaming pile of good intentions and blunders to give you a detailed look at all the genuinely elegant parts of JavaScript, including: Syntax Objects Functions Inheritance Arrays Regular expressions Methods Style Beautiful features The real beauty? As you move ahead with the subset of JavaScript that this book presents, you'll also sidestep the need to unlearn all the bad parts. Of course, if you want to find out more about the bad parts and how to use them badly, simply consult any other JavaScript book. With JavaScript: The Good Parts, you'll discover a beautiful, elegant, lightweight and highly expressive language that lets you create effective code, whether you're managing object libraries or just trying to get Ajax to run fast. If you develop sites or applications for the Web, this book is an absolute must.",
                    PageCount = 172,
                    ThumbnailLink = "http://books.google.com/books/content?id=PXa2bby0oQ0C&printsec=frontcover&img=1&zoom=1&edge=curl&source=gbs_api",
                    CreatedDate = DateTime.Now
                },
                new Book()
                {
                    Id = 6,
                    Code = "P006",
                    ISBN10 = "0596554478",
                    ISBN13 = "9780596554477",
                    Title = "JavaScript: The Definitive Guide",
                    Subtitle = "The Definitive Guide",
                    Authors = "David Flanagan",
                    Publisher = "\"O'Reilly Media, Inc.\"",
                    PublishedDate = "2006-08-17",
                    Description = "This Fifth Edition is completely revised and expanded to cover JavaScript as it is used in today's Web 2.0 applications. This book is both an example-driven programmer's guide and a keep-on-your-desk reference, with new chapters that explain everything you need to know to get the most out of JavaScript, including: Scripted HTTP and Ajax XML processing Client-side graphics using the canvas tag Namespaces in JavaScript--essential when writing complex programs Classes, closures, persistence, Flash, and JavaScript embedded in Java applications Part I explains the core JavaScript language in detail. If you are new to JavaScript, it will teach you the language. If you are already a JavaScript programmer, Part I will sharpen your skills and deepen your understanding of the language. Part II explains the scripting environment provided by web browsers, with a focus on DOM scripting with unobtrusive JavaScript. The broad and deep coverage of client-side JavaScript is illustrated with many sophisticated examples that demonstrate how to: Generate a table of contents for an HTML document Display DHTML animations Automate form validation Draw dynamic pie charts Make HTML elements draggable Define keyboard shortcuts for web applications Create Ajax-enabled tool tips Use XPath and XSLT on XML documents loaded with Ajax And much more Part III is a complete reference for core JavaScript. It documents every class, object, constructor, method, function, property, and constant defined by JavaScript 1.5 and ECMAScript Version 3. Part IV is a reference for client-side JavaScript, covering legacy web browser APIs, the standard Level 2 DOM API, and emerging standards such as the XMLHttpRequest object and the canvas tag. More than 300,000 JavaScript programmers around the world have madethis their indispensable reference book for building JavaScript applications. \"A must-have reference for expert JavaScript programmers...well-organized and detailed.\" -- Brendan Eich, creator of JavaScript",
                    PageCount = 1032,
                    ThumbnailLink = "http://books.google.com/books/content?id=2weL0iAfrEMC&printsec=frontcover&img=1&zoom=1&edge=curl&source=gbs_api",
                    CreatedDate = DateTime.Now
                }
            );
        }
    }
}
