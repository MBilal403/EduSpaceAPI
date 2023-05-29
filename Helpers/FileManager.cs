using EduSpaceAPI.Repository;
using Microsoft.AspNetCore.Hosting;

namespace EduSpaceAPI.Helpers
{
    public class FileManager
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FileManager(IWebHostEnvironment webHostEnvironment) { 
            _webHostEnvironment = webHostEnvironment;
           
        }

        public string GetFilePath(IFormFile file) {
            string? uploadsFolder = null;
            if (file == null || file.Length <= 0)
            {
                return "Invalid file.";
            }

            // Generate a unique filename
            // Get the uploads folder path
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            if (Path.GetExtension(file.FileName) == ".jpg" || Path.GetExtension(file.FileName) == ".png")
            {
             uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images");

            }
            else
            {
                 uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Resumes");
            }
            // Create the uploads folder if it doesn't exist
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }
            // Combine the uploads folder path with the file name
            string filePath = Path.Combine(uploadsFolder, fileName);
            // Save the file to the server
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            byte[] filebyte = System.IO.File.ReadAllBytes(filePath);
            System.IO.File.WriteAllBytes(filePath, filebyte);

            return filePath;

        }






    }
}
