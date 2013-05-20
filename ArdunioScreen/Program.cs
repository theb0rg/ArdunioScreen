using System;
using System.Text;
using System.Threading;

namespace ArdunioScreen
{
	class MainClass
	{
		static int NumberOfCharacters = 32;// 16 + 16
		static int FrameTime = 110;
		static String ComPort = "COM20";

		public static void Main (string[] args)
		{
			while (true) {
				/*try {   
				Console.WriteLine ("Insert number of characters(default 32), empty to use default:");
				String _numberOfCharacters = Console.ReadLine ();
				if (_numberOfCharacters != String.Empty) {
					NumberOfCharacters = int.Parse (_numberOfCharacters);
				}*/
				try {   
					Console.WriteLine ("Insert frametime in ms(default 110), empty to use default:");
					String _FrameTime = Console.ReadLine ();
					if (_FrameTime != String.Empty) {
						FrameTime = int.Parse (_FrameTime);
					}

					Console.WriteLine ("Message to send here: ");
					String MessageToSend = Console.ReadLine ();

					//Trim string if it is too long
					if (MessageToSend.Length > NumberOfCharacters) {
						MessageToSend.Remove (NumberOfCharacters - 1);
					}
				  
					using (SerialPortDisposable mySerialPort = new SerialPortDisposable(ComPort)) {
						lock (mySerialPort) {
							mySerialPort.OpenPort ();

							StringBuilder sb = new StringBuilder ();

							while (true) {
								//Start sequence
								for (int h = 0; h < MessageToSend.Length; h++) {
									sb.Clear ();
									sb.Append (MessageToSend.Substring(MessageToSend.Length-h)).Append (' ', NumberOfCharacters - h);
									mySerialPort.SendFunc (sb.ToString());
									WriteScreen (sb.ToString(), mySerialPort.ReadFunc (), 2, NumberOfCharacters / 2);
									Thread.Sleep (FrameTime);
								}

								//Main sequence
								for (int i = 0; i < NumberOfCharacters-MessageToSend.Length+1; i++) {
									sb.Clear ();
									sb.Append (' ', i).Append (MessageToSend).Append (' ', NumberOfCharacters - MessageToSend.Length - i);
									mySerialPort.SendFunc (sb.ToString());
									WriteScreen (sb.ToString(), mySerialPort.ReadFunc (), 2, NumberOfCharacters / 2);
									Thread.Sleep (FrameTime);
								}

								//End sequence
								for (int k = 1; k <= MessageToSend.Length; k++) {
									sb.Clear ();
									String Word2 = MessageToSend.Remove (MessageToSend.Length - k);
									sb.Append (' ', NumberOfCharacters - Word2.Length).Append (Word2);
									mySerialPort.SendFunc (sb.ToString());
									WriteScreen (sb.ToString(), mySerialPort.ReadFunc (), 2, NumberOfCharacters / 2);
									Thread.Sleep (FrameTime);
								}
							}

							mySerialPort.Close ();
						}
					}
				} catch (Exception ex) {
					Console.WriteLine (ex.ToString());
				} finally {

				}
			}
		}
		//Writescreen assumes that message is lines*characters long.
		public static void WriteScreen (String SimulatedMessage, String RecievedMessage, int lines, int characters)
		{
			Console.Clear ();
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine ("Sent Message:");
			Console.WriteLine ("----------------");
			for (int i = 0; i < lines; i++) {
				Console.WriteLine (SimulatedMessage.Substring(characters*i,characters));
			}
			Console.WriteLine ("----------------");
			Console.WriteLine ();
			Console.WriteLine ("Recieved Message:");
			Console.WriteLine ("----------------");
			if (RecievedMessage.Length < NumberOfCharacters) {
				Console.WriteLine ("Missmatch in recieved message. Got length '" + RecievedMessage.Length + "' but expected '" + NumberOfCharacters + "' characters");
				Console.WriteLine (RecievedMessage);
			} else
				for (int i = 0; i < lines; i++) {   
					Console.WriteLine (RecievedMessage.Substring(characters*i,characters));
				}
			Console.WriteLine ("----------------");
			Console.WriteLine (String.Format("Size: {0}x{1} on COMPORT: {2}",characters,lines, ComPort));
		}
	}
}
