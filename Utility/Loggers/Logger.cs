using System;

#if DEBUG
using System.Diagnostics;
#endif

using Utility.Singleton;

namespace Utility.Loggers {
	public enum LogLevel { LowLevelError, Normal, Critical }

	public class Logger {

		public string ScriptName { get; }

		public string LogLocation {
			get => LogManager.LogHome;
		}

		public bool Verbose {
			get;
			set;
		}

		private LogManager logManager;

		public Logger( string scriptName, bool verbose = false ) {
			ScriptName = scriptName;
			Verbose = verbose;

			logManager = LogManager.Instence;
		}

		public void Flush() {
			logManager.FlushQueues();
		}

		#region WriteToLog Methods

		public void WriteToLog( string message, LogLevel errorLevel = LogLevel.Normal ) {
			logManager.WriteToLog( $"{ScriptName}: {message}", errorLevel );

			if ( Verbose ) {
				Console.Write( message );

#if DEBUG
				Debug.Write( message );
#endif
			}
		}

		public void WriteToLog( char message, LogLevel errorLevel = LogLevel.Normal ) {
			WriteToLog( $"{message}", errorLevel );
		}

		public void WriteToLog( bool message, LogLevel errorLevel = LogLevel.Normal ) {
			WriteToLog( $"{message}", errorLevel );
		}

		public void WriteToLog( byte message, LogLevel errorLevel = LogLevel.Normal ) {
			WriteToLog( $"{message}", errorLevel );
		}

		public void WriteToLog( ushort message, LogLevel errorLevel = LogLevel.Normal ) {
			WriteToLog( $"{message}", errorLevel );
		}

		public void WriteToLog( uint message, LogLevel errorLevel = LogLevel.Normal ) {
			WriteToLog( $"{message}", errorLevel );
		}

		public void WriteToLog( ulong message, LogLevel errorLevel = LogLevel.Normal ) {
			WriteToLog( $"{message}", errorLevel );
		}

		public void WriteToLog( short message, LogLevel errorLevel = LogLevel.Normal ) {
			WriteToLog( $"{message}", errorLevel );
		}

		public void WriteToLog( int message, LogLevel errorLevel = LogLevel.Normal ) {
			WriteToLog( $"{message}", errorLevel );
		}

		public void WriteToLog( long message, LogLevel errorLevel = LogLevel.Normal ) {
			WriteToLog( $"{message}", errorLevel );
		}

		public void WriteToLog( float message, LogLevel errorLevel = LogLevel.Normal ) {
			WriteToLog( $"{message}", errorLevel );
		}

		public void WriteToLog( double message, LogLevel errorLevel = LogLevel.Normal ) {
			WriteToLog( $"{message}", errorLevel );
		}

		public void WriteToLog( decimal message, LogLevel errorLevel = LogLevel.Normal ) {
			WriteToLog( $"{message}", errorLevel );
		}

		public void WriteToLog( object message, LogLevel errorLevel = LogLevel.Normal ) {
			WriteToLog( message.ToString(), errorLevel );
		}

		public void WriteLineToLog( string message, LogLevel errorLevel = LogLevel.Normal ) {
			WriteToLog( $"{message}\n", errorLevel );
		}

		public void WriteLineToLog( char message, LogLevel errorLevel = LogLevel.Normal ) {
			WriteToLog( $"{message}\n", errorLevel );
		}

		public void WriteLineToLog( bool message, LogLevel errorLevel = LogLevel.Normal ) {
			WriteToLog( $"{message}\n", errorLevel );
		}

		public void WriteLineToLog( byte message, LogLevel errorLevel = LogLevel.Normal ) {
			WriteToLog( $"{message}\n", errorLevel );
		}

		public void WriteLineToLog( ushort message, LogLevel errorLevel = LogLevel.Normal ) {
			WriteToLog( $"{message}\n", errorLevel );
		}

		public void WriteLineToLog( uint message, LogLevel errorLevel = LogLevel.Normal ) {
			WriteToLog( $"{message}\n", errorLevel );
		}

		public void WriteLineToLog( ulong message, LogLevel errorLevel = LogLevel.Normal ) {
			WriteToLog( $"{message}\n", errorLevel );
		}

		public void WriteLineToLog( short message, LogLevel errorLevel = LogLevel.Normal ) {
			WriteToLog( $"{message}\n", errorLevel );
		}

		public void WriteLineToLog( int message, LogLevel errorLevel = LogLevel.Normal ) {
			WriteToLog( $"{message}\n", errorLevel );
		}

		public void WriteLineToLog( long message, LogLevel errorLevel = LogLevel.Normal ) {
			WriteToLog( $"{message}\n", errorLevel );
		}

		public void WriteLineToLog( float message, LogLevel errorLevel = LogLevel.Normal ) {
			WriteToLog( $"{message}\n", errorLevel );
		}

		public void WriteLineToLog( double message, LogLevel errorLevel = LogLevel.Normal ) {
			WriteToLog( $"{message}\n", errorLevel );
		}

		public void WriteLineToLog( decimal message, LogLevel errorLevel = LogLevel.Normal ) {
			WriteToLog( $"{message}\n", errorLevel );
		}

		public void WriteLineToLog( object message, LogLevel errorLevel = LogLevel.Normal ) {
			WriteToLog( $"{message}\n", errorLevel );
		}

		#endregion
	}
}
