namespace DiscordMonitoring.Web.Model
{
    public class GameEntity
    {
        public int Id { get; set; }

        public string GameName { get; set; }

        public ulong GameSteamId { get; set; }

        public string ImageUrl { get; set; }

        public string IconUrl { get; set; }

        public int ServerCount { get; set; }

        public int GeneralOnline { get; set; }
    }
}
