using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DiscordMonitoring.Web.Model
{
    public class ServerEntity
    {
        public int Id { get; set; }
        public string ServerName { get; set; }
        public string? ServerDescription { get; set; }
        public string? ServerOnline { get; set; }
        public string ServerVersion { get; set; }
        public string ServerMap { get; set; }
        public string Location { get; set; }
        public string ServerIP { get; set; }
        public string QueryPort { get; set; }
        public string GamePort { get; set; }
        public string CreateTime { get; set; }
        public bool? IsOnline { get; set; }
        public int? GameId { get; set; } 
        public GameEntity? Game { get; set; }
        public List<CategoryEntity>? Categories { get; set; }
        public List<OnlineLog>? OnlineLog { get; set; }
        public int? UserId { get; set; }
        public IdentityUser? User { get; set; }
    }
}
