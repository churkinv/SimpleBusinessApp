namespace SimpleBusinessApp.Model
{
    /// <summary>
    /// Сlass to lookup members by Id in DB, and display name of the member.
    /// </summary>
    public class LookupItem
    {
        public int Id { get; set; }

        public string DisplayMember { get; set; }

    }
}
