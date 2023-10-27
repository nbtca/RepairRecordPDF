using System.Security.Cryptography.X509Certificates;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;

//Newtonsoft.Json.Linq.JObject

QuestPDF.Settings.License = LicenseType.Community;

// code in your main method
Document
    .Create(container =>
    {
        container.Page(page =>
        {
            page.Size(PageSizes.A5);
            page.Margin(1, Unit.Centimetre);
            page.PageColor(Colors.White);
            page.DefaultTextStyle(x => x.FontSize(18).FontFamily("Microsoft YaHei"));
            page.Header()
                .Text("维修记录单")
                .Underline()
                .SemiBold()
                .FontSize(32)
                .FontColor(Colors.Blue.Medium);
            page.Content()
                .PaddingVertical(1, Unit.Centimetre)
                .Column(x =>
                {
                    x.Spacing(20);
                    x.Item()
                        .Border(1)
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });
                            uint currentLine = 0;
                            void Line(string type, string content)
                            {
                                currentLine++;
                                Cell(currentLine, 1).Text(type);
                                Cell(currentLine, 2).Text(content);
                                IContainer Cell(uint row, uint column) =>
                                    table
                                        .Cell()
                                        .Row(row)
                                        .Column(column)
                                        .Element(
                                            c =>
                                                c.Border(0)
                                                    .ShowOnce()
                                                    .MinWidth(50)
                                                    .MinHeight(50)
                                                    .AlignCenter()
                                                    .AlignMiddle()
                                        );
                            }
                            Line("报修人", "xxx");
                            Line("日期", "xxx");
                            Line("描述", "xxx\ntest");
                            Line("处理结果", "xxx");
                            Line("评价", "xxx");
                        });
                    //x.Item().Table(t =>
                    //{
                    //});
                    x.Item().AlignCenter().Width(100).Image("CA-logo.png");
                    //x.Item().Image(Placeholders.Image(200, 100));
                });
            page.Footer()
                .AlignCenter()
                .Text(x =>
                {
                    x.AlignCenter();
                    x.Span("浙大宁波理工学院");
                    x.EmptyLine();
                    x.Span("计算机协会").LineHeight(0.5f);
                    //x.Span("Page ");
                    //x.CurrentPageNumber();
                });
        });
    })
#if DEBUG
    .ShowInPreviewer();
#else
    .GeneratePdf("hello.pdf");
#endif
