using CloudinaryDotNet.Actions;

namespace DatingApplication.Interfaces
{
    public interface IPhotoServices
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);

        Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}
