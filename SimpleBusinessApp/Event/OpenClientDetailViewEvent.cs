using Prism.Events;

namespace SimpleBusinessApp.Event
{

    /// <summary>
    /// We will pulish this event from NavigationViewModel
    /// </summary>
    class OpenClientDetailViewEvent : PubSubEvent<int?> // we inhereit from Prism PubSub... and point int as parametr, what will be our Client ID
    {
    }
}
