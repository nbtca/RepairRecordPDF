using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;
//Newtonsoft.Json.Linq.JObject

QuestPDF.Settings.License = LicenseType.Community;
// code in your main method
Document.Create(container =>
    {
        container.Page(page =>
        {
            page.Size(PageSizes.A4);
            page.Margin(2, Unit.Centimetre);
            page.PageColor(Colors.White);
            page.DefaultTextStyle(x => x.FontSize(20).FontFamily("Microsoft YaHei"));
            page.Header()
                .Text("维修记录")
                .SemiBold().FontSize(36).FontColor(Colors.Blue.Medium);
            page.Content()
                .PaddingVertical(1, Unit.Centimetre)
                .Column(x =>
                {
                    x.Spacing(20);
                    x.Item().Text(Placeholders.LoremIpsum());
                    x.Item().Image(Placeholders.Image(200, 100));
                });

            page.Footer()
                .AlignCenter()
                .Text(x =>
                {
                    x.Span("Page ");
                    x.CurrentPageNumber();
                });
        });
    })
#if DEBUG
    .ShowInPreviewer();
#else
    .GeneratePdf("hello.pdf");
#endif