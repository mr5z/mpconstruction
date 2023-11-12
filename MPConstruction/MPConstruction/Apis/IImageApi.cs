using MPConstruction.Models;
using Refit;
using System.Threading.Tasks;

namespace MPConstruction.Apis
{
    interface IImageApi
    {
        [Post("/api/images")]
        Task<ImageResponse> UploadImage([Body] string base64);
    }
}
