using System;
using System.Collections.Generic;

// 1. QuestPDF Namespaces
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

using PRS.Models;

namespace PRS.Reports;

public class ParticipantsReport : IDocument
{
    private readonly string _eventName;
    private readonly DateOnly _startDate;
    private readonly DateOnly _endDate;
    private readonly string _location;
    private readonly List<Registrant> _registrants;

    public ParticipantsReport(string eventName, DateOnly startDate, DateOnly endDate, string location, List<Registrant> registrants)
    {
        _eventName = eventName;
        _startDate = startDate;
        _endDate = endDate;
        _location = location;
        _registrants = registrants;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Size(PageSizes.A4.Landscape());
            page.Margin(20);
            page.DefaultTextStyle(x => x.FontSize(10));

            page.Header().Column(col =>
            {
                col.Item().Text(_eventName)
                    .FontSize(16)
                    .Bold();

                col.Item().Text($"{_startDate:MMM dd} - {_endDate:MMM dd, yyyy} · {_location}")
                    .FontSize(10)
                    .FontColor(Colors.Grey.Darken1);

                col.Item().PaddingTop(5).Text($"Printed: {DateTime.Now:MMM dd, yyyy hh:mm tt}")
                    .FontSize(9)
                    .FontColor(Colors.Grey.Medium);
            });

            page.Content().PaddingVertical(10).Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(30);    // #
                    columns.RelativeColumn(2);     // Name
                    columns.ConstantColumn(60);    // Gender
                    columns.RelativeColumn(2);     // Contact
                    columns.RelativeColumn(3);     // Email
                    columns.RelativeColumn(4);     // Address
                });

                // Header
                table.Header(header =>
                {
                    header.Cell().Element(HeaderStyle).Text("#").Bold();
                    header.Cell().Element(HeaderStyle).Text("Full Name").Bold();
                    header.Cell().Element(HeaderStyle).Text("Gender").Bold();
                    header.Cell().Element(HeaderStyle).Text("Contact").Bold();
                    header.Cell().Element(HeaderStyle).Text("Email").Bold();
                    header.Cell().Element(HeaderStyle).Text("Address").Bold();

                    static IContainer HeaderStyle(IContainer container)
                    {
                        return container
                            .Background(Colors.Grey.Lighten3)
                            .Padding(5);
                    }
                });

                int i = 1;

                foreach (var r in _registrants)
                {
                    // Alternate row colors for better readability
                    var backgroundColor = i % 2 == 0 ? Colors.Grey.Lighten4 : Colors.White;

                    table.Cell().Element(c => CellStyle(c, backgroundColor)).Text(i.ToString());
                    table.Cell().Element(c => CellStyle(c, backgroundColor)).Text($"{r.FirstName} {r.LastName}");
                    table.Cell().Element(c => CellStyle(c, backgroundColor)).Text(r.Gender ?? string.Empty);
                    table.Cell().Element(c => CellStyle(c, backgroundColor)).Text(r.ContactNo ?? string.Empty);
                    
                    // Wrapping anywhere is useful for emails so they don't push column widths out
                    table.Cell().Element(c => CellStyle(c, backgroundColor)).Text(r.Email ?? string.Empty);
                    
                    table.Cell().Element(c => CellStyle(c, backgroundColor)).Text(r.Address ?? string.Empty);

                    i++;

                    static IContainer CellStyle(IContainer container, string bgColor)
                    {
                        return container
                            .Background(bgColor)
                            .Padding(5)
                            .BorderBottom(1)
                            .BorderColor(Colors.Grey.Lighten2);
                    }
                }
            });

            // Added Footer for Page Numbering
            page.Footer()
                .AlignCenter()
                .Text(x =>
                {
                    x.Span("Page ").FontSize(9).FontColor(Colors.Grey.Medium);
                    x.CurrentPageNumber().FontSize(9).FontColor(Colors.Grey.Medium);
                    x.Span(" of ").FontSize(9).FontColor(Colors.Grey.Medium);
                    x.TotalPages().FontSize(9).FontColor(Colors.Grey.Medium);
                });
        });
    }
}