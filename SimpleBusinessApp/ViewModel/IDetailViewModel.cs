using System.Threading.Tasks;

namespace SimpleBusinessApp.ViewModel
{
    public interface IDetailViewModel
    {
        Task LoadAsync(int? Id);
        bool HasChanges { get; }
        int Id { get; }
    }
}