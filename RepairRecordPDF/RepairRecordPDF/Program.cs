using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;
using RepairRecordPDF;
using SaturdayAPI.Core.Types;
using SaturdayAPI.Wrapper;
using Action = SaturdayAPI.Core.Types.Action;

bool outputImg = true; //true为输出图片，false为输出pdf
QuestPDF.Settings.License = LicenseType.Community;

var outputDir = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "output"));
if (!outputDir.Exists)
    outputDir.Create();
var api = new ApiWithCache(Path.Combine(Environment.CurrentDirectory, "cache"));
string FormatTime(DateTimeOffset time) => $"{time:yyyy-MM-dd HH:mm}";
var logoSvg = new Svg.Skia.SKSvg();
logoSvg.Load("CA-logo.svg");
foreach (var eventInfo in (await api.GetEvents()).Skip(1))
{
    if (eventInfo.Status != Status.Closed)
    {
        Console.WriteLine("跳过：" + eventInfo.EventId);
        continue; //跳过未关闭的事件
    }
#if !DEBUG
    var fileName =
        $"{eventInfo.GmtCreate:yyyy-MM-dd}_{eventInfo.Member.MemberId}" + (outputImg ? "" : ".pdf");
    var filePath = Path.Combine(outputDir.Name, fileName);
    if (File.Exists(filePath))
        continue; //已经生成过了
    Console.WriteLine("生成：" + fileName);
#endif
    var logs = eventInfo.Logs ?? (await api.GetEventById(eventInfo.EventId)).Logs!;
    var doc = Document
        .Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A5);
                page.Margin(1, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(15).FontFamily("Microsoft YaHei"));
                page.Header()
                    .Container()
                    .Table(t =>
                    {
                        t.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });
                        t.Cell()
                            .Row(1)
                            .Column(1)
                            .AlignMiddle()
                            .Text("维修记录单")
                            .Underline()
                            .SemiBold()
                            .FontSize(30)
                            .FontColor(Colors.Blue.Medium);
                        t.Cell().Row(1).Column(2).AlignRight().Width(80).Svg(logoSvg);
                    });

                //x.Item().AlignCenter().Width(100).Image("CA-logo.png");
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
                                    columns.RelativeColumn(1);
                                    columns.RelativeColumn(3);
                                });
                                uint currentLine = 0;
                                void Line(
                                    string type,
                                    string? content = null,
                                    DateTimeOffset? timeOrNull = null
                                )
                                {
                                    currentLine++;
                                    Cell(currentLine, 1).Text(type);
                                    if (timeOrNull is { } time)
                                    {
                                        Cell(currentLine, 2)
                                            .AlignCenter()
                                            .Container()
                                            .Column(t =>
                                            {
                                                t.Item()
                                                    .AlignCenter()
                                                    .Text(FormatTime(time))
                                                    .FontColor(Colors.Blue.Darken1)
                                                    .Italic();
                                                if (content is not null)
                                                {
                                                    var text = t.Item()
                                                        .AlignCenter()
                                                        .Text(content)
                                                        .FontColor(Colors.Green.Darken2);
                                                    if (content.Length > 50)
                                                    {
                                                        text.FontSize(13);
                                                    }
                                                }
                                            });
                                    }
                                    else
                                    {
                                        Cell(currentLine, 2).AlignCenter().Text(content);
                                    }
                                    IContainer Cell(uint row, uint column) =>
                                        table
                                            .Cell()
                                            .Row(row)
                                            .Column(column)
                                            .Element(
                                                c =>
                                                    c.BorderBottom(1)
                                                        .BorderColor(Colors.Grey.Lighten2)
                                                        .ShowOnce()
                                                        .MinWidth(50)
                                                        .MinHeight(25)
                                                        .AlignCenter()
                                                        .AlignMiddle()
                                            );
                                }
                                Line("报修人", $"{eventInfo.Member}");
                                Line("创建日期", timeOrNull: eventInfo.GmtCreate);
                                Line("结束日期", timeOrNull: eventInfo.GmtModified);
                                Line("型号", $"{eventInfo.Model}");
                                Line("问题描述", $"{eventInfo.Problem}");
                                Line("处理人", $"{eventInfo.ClosedBy}");
                                foreach (var eventLog in logs)
                                {
                                    switch (eventLog.Action)
                                    {
                                        case Action.Create:
                                            Line("+ 创建", timeOrNull: eventLog.GmtCreate);
                                            break;
                                        case Action.Accept:
                                            Line("+ 接受", timeOrNull: eventLog.GmtCreate);
                                            break;
                                        case Action.Cancel:
                                            Line("+ 取消", timeOrNull: eventLog.GmtCreate);
                                            break;
                                        case Action.Drop:
                                            Line("- 丢弃", timeOrNull: eventLog.GmtCreate);
                                            break;
                                        case Action.Commit:
                                            Line(
                                                "+ 进度",
                                                $"{eventLog.Description}",
                                                eventLog.GmtCreate
                                            );
                                            break;
                                        case Action.Reject:
                                            Line("- 拒绝", timeOrNull: eventLog.GmtCreate);
                                            break;
                                        case Action.Close:
                                            Line("- 关闭", timeOrNull: eventLog.GmtCreate);
                                            break;
                                        case Action.Update:
                                            Line(
                                                "+",
                                                $"{eventLog.Description}",
                                                eventLog.GmtCreate
                                            );
                                            break;
                                        default:
                                            Line(
                                                $"+{eventLog.Action}",
                                                timeOrNull: eventLog.GmtCreate
                                            );
                                            break;
                                    }
                                }
                            });
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
        .ShowInPreviewer(); //预览
#else
        //添加元数据
        .WithMetadata(
            new DocumentMetadata
            {
                Author = "NBTCA",
                CreationDate = eventInfo.GmtModified.DateTime,
            }
        );
    if (outputImg)
    { //直接输出图片
        doc.GenerateImages(index => filePath + "_" + index + ".png");
    }
    else
    { //输出pdf
        doc.GeneratePdf(filePath);
    }
#endif
}
