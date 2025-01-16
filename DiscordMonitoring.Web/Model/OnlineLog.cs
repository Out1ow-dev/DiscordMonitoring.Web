namespace DiscordMonitoring.Web.Model
{
    public class OnlineLog
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string OnlinePlayers { get; set; }

        public int? ServerId { get; set; }
        ServerEntity? serverEntity { get; set; }
    }
}
