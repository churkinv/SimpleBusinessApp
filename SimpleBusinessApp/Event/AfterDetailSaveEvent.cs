using Prism.Events;

namespace SimpleBusinessApp.Event
{
    public class AfterDetailSaveEvent : PubSubEvent<AfterDetailSaveEventArgs>
    {

    }

    public class AfterDetailSaveEventArgs
    {
        public int Id { get; set; }
        public string DisplayMember { get; set; }
        public string ViewModelName { get; set; }
    }
}
