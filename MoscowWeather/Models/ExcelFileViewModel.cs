using Microsoft.AspNetCore.Http;
using MoscowWeather.Consts;
using MoscowWeather.CustomAttributes;
using MoscowWeather.DbModels;
using MoscowWeather.Dtos;
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
        private readonly ViewModelConsts _viewModelConsts = new ViewModelConsts();

        [Required(ErrorMessage = "Пожалуйста, выберете файл.")]
        [DataType(DataType.Upload)]
        [AllowedFileExtension(new string[] { ".xlsx" })]
        public List<IFormFile> ExcelFiles { get; set; } = new List<IFormFile>();

        public void ReadFromExcel(MemoryStream excelFileStream, MoscowWeatherContext dbConnection)
        {
            excelFileStream.Position = 0;
            var workBook = new XSSFWorkbook(excelFileStream);

            var excelRowDtos = new List<ExcelRowDto>();

            for (var sheetIndex = 0; sheetIndex < workBook.NumberOfSheets; sheetIndex++)
            {
                var sheet = workBook.GetSheetAt(sheetIndex);

                ProcessSheetHeaders(sheet);
                excelRowDtos.AddRange(GetDataFromRowColumns(sheet));
            }

            ProcessRows(excelRowDtos, dbConnection);
        }

        private void ProcessSheetHeaders(ISheet sheet)
        {
            var excelFileDto = new ExcelContentHeaderDto();

            int headerOffset = sheet.FirstRowNum + 2;

            var headerRow = sheet.GetRow(headerOffset);
            var subHeaderRow = sheet.GetRow(headerOffset + 1);

            if (headerRow is null || headerRow.Cells.All(d => d.CellType == CellType.Blank))
            {
                throw new ArgumentNullException("Пустые поля в заголовке документа.");
            }

            const int headerCellsCount = 12;
            if (headerRow.LastCellNum != headerCellsCount)
            {
                throw new ArgumentException("Количество требуемых полей не соответствует допустимому.");
            }

            for (int headerCell = headerRow.FirstCellNum; headerCell < headerCellsCount; headerCell++)
            {
                var columnName = headerRow.GetCell(headerCell).ToString() ?? string.Empty;
                columnName += " " + subHeaderRow.GetCell(headerCell).ToString() ?? string.Empty;

                excelFileDto.Headers.Add(columnName.Trim());
            }

            bool hasCorrectHeaders =
                excelFileDto.Headers[0] == _viewModelConsts.Date &&
                excelFileDto.Headers[1] == _viewModelConsts.Time &&
                excelFileDto.Headers[2] == _viewModelConsts.Temperature &&
                excelFileDto.Headers[3] == _viewModelConsts.Humidity &&
                excelFileDto.Headers[4] == _viewModelConsts.Td &&
                excelFileDto.Headers[5] == _viewModelConsts.AtmosphericPressure &&
                excelFileDto.Headers[6] == _viewModelConsts.WindDirection &&
                excelFileDto.Headers[7] == _viewModelConsts.WindSpeed &&
                excelFileDto.Headers[8] == _viewModelConsts.Cloudiness &&
                excelFileDto.Headers[9] == _viewModelConsts.H &&
                excelFileDto.Headers[10] == _viewModelConsts.Vv &&
                excelFileDto.Headers[11] == _viewModelConsts.WeatherCondition;

            if (!hasCorrectHeaders)
                throw new ArgumentException("Некорректные поля в файле.");
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

        private List<ExcelRowDto> GetDataFromRowColumns(ISheet sheet)
        {
            var rowDtos = new List<ExcelRowDto>();

            const int cellCount = 12;
            int dataStartRowNum = (sheet.FirstRowNum + 4);

            for (int dataRow = dataStartRowNum; dataRow <= sheet.LastRowNum; dataRow++)
            {
                var row = sheet.GetRow(dataRow);

                var dataRowDto = new ExcelRowDto();

                if (row is null)
                    continue;
                if (row.Cells.All(d => d.CellType == CellType.Blank))
                    continue;

                for (var cellNum = row.FirstCellNum; cellNum < cellCount; cellNum++)
                {
                    if (row.GetCell(cellNum) != null)
                    {
                        dataRowDto.RowColumns.Add(row.GetCell(cellNum).ToString().Trim());
                    }
                    else
                    {
                        dataRowDto.RowColumns.Add(string.Empty);
                    }
                }

                rowDtos.Add(dataRowDto);
            }

            return rowDtos;
        }
    }
}
