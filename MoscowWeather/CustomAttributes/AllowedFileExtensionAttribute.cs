using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace MoscowWeather.CustomAttributes
{
    public class AllowedFileExtensionAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;

        public AllowedFileExtensionAttribute(string [] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var files = value as List<IFormFile>;

            if (files.Count <= 0)
                return new ValidationResult(GetErrorMessage());

            foreach (var file in files)
            {
                var extension = Path.GetExtension(file.FileName);

                if (file != null)
                {
                    if (!_extensions.Contains(extension.ToLower()))
                    {
                        return new ValidationResult(GetErrorMessage());
                    }
                }
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            string errorMessage =
                "Неподдерживаемое расширение. Используйте: \r\n";

            foreach (var extension in _extensions)
            {
                errorMessage += extension + "\r\n";
            }

            return errorMessage;
        }
    }
}
