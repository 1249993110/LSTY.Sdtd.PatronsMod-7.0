using LSTY.Sdtd.PatronsMod.Extensions;
using LSTY.Sdtd.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSTY.Sdtd.PatronsMod.Commands
{
    public class GlobalMessage : ConsoleCmdBase
    {
		public override string[] GetCommands()
		{
			return new string[]
			{
				"ty-GlobalMessage",
				"ty-gm",
				"ty-say"
			};
		}

		public override string GetDescription()
		{
			return "Usage:\n" +
			   "  1. ty-gm <Message>\n" +
			   "  2. ty-gm <Message> <SenderName>\n" +
			   "1. Sends a message to all connected clients by default server name: " + Shared.Constants.DefaultServerName + "\n" +
			   "2. Sends a message to all connected clients by sender name";
		}

		public override void Execute(List<string> args, CommandSenderInfo _senderInfo)
		{
			if (args.Count < 1)
			{
				Log("Wrong number of arguments, expected 1, found " + args.Count + ".");
				return;
			}

			string message = args[0];
			string senderName = (args.Count < 2 || string.IsNullOrEmpty(args[1])) ? Shared.Constants.DefaultServerName : args[1];

			GameManager.Instance.ChatMessageServer(ClientInfoExtension.GetCmdExecuteDelegate(), EChatType.Global, -1, message, senderName, false, null);
		}
	}
}
