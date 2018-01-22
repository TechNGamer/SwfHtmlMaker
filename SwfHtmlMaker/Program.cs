using System;
using System.Collections.Generic;
using System.IO;
using Utility.Loggers;

namespace SwfHtmlMaker {
	static class Program {

		static bool recursive, verbose; // Verbose prints all relevent information on what it's doing out. Recursive looks into sub directories.
		static FileInfo masterHTML; // masterHTML is the file that is at the psudo root.
		static List<string> ignoreNames; // A list for the FSSpider to ignore. See FSSpider in Utilities to see what it does.

		static void Main( string[] args ) {
			// Checks to see if there are any args.
			if( args == null || args.Length == 0 ) {
				ThrowHelp( true );
			}
			// Checks for valid arguments.
			for( int i = 0; i < args.Length - 1; i++ ) {
				if ( args[i].StartsWith( "--" ) ) { // Otherwise check to see if it's a string argument.
					// Sanatizes the argument string before comparing.
					string argument = args[ i ].Substring( 2 );
					// Checks to see which argument to use.
					switch ( argument ) {
						// master-file is the HTML file that will be created at the psudo root level.
						case "master-file":
							masterHTML = new FileInfo( args[ ++i ] );
							recursive = true;
							break;
						// Do I need to explain what verbose is?
						case "verbose":
							verbose = true;
							break;
						// Uses a FileSystemSpider to look through all the sub directories for other files.
						case "recursive":
							recursive = true;
							break;
						// Creates a list of names to ignore.
						// Note: This argument will tell the FSSpider to not go into that directory if it appears in the directory name.
						case "ignore":
							i = CreateIgnoreList( args, ++i );
							break;
						// Do I need to explain what this is?
						case "help":
							ThrowHelp( );
							break;
						default:
							ThrowHelp( true );
							break;
					}
				} else if ( args[ i ].StartsWith( "-" ) ) { // Checks to see if it's a char argument.
					char[] charArgs = args[ i ].Substring( 1 ).ToCharArray(); // This is to get the individual charectors for analizing.

					foreach ( char @char in charArgs ) {
						switch ( @char ) {
							case 'h':
								ThrowHelp();
								break;
							case 'r':
							case 'R':
								recursive = true;
								break;
							case 'v':
								verbose = true;
								break;
							default:
								ThrowHelp( true );
								break;
						}
					}
				}
			}

			if ( recursive ) {
				masterHTML = new FileInfo( Path.Combine( args[ args.Length - 1 ], "Master.html" ) );
			} else {
				masterHTML = new FileInfo( args[ args.Length ] );
			}

			BeginCreation();
		}
		/*
		 * <summary>
		 * This method starts the whole sha-bang off.
		 */
		static void BeginCreation() {
			Logger classLogger = new Logger( "Program.cs" );

			classLogger.WriteToLog( "Everything is normal. No errors yet." );

			Console.Read();
		}
		/*
		 * <summary>
		 * This method creates the ignoreList for the ignore argument.
		 * </summary>
		 */
		static int CreateIgnoreList(string[] args, int startIndex ) {
			// Creates a new ignore list.
			ignoreNames = new List<string>();
			// Checks to see if the starting arg is a curly bracket.
			if(args[startIndex] == "{" ) {
				// Starts a loop where i is startIndex + 1.
				for ( int i = startIndex + 1; i < args.Length; i++ ) {
					ignoreNames.Add( args[ i ] );

					// Checks to see the next one isn't a curly bracket.
					if ( args[ i + 1 ] == "}" ) {
						return i;
					}
				}
			}

			return startIndex;
		}
		/*
		 * <summary>
		 * This method opens a text file located in the Resources folder and outputs it to the console.
		 * </summary>
		 */
		static void ThrowHelp(bool wrongOrInvalidArg = false) {
			Console.WriteLine( File.ReadAllText( Path.Combine( Environment.CurrentDirectory, "Resources", "help.txt" ) ) ); // Opens the help.txt file for reading, outputs it, then closes the file.

			if ( wrongOrInvalidArg ) {
				Environment.ExitCode = -1; // -1 indicates that the program has exited, but an argument was wrong.
			} else {
				Environment.ExitCode = 0; // 0 indicates that the program has exited fine.
			}

			Console.WriteLine( $"\nExit code: {Environment.ExitCode}\n\nPlease go to the GitHub page to learn what each exit code means." );

			Environment.Exit( Environment.ExitCode );
		}
	}
}
