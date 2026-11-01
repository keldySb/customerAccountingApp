using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using finalExam_diplom_.classes;
using System.Globalization;
using System.IO;

namespace finalExam_diplom_
{
    class MakeWordFile
    {

        private readonly string _templatePath;

        public MakeWordFile(string templatePath)
        {
            if (!File.Exists(templatePath))
                throw new ArgumentException("Шаблон не найден: " + templatePath);

            _templatePath = templatePath;
        }

        public bool Process(StatisticsClass statistic,Dictionary<string, string> items, string outputPath)
        {
            try
            {
                string folder = Path.GetDirectoryName(outputPath);

                if (!string.IsNullOrEmpty(folder))
                    Directory.CreateDirectory(folder);

                File.Copy(_templatePath, outputPath, true);

                using (WordprocessingDocument document =
                       WordprocessingDocument.Open(outputPath, true))
                {
                    var mainPart = document.MainDocumentPart;

                    if (mainPart == null || mainPart.Document == null)
                        throw new Exception("Не удалось прочитать документ Word.");

                    ReplaceInParagraphs(
                        mainPart.Document.Descendants<Paragraph>(),
                        items);

                    List<ChartPart> charts = mainPart.ChartParts.ToList();

                    if (charts.Count < 2)
                        throw new Exception("В шаблоне должно быть две диаграммы.");

                    UpdateStatusChart(charts[1], items);
                    UpdatePriorityChart(charts[0], items);

                    foreach (var header in mainPart.HeaderParts)
                    {
                        ReplaceInParagraphs(
                            header.Header.Descendants<Paragraph>(),
                            items);
                    }

                    foreach (var footer in mainPart.FooterParts)
                    {
                        ReplaceInParagraphs(
                            footer.Footer.Descendants<Paragraph>(),
                            items);
                    }

                    mainPart.Document.Save();
                }

                return true;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(
                    ex.ToString(),
                    "Ошибка формирования отчёта",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);

                return false;
            }
        }

        private static void ReplaceInParagraphs(
            IEnumerable<Paragraph> paragraphs,
            Dictionary<string, string> items)
        {
            foreach (Paragraph paragraph in paragraphs)
            {
                List<DocumentFormat.OpenXml.Wordprocessing.Text> textElements = new List<DocumentFormat.OpenXml.Wordprocessing.Text>();

                foreach (DocumentFormat.OpenXml.Wordprocessing.Text text in paragraph.Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>())
                {
                    textElements.Add(text);
                }

                if (textElements.Count == 0)
                    continue;

                string fullText = "";

                foreach (DocumentFormat.OpenXml.Wordprocessing.Text text in textElements)
                {
                    fullText += text.Text;
                }

                string updatedText = fullText;

                foreach (var item in items)
                {
                    updatedText = updatedText.Replace(
                        item.Key,
                        item.Value ?? "");
                }

                if (updatedText == fullText)
                    continue;

                textElements[0].Text = updatedText;
                textElements[0].Space =
                    SpaceProcessingModeValues.Preserve;

                for (int i = 1; i < textElements.Count; i++)
                {
                    textElements[i].Text = "";
                }
            }
        }

        private void UpdateStatusChart(
            ChartPart chartPart,
            Dictionary<string, string> items)
        {
            int newCount = int.Parse(items["<COUNTNEW>"]);
            int workCount = int.Parse(items["<COUNTWORK>"]);
            int finishCount = int.Parse(items["<COUNTFINISH>"]);
            int cancelCount = int.Parse(items["<COUNTCANCEL>"]);

            int[] counts =
            {
                    newCount,
                    workCount,
                    finishCount,
                    cancelCount
                    };

            EmbeddedPackagePart excelFile = chartPart.EmbeddedPackagePart;

            if (excelFile == null)
                throw new Exception("Excel-файл диаграммы не найден");

            using (Stream stream = excelFile.GetStream(
                FileMode.Open, FileAccess.ReadWrite))
            {
                using (SpreadsheetDocument ExcelDoc =
                    SpreadsheetDocument.Open(stream, true))
                {
                    WorkbookPart ExcelBook = ExcelDoc.WorkbookPart;

                    WorksheetPart ExcelSheet =
                        ExcelBook.WorksheetParts.First();

                    SheetData sheetData =
                        ExcelSheet.Worksheet.GetFirstChild<SheetData>();

                    string[] cellNames = { "B2", "B3", "B4", "B5" };

                    for (int i = 0; i < cellNames.Length; i++)
                    {
                        Cell cell = sheetData.Descendants<Cell>()
                            .FirstOrDefault(c => c.CellReference == cellNames[i]);

                        if (cell == null)
                            throw new Exception(
                                "Не найдена ячейка " + cellNames[i]);

                        cell.DataType = null;

                        cell.CellValue = new CellValue(
                            counts[i].ToString(CultureInfo.InvariantCulture));
                    }

                    ExcelSheet.Worksheet.Save();
                    ExcelBook.Workbook.Save();
                }
            }

            NumberingCache numberCache =
                chartPart.ChartSpace.Descendants<NumberingCache>()
                .FirstOrDefault();

            if (numberCache == null)
                throw new Exception("Кэш диаграммы не найден");

            int index = 0;

            foreach (NumericPoint point in
                     numberCache.Descendants<NumericPoint>())
            {
                if (index >= counts.Length)
                    break;

                point.NumericValue.Text =
                    counts[index].ToString(CultureInfo.InvariantCulture);

                index++;
            }

            chartPart.ChartSpace.Save();
        }


        private void UpdatePriorityChart(
            ChartPart chartPart,
            Dictionary<string, string> items)
        {
            int lowCount = int.Parse(items["<COUNTLOW>"]);
            int middleCount = int.Parse(items["<COUNTMIDDLE>"]);
            int highCount = int.Parse(items["<COUNTHIGH>"]);

            int[] counts =
            {
                lowCount,
                middleCount,
                highCount
            };

            if (chartPart == null)
                throw new Exception("Диаграмма не найдена.");

            EmbeddedPackagePart excelFile = chartPart.EmbeddedPackagePart;

            if (excelFile == null)
                throw new Exception("Excel-файл диаграммы не найден.");

            using (Stream stream = excelFile.GetStream(
                FileMode.Open, FileAccess.ReadWrite))
            {
                using (SpreadsheetDocument ExcelDoc =
                    SpreadsheetDocument.Open(stream, true))
                {
                    WorkbookPart ExcelBook = ExcelDoc.WorkbookPart;

                    WorksheetPart ExcelSheet =
                        ExcelBook.WorksheetParts.First();

                    SheetData sheetData =
                        ExcelSheet.Worksheet.GetFirstChild<SheetData>();

                    string[] cellNames = { "B2", "B3", "B4" };

                    for (int i = 0; i < cellNames.Length; i++)
                    {
                        Cell cell = sheetData.Descendants<Cell>()
                            .FirstOrDefault(c => c.CellReference == cellNames[i]);

                        if (cell == null)
                            throw new Exception(
                                "Не найдена ячейка " + cellNames[i]);

                        cell.DataType = null;

                        cell.CellValue = new CellValue(
                            counts[i].ToString(CultureInfo.InvariantCulture));
                    }

                    ExcelSheet.Worksheet.Save();
                    ExcelBook.Workbook.Save();
                }
            }

            NumberingCache numberCache =
                chartPart.ChartSpace.Descendants<NumberingCache>()
                .FirstOrDefault();

            if (numberCache == null)
                throw new Exception("Кэш диаграммы не найден");

            int index = 0;

            foreach (NumericPoint point in
                     numberCache.Descendants<NumericPoint>())
            {
                if (index >= counts.Length)
                    break;

                point.NumericValue.Text =
                    counts[index].ToString(CultureInfo.InvariantCulture);

                index++;
            }

            chartPart.ChartSpace.Save();
        }
    }
}

