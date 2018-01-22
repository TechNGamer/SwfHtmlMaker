using System;
using System.Collections.Generic;
using System.Text;
using Utility.Singleton;

namespace Utility.Loggers {
	public enum LogLevel { LowLevelError, Normal, Critical }

	public class Logger {

		public string csScriptName {
			get {
				return _csScriptName;
			}
		}

		public string logLocation {
			get => LogManager.LogHome;
		}

		private string _csScriptName;

		private LogManager logManager;

		public Logger(string csScriptName ) {
			_csScriptName = csScriptName;

			logManager = LogManager.Instence;
		}

		#region WriteToLog Methods

		public void WriteToLog( string message, LogLevel errorLevel = LogLevel.Normal ) {
			logManager.WriteToLog( $"{csScriptName}: {message}", errorLevel );
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

		#endregion
	}
}
