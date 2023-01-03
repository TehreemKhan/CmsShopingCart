﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShopingCart.Infrastructure
{
    public class FileExtensionAttribute :ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null) {
                var extension = Path.GetExtension(file.FileName);

                //allowed extention
                string[] extensions = { "jpg","jpeg","png", "webp" };
                bool result = extensions.Any(x => extension.EndsWith(x));
                if (!result) {
                    return new ValidationResult(GetErrorMessage());
                }
            }
            return ValidationResult.Success;
        }
        private string GetErrorMessage() {
            return "Allowed extensions are jpg, jpeg, png, webp";
        }
    }
}
