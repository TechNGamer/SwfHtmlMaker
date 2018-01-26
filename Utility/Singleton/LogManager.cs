using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Utility.Loggers;

#if DEBUG

#endif

namespace Utility.Singleton {

	internal class LogManager {
		//Allows for the grabbing of the singleton.
		public static LogManager Instence {
			get {
				if ( INSTENCE == null ) {
					INSTENCE = new LogManager();
				}

				return INSTENCE;
			}
		}
		//Allows external classes to grab where the log's are located on the disk.
		public static string LogHome {
			get {
				return logHome;
			}
		}
		//Static varibles that hold everything the log manager is going to need.
		private static LogManager INSTENCE;
		private static string logHome = Path.Combine( new string[] {
			Environment.CurrentDirectory,
			"TNG_.NET_Core_Logs"
		} );
		private static string normalLog = Path.Combine( logHome, "Normal.log" );
		private static string lowLevelErrorLog = Path.Combine( logHome, "Low_Level_Error.log" );
		private static string criticalLog = Path.Combine( logHome, "Critical.log" );
		//Queue's for the different log files.
		private Queue<string> normalBuffer = new Queue<string>(),
			lowLevelBuffer = new Queue<string>(),
			criticalBuffer = new Queue<string>();
		private Thread loggingWorker;
		private ManualResetEvent waiting = new ManualResetEvent( false ),
			terminate = new ManualResetEvent( false ),
			hasNewNormalItem = new ManualResetEvent( false ),
			hasNewLowLevelItem = new ManualResetEvent( false ),
			hasNewCriticalItem = new ManualResetEvent( false );
		private FileStream normalLogFile, lowLevelLogFile, criticalLogFile;

		private LogManager() {
			//Checks to see if the loggingWorker is already running.
			if ( loggingWorker == null ) {
				//Creates a new loggingWorker if not.
				loggingWorker = new Thread( new ThreadStart( ProcessQueue ) );
			}
			//Checks to see if the log dir already exists.
			if ( !Directory.Exists( LogHome ) ) {
				Directory.CreateDirectory( LogHome );
			}
			//Opens or Creates the logs for writing to.
			normalLogFile = File.Open( normalLog, FileMode.Create );
			lowLevelLogFile = File.Open( lowLevelErrorLog, FileMode.Create );
			criticalLogFile = File.Open( criticalLog, FileMode.Create );
			//Starts the logging thread, assigns it as a background thread, and gives it the lowest thread priority.
			loggingWorker.Start();
			loggingWorker.IsBackground = true;
			loggingWorker.Priority = ThreadPriority.Lowest;
		}
		//Method to be used in processing the queue's.

		#region Thread methods

		private void ProcessQueue() {
			while ( true ) {
				//Creates a varable called i and set's it to 0.
				int i = 0;
				//Checks to see if any of the flags are set, and if none of them are, set the waiting flag and wait until one of the flags become active. Otherwise, continue to the writing part.
				i = WaitHandle.WaitAny( new WaitHandle[] { hasNewNormalItem, hasNewLowLevelItem, hasNewCriticalItem, terminate } );
				//Looks to see what value i is and to write to the corrosponding log.
				switch ( i ) {
					case 0:
					case 1:
					case 2:
						Flush();
						hasNewNormalItem.Reset();
						hasNewLowLevelItem.Reset();
						hasNewCriticalItem.Reset();
						break;
					case 3:
						Flush();
						return;
				}
			}
		}
		//Writes to the critical log.
		private void WriteToCriticalLog() {
			Queue<string> queueCopy;

			lock ( criticalBuffer ) {
				if ( criticalBuffer.Count > 0 ) {
					queueCopy = new Queue<string>( criticalBuffer );
					criticalBuffer.Clear();
				} else {
					return;
				}
			}

			foreach ( string logMessage in queueCopy ) {
				byte[] info = new UTF8Encoding( true ).GetBytes( $"{logMessage}".ToCharArray() );

				criticalLogFile.Write( info, 0, info.Length );
				criticalLogFile.Flush();
			}
		}
		//Writes to the Low Level Error Log.
		private void WriteToLowLevelLog() {
			Queue<string> queueCopy;

			lock ( lowLevelBuffer ) {
				if ( lowLevelBuffer.Count > 0 ) {
					queueCopy = new Queue<string>( lowLevelBuffer );
					lowLevelBuffer.Clear();
				} else {
					return;
				}
			}

			foreach ( string logMessage in queueCopy ) {
				byte[] info = new UTF8Encoding( true ).GetBytes( $"{logMessage}".ToCharArray() );

				lowLevelLogFile.Write( info, 0, info.Length );
				lowLevelLogFile.Flush();
			}
		}
		//Writes to the normal log.
		private void WriteToNormalLog() {
			Queue<string> queueCopy;

			lock ( normalBuffer ) {
				if ( normalBuffer.Count > 0 ) {
					queueCopy = new Queue<string>( normalBuffer );
					normalBuffer.Clear();
				} else {
					return;
				}
			}

			foreach ( string logMessage in queueCopy ) {
				byte[] info = new UTF8Encoding( true ).GetBytes( $"{logMessage}".ToCharArray() );

				normalLogFile.Write( info, 0, info.Length );
				normalLogFile.Flush();
			}
		}
		//Makes sure all logs have been flushed to the file.
		private void Flush() {
			WriteToNormalLog();
			WriteToLowLevelLog();
			WriteToCriticalLog();
		}

		#endregion

		//Puts the message in the corrosponding log.
		public void WriteToLog( string message, LogLevel errorLevel ) {
			//Message is reasigned to a new string that has the time and message.
			message = $"{DateTime.Now.ToLocalTime().ToString()}: {message}";
			#if DEBUG
			string errorLevelName = Enum.GetName( typeof( LogLevel ), errorLevel );
			Debug.WriteLine( $"Adding message; '{message}' to {errorLevel} queue to be written to a {errorLevelName}.log." );
			#endif
			//Checks to see what error level it is and to enqueue it into that file.
			if ( errorLevel == LogLevel.LowLevelError ) {
				//Locks that queue from being used.
				lock ( lowLevelBuffer ) {
					lowLevelBuffer.Enqueue( message );
				}
				//Raises the flag that the message has been placed in the queue.
				hasNewLowLevelItem.Set();

			} else if ( errorLevel == LogLevel.Normal ) {

				lock ( normalBuffer ) {
					normalBuffer.Enqueue( message );
				}

				hasNewNormalItem.Set();

			} else if ( errorLevel == LogLevel.Critical ) {

				lock ( criticalBuffer ) {
					criticalBuffer.Enqueue( message );
				}

				hasNewCriticalItem.Set();
			}
		}

		public void FlushQueues() {
			terminate.Set();
		}
	}
}