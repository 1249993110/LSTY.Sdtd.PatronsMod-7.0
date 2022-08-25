namespace LSTY.Sdtd.PatronsMod.Commands
{
    public class SayToPlayer : ConsoleCmdBase
    {
        public override string GetDescription()
        {
            return "Send a message to a single player";
        }

        public override string GetHelp()
        {
            return "Usage:\n" +
                   "  1. ty-pm <EntityId/PlayerId/PlayerName> <Message>\n" +
                   "1. Send a PM to the player given by the entity id or player id or player name (as given by e.g. \"lpi\").";
        }

        public override string[] GetCommands()
        {
            return new[] { "ty-SayToPlayer", "ty-pm" };
        }

        private void SendMessage(ClientInfo receiver, ClientInfo sender, string message, string senderName)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            string senderId;

            if (sender == null)
            {
                senderId = Shared.Constants.NonPlayer;
            }
            else
            {
                senderId = sender.PlatformId.CombinedString;
                senderName = sender.playerName;
            }

            receiver.SendPackage(NetPackageManager.GetPackage<NetPackageChat>().Setup(EChatType.Whisper, -1, message, senderName, false, null));

            CustomLogger.Info("Message \"{0}\" to player {1} sent with sender {2}.", message, receiver.PlatformId.CombinedString, senderId);
        }

        private void InternalExecute(ClientInfo sender, List<string> args)
        {
            if (args.Count < 2)
            {
                Log("Usage: sayplayer <EOS/EntityId/PlayerName> <message>");
                return;
            }

            string message = args[1];

            ClientInfo receiver = ConsoleHelper.ParseParamIdOrName(args[0]);
            if (receiver == null)
            {
                Log("EOS or entityId or playerName not found.");
            }
            else
            {
                string senderName = (args.Count < 3 || string.IsNullOrEmpty(args[2])) ? Shared.Constants.DefaultServerName : args[2];
                SendMessage(receiver, sender, message, senderName);
            }
        }

        public override void Execute(List<string> args, CommandSenderInfo senderInfo)
        {
            // From game client.
            if (senderInfo.RemoteClientInfo != null)
            {
                InternalExecute(senderInfo.RemoteClientInfo, args);
            }
            // From console.
            else
            {
                InternalExecute(null, args);
            }
        }
    }
}