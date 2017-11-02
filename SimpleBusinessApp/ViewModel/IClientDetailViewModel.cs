using System.Threading.Tasks;

namespace SimpleBusinessApp.ViewModel
{
    public interface IClientDetailViewModel
    {
        Task LoadAsync(int? clientId);
        bool HasChanges { get; }
    }
}