using MPConstruction.Models;
using Refit;
using System.Threading.Tasks;

namespace MPConstruction.Apis
{
    interface IReportApi
    {
        [Post("/api/reports")]
        Task<ImageResponse> SendReport([Body] Report report);
    }
}
