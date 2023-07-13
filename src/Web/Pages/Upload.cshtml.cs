using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;


namespace Web.Pages
{
    public class UploadModel : PageModel
    {
        private readonly IFileUploadService _uploadService;

        public UploadModel(IFileUploadService uploadService)
        {
            _uploadService = uploadService;
        }

        [BindProperty] public InputModel Input { get; set; }
        public class InputModel
        {
            public IFormFile File { get; set; } 
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var outStream = new MemoryStream();

            using (var image = Image.Load(Input.File.OpenReadStream()) )
            {
                image.Mutate(x => x.Resize(400, 400));

                switch (Input.File.ContentType)
                {
                    case "image/png":
                        image.SaveAsPng(outStream);
                        break;
                    case "image/jpeg":
                        image.SaveAsJpeg(outStream, new JpegEncoder { Quality = 100});
                        break;
                    default:
                        break;
                }

                outStream.Position = 0; 
            }

            return File(outStream, Input.File.ContentType,"New Image");

            var result = _uploadService.UploadFile(Input.File.OpenReadStream());


            return RedirectToPage("Index");
        }
    }
}
