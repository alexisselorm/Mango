using System.ComponentModel.DataAnnotations;

namespace Mango.Web.Utility
{
    public class AllowExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;
        public AllowExtensionsAttribute(string[] extensions)
        {
            _extensions = extensions;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);

                foreach (var item in _extensions)
                {
                    Console.WriteLine(item);
                }

                Console.WriteLine(extension);
                if (!_extensions.Contains(extension.ToLower()))
                {
                    return new ValidationResult("This extension is not allowed");
                }
            }

            return ValidationResult.Success;
        }
    }
}
