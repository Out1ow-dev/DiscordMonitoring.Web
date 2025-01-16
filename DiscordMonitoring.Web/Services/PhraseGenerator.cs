namespace DiscordMonitoring.Web.Services
{
    public class PhraseGenerator
    {
        private static readonly string[] phrases = { "Phrase1", "Phrase2", "Phrase3" };

        public static string GenerateRandomPhrase()
        {
            return phrases[new Random().Next(0, phrases.Length)];
        }
    }
}
