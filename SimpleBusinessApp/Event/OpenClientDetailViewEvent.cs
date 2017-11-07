using Prism.Events;

namespace SimpleBusinessApp.Event
{

    /// <summary>
    /// We will pulish this event from NavigationViewModel
    /// </summary>
    class OpenDetailViewEvent : PubSubEvent<OpenDetailViewEventArgs> // we inhereit from Prism 
    {
    }

    public class OpenDetailViewEventArgs
    {
        public int? Id { get; set; }
        public string ViewModelName { get; set; }
    }
}
