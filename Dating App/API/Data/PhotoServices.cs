using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApplication.Helper;
using DatingApplication.Interfaces;
using Microsoft.Extensions.Options;

namespace DatingApplication.Data
{
    public class PhotoServices : IPhotoServices
    {
        private readonly Cloudinary _cloudinary;
        public PhotoServices(IOptions<CloudinarySettings> config)
        {
            var acc = new Account(config.Value.CloudName, config.Value.APIKey, config.Value.APISecret);
            _cloudinary = new Cloudinary(acc);
        }
        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            var uploadImg = new ImageUploadResult();

            if(file.Length >0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParma = new ImageUploadParams()
                    {
                        File = new FileDescription(file.FileName, stream),
                        Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face"),
                        Folder = "da-net6"
                    };

                    uploadImg = await _cloudinary.UploadAsync(uploadParma);
                }
            }
            return uploadImg;
        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deleteParam = new DeletionParams(publicId);

            return await _cloudinary.DestroyAsync(deleteParam);
        }
    }
}
