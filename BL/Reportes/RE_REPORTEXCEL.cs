using BE.Almacen;
using DA.Almacen;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Reportes
{
    public class RE_REPORTEXCEL
    {
        public static Stream CreateGetRepStockxAlmacen(int idCompany, string idWarhouse)
        {
            var list02 = TRA_WAREHOUSEDA.GetRepStockxAlmacen(idCompany, idWarhouse);
            var ms = new MemoryStream();
            using (var document = SpreadsheetDocument.Create(ms, SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();
                var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet();

                var stylePart = workbookPart.AddNewPart<WorkbookStylesPart>();
                stylePart.Stylesheet = CreateStylesheet();
                stylePart.Stylesheet.Save();

                worksheetPart.Worksheet.AppendChild(CreateColumns());

                var sheets = workbookPart.Workbook.AppendChild(new Sheets());
                var sheet = new Sheet { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "StockXAlmacen" };
                sheets.Append(sheet);
                workbookPart.Workbook.Save();

                var sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());
                CreateTitle(sheetData, idWarhouse);
                sheetData.AppendChild(CreateHeaders());
                var fill = false;
                foreach (var item in list02)
                {
                    sheetData.AppendChild(CreateRow(item, fill ? 7u : 10u));
                    fill = !fill;
                }
                worksheetPart.Worksheet.Save();
            }
            return ms;
        }

        private static Stylesheet CreateStylesheet()
        {
            var fonts = new Fonts(
                new Font(new FontSize { Val = 11 }), //INDEX 0 - Default
                new Font(new FontSize { Val = 11 }, new Bold()), //INDEX 1 - Con negrita
                new Font(new FontSize { Val = 11 }, new Bold(), new Color { Rgb = new HexBinaryValue("FFFFFF") }), //INDEX 2 - Header Tabla
                new Font(new FontSize { Val = 20 }, new Bold(), new Italic()) //INDEX 3 - titulo
            );

            var fills = new Fills(
                new Fill(new PatternFill { PatternType = PatternValues.None }), //INDEX 0 - Default
                new Fill(new PatternFill { PatternType = PatternValues.Gray125 }), //INDEX 1
                new Fill(
                    new PatternFill(
                        new ForegroundColor { Rgb = new HexBinaryValue("0D0D0D") },
                        new BackgroundColor { Indexed = 64u }
                    ) { PatternType = PatternValues.Solid }), //INDEX 2 - Header tabla
                new Fill(
                    new PatternFill(
                        new ForegroundColor { Rgb = new HexBinaryValue("D9D9D9") },
                        new BackgroundColor { Indexed = 64u }
                    ) { PatternType = PatternValues.Solid }) //INDEX 3 - Body row
            );

            var borders = new Borders(
                new Border(), //INDEX 0 - default
                new Border( //INDEX 1 - Header Left
                    new LeftBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                    new TopBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                    new BottomBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin }
                ),
                new Border( //INDEX 2 - Header Middle
                    new TopBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                    new BottomBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin }
                ),
                new Border( //INDEX 3 - Header Middle
                    new RightBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                    new TopBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                    new BottomBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin }
                ),
                new Border( //INDEX 4 - Body Left
                    new LeftBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                    new BottomBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin }
                ),
                new Border( //INDEX 5 - Body Middle
                    new BottomBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin }
                ),
                new Border( //INDEX 6 - Body Middle
                    new RightBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                    new BottomBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin }
                )
            );

            var cellFormats = new CellFormats(
                new CellFormat(), //INDEX 0 - default
                new CellFormat { FontId = 3, BorderId = 0, FillId = 0 }, //INDEX 1 - title
                new CellFormat { FontId = 1, BorderId = 0, FillId = 0 }, //INDEX 2 - Label
                new CellFormat { FontId = 2, BorderId = 1, FillId = 2 }, //INDEX 3 - Table header left
                new CellFormat { FontId = 2, BorderId = 2, FillId = 2 }, //INDEX 4 - Table header middle
                new CellFormat { FontId = 2, BorderId = 3, FillId = 2 }, //INDEX 5 - Table header right
                new CellFormat { FontId = 0, BorderId = 4, FillId = 0 }, //INDEX 6 - table body
                new CellFormat { FontId = 0, BorderId = 5, FillId = 0 }, //INDEX 7 - table body
                new CellFormat { FontId = 0, BorderId = 6, FillId = 0 }, //INDEX 8 - table body
                new CellFormat { FontId = 0, BorderId = 4, FillId = 3 }, //INDEX 9 - table body
                new CellFormat { FontId = 0, BorderId = 5, FillId = 3 }, //INDEX 10 - table body
                new CellFormat { FontId = 0, BorderId = 6, FillId = 3 }  //INDEX 11 - table body
            );

            return new Stylesheet(fonts, fills, borders, cellFormats);
        }

        private static Columns CreateColumns()
        {
            return new Columns(
                new Column { Min = 1, Max = 1, Width = 9.86, CustomWidth = true }, //CODIGO
                new Column { Min = 2, Max = 2, Width = 45.43, CustomWidth = true }, //DESCRIPCION
                new Column { Min = 3, Max = 3, Width = 17.29, CustomWidth = true }, //PRESENTACION
                new Column { Min = 4, Max = 4, Width = 8.14, CustomWidth = true }, //UNIDAD
                new Column { Min = 5, Max = 5, Width = 10.71, CustomWidth = true } //STOCK
            );
        }

        private static void CreateTitle(SheetData sheetData, string idWarhouse)
        {
            var row = new Row();
            row.AppendChild(CreateCell("Reporte de Stock por Almacen", CellValues.String, 1));
            sheetData.AppendChild(row);
            sheetData.AppendChild(new Row());
            row = new Row();
            row.AppendChild(CreateCell("Almacen :", CellValues.String, 2));
            row.AppendChild(CreateCell(idWarhouse, CellValues.String));
            sheetData.AppendChild(row);
            sheetData.AppendChild(new Row());
        }

        private static Row CreateHeaders()
        {
            var row = new Row();
            row.Append(CreateCell("Código", CellValues.String, 3),
                CreateCell("Descripción", CellValues.String, 4),
                CreateCell("Presentación", CellValues.String, 4),
                CreateCell("Unidad", CellValues.String, 4),
                CreateCell("Stock", CellValues.String, 5)
            );
            return row;
        }

        private static Row CreateRow(ERE_LISTA02 item, uint styleIndex)
        {
            var row = new Row();
            row.Append(
                CreateCell(item.CODIGO.ToString(), CellValues.String, styleIndex - 1),
                CreateCell(item.DESCRIPCION, CellValues.String, styleIndex),
                CreateCell(item.MODELO, CellValues.String, styleIndex),
                CreateCell(item.UND, CellValues.String, styleIndex),
                CreateCell(item.STOCK.ToString(), CellValues.Number, styleIndex + 1)
            );
            return row;
        }

        private static Cell CreateCell(string value, CellValues type, uint styleIndex = 0) => new Cell
        {
            CellValue = new CellValue(value),
            DataType = new EnumValue<CellValues>(type),
            StyleIndex = styleIndex
        };
    }
}
