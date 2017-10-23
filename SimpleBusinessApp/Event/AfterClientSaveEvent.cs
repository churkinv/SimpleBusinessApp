using Prism.Events;

namespace SimpleBusinessApp.Event
{
    public class AfterClientSaveEvent : PubSubEvent<AfterClientSaveEventArgs>
    {

    }

    public class AfterClientSaveEventArgs
    {
        public int Id { get; set; }
        public string DisplayMember { get; set; }
    }
}
