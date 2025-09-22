namespace SignalRClientServerSelfMVC.Models
{
    public class ReceivedOpcData
    {
        public ReceivedOpcData(string tagName, int tagValue)
        {
            TagName = tagName;
            TagValue = tagValue;
        }

        public string TagName { get; set; }
        public int TagValue { get; set; }
    }
}
