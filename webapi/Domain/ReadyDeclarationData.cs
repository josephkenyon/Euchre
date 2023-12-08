using static webapi.Domain.Statics.Enums;

namespace webapi.Domain
{
    public class ReadyDeclarationData
    {
        public bool Ready { get; set; }
        public string GameName { get; set; }
        public string PlayerName { get; set; }

        public ReadyDeclarationData(string gameName, string playerName, bool ready) {
            Ready = ready;
            GameName = gameName;
            PlayerName = playerName;
        }
    }
}
