
using System.IO;
using System.Threading.Tasks;

namespace Antalaktiko.Services
{
    public interface IPhotoPickerService
    {
        Task<Stream> GetImageStreamAsync();
    }
}
