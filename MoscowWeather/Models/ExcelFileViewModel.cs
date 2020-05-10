using Microsoft.AspNetCore.Http;
using MoscowWeather.CustomAttributes;
using MoscowWeather.DbModels;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;

namespace MoscowWeather.Models
{
    public class ExcelFileViewModel
    {
        [Required(ErrorMessage = "Пожалуйста, выберете файл.")]
        [DataType(DataType.Upload)]
        [AllowedFileExtension(new string[] { ".xls", ".xlsx" })]
        public List<IFormFile> ExcelFiles { get; set; } = new List<IFormFile>();

        public void ReadFromExcel(MemoryStream excelFileStream, MoscowWeatherContext dbConnection)
        {
            excelFileStream.Position = 0;
            var workBook = new XSSFWorkbook(excelFileStream);

            var rowList = new List<ExcelRowDto>();

            for (var sheetIndex = 0; sheetIndex < workBook.NumberOfSheets; sheetIndex++)
            {
                var sheet = workBook.GetSheetAt(sheetIndex);

                for (int dataRow = (sheet.FirstRowNum + 5); dataRow <= sheet.LastRowNum; dataRow++)
                {
                    var row = sheet.GetRow(dataRow);
                    if (row is null)
                        continue;
                    if (row.Cells.All(d => d.CellType == CellType.Blank))
                        continue;

                    var rowDto = new ExcelRowDto();

                    for (var cellNum = row.FirstCellNum; cellNum < 12; cellNum++)
                    {
                        if (row.GetCell(cellNum) != null)
                        {
                            rowDto.RowColumns.Add(row.GetCell(cellNum).ToString().Trim());
                        }
                        else
                        {
                            rowDto.RowColumns.Add(string.Empty);
                        }
                    }

                    rowList.Add(rowDto);
                }
            }

            ProcessRows(rowList, dbConnection);
        }

        private void ProcessRows(List<ExcelRowDto> excelRowDtos, MoscowWeatherContext dbConnection)
        {
            foreach (var dto in excelRowDtos)
            {
                var weatherModel = new Weather();

                if (!string.IsNullOrEmpty(dto.RowColumns[0]) && !string.IsNullOrWhiteSpace(dto.RowColumns[0]))
                    weatherModel.Date = DateTime.ParseExact(dto.RowColumns[0], "dd.MM.yyyy", CultureInfo.InvariantCulture);

                if (!string.IsNullOrEmpty(dto.RowColumns[1]) && !string.IsNullOrWhiteSpace(dto.RowColumns[1]))
                    weatherModel.Time = TimeSpan.Parse(dto.RowColumns[1]);

                if (!string.IsNullOrEmpty(dto.RowColumns[2]) && !string.IsNullOrWhiteSpace(dto.RowColumns[2]))
                    weatherModel.Temperature = decimal.Parse(dto.RowColumns[2]);

                if (!string.IsNullOrEmpty(dto.RowColumns[3]) && !string.IsNullOrWhiteSpace(dto.RowColumns[3]))
                    weatherModel.Humidity = decimal.Parse(dto.RowColumns[3]);

                if (!string.IsNullOrEmpty(dto.RowColumns[4]) && !string.IsNullOrWhiteSpace(dto.RowColumns[4]))
                    weatherModel.Td = decimal.Parse(dto.RowColumns[4]);

                if (!string.IsNullOrEmpty(dto.RowColumns[5]) && !string.IsNullOrWhiteSpace(dto.RowColumns[5]))
                    weatherModel.AtmosphericPressure = decimal.Parse(dto.RowColumns[5]);

                weatherModel.WindDirection = (dto.RowColumns[6]);

                if (!string.IsNullOrEmpty(dto.RowColumns[7]) && !string.IsNullOrWhiteSpace(dto.RowColumns[7]))
                    weatherModel.WindSpeed = decimal.Parse(dto.RowColumns[7]);

                if (!string.IsNullOrEmpty(dto.RowColumns[8]) && !string.IsNullOrWhiteSpace(dto.RowColumns[8]))
                    weatherModel.Cloudiness = decimal.Parse(dto.RowColumns[8]);

                if (!string.IsNullOrEmpty(dto.RowColumns[9]) && !string.IsNullOrWhiteSpace(dto.RowColumns[9]))
                    weatherModel.H = decimal.Parse(dto.RowColumns[9]);

                weatherModel.Vv = (dto.RowColumns[10]);

                weatherModel.WeatherCondition = dto.RowColumns[11];

                dbConnection.Add(weatherModel);
            }

            dbConnection.SaveChanges();
        }
    }
}
