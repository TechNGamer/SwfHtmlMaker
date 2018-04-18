using System;
using System.Globalization;
using System.Collections.Generic;
using System.IO;
using System.Threading;

using Utility.Loggers;
using Utility.FS;

namespace SwfHtmlMaker {
	static class Program {

		static bool recursive, verbose; // Verbose prints all relevent information on what it's doing out. Recursive looks into sub directories.
		static FileInfo masterHTML; // masterHTML is the file that is at the psudo root.
		static List<string> ignoreNames; // A list for the FSSpider to ignore. See FSSpider in Utilities to see what it does.
		static Logger classLogger;

		static void Main( string[] args ) {
			// Checks to see if there are any args.
			if ( args == null || args.Length == 0 ) {
				ThrowHelp( true );
			}
			// Checks for valid arguments.
			for ( int i = 0; i < args.Length - 1; i++ ) {
				if ( args[ i ].StartsWith( "--" ) ) { // Otherwise check to see if it's a string argument.
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
							ThrowHelp();
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

			classLogger = new Logger( "Program.cs", verbose );

			if (ignoreNames == null ) {
				classLogger.WriteLineToLog( "ignore option not used, creating empty ignore list." );

				ignoreNames = new List<string>();
			}

			classLogger.WriteLineToLog( $"Creating a master html file at '{args[ args.Length - 1 ]}'" );

			if ( recursive ) {
				masterHTML = new FileInfo( Path.Combine( args[ args.Length - 1 ], "Master.html" ) );
			} else {
				masterHTML = new FileInfo( args[ args.Length - 1 ] );
			}

			classLogger.WriteLineToLog( $"Checking variables:\n\tPath: {args[ args.Length - 1 ]}\n\tRecursive: {recursive}\n\tVerbose: {verbose}\n\tMasterHTML: {masterHTML.FullName}" );

			BeginCreation();
		}
		/*
		 * <summary>
		 * This method starts the whole sha-bang off.
		 * </summary>
		 */
		static void BeginCreation() {
			StreamWriter masterHTMLWriter;
			List<string> swfFiles;

			classLogger.WriteLineToLog( $"Getting all subfiles from '{masterHTML.DirectoryName}'" );
			
			swfFiles = FSSpider.GetAllSubFiles( masterHTML.Directory.FullName, ignoreNames.ToArray(), verbose, ".swf" );

			masterHTMLWriter = masterHTML.CreateText();
			masterHTMLWriter.AutoFlush = true;

			if ( !recursive ) {
				classLogger.WriteLineToLog( "Create single html file." );

				CreateMasterHTMLForOne( masterHTMLWriter, swfFiles[ 0 ] );
			} else {
				classLogger.WriteLineToLog( "Create multiple html files." );

				CreateMasterHTMLForAll( masterHTMLWriter, swfFiles.ToArray() );
			}

#if DEBUG
			Console.WriteLine( $"Done, please press any key to continue." );

			Console.Read();
#else
			Console.WriteLine( $"Exit code ({Environment.ExitCode})" );
#endif
			classLogger.Flush();

			Thread.Sleep( 250 );
		}
		// TODO: Create a Sanatize method.
		//static void SanatizeLocation() {

		//}

		static void CreateMasterHTMLForAll( StreamWriter mainHTML, string[] swfPaths ) {
			List<string> htmlPaths = new List<string>();

			classLogger.WriteLineToLog( $"Creating sub html files at different locations." );

			for ( int i = 0; i < swfPaths.Length; i++ ) {
				htmlPaths.Add( swfPaths[ i ].Replace( ".swf", ".html", true, CultureInfo.CurrentCulture ) );

				classLogger.WriteLineToLog( $"Adding '{htmlPaths[ i ]}' to the htmlPaths." );

				StreamWriter miniHTML = File.CreateText( htmlPaths[ i ] );

				classLogger.WriteLineToLog( $"Creating html file at '{htmlPaths[i]}'." );

				CreateMasterHTMLForOne( miniHTML, swfPaths[ i ] );

				miniHTML.Flush();
				miniHTML.Close();
				miniHTML.Dispose();
			}

			foreach ( string line in File.ReadAllLines( Path.Combine( Environment.CurrentDirectory, "Resources", "list_swf.html" ) ) ) {
				if ( line.Contains( "[parentFolder]" ) ) {
					string newLine = line.Replace( "[parentFolder]", masterHTML.DirectoryName );

					classLogger.WriteLineToLog( $"Found [parentFolder], replacing with '{masterHTML.DirectoryName}'." );

					mainHTML.WriteLine( newLine );
				} else if ( line.Trim().StartsWith( "<a " ) ) {
					foreach ( string htmlPath in htmlPaths ) {
						FileInfo htmlFile = new FileInfo( htmlPath );

						string newLine = line.Replace( "[path]",
							htmlPath,
							true,
							CultureInfo.CurrentCulture
						).Replace( "[fileName]",
							htmlFile.Name.Remove( htmlFile.Name.LastIndexOf( "." ) ),
							true,
							CultureInfo.CurrentCulture
						);

						classLogger.WriteLineToLog( $"Found [path] and [fileName]. Appending to html file." );

						mainHTML.WriteLine( $"{newLine}\n\t\t<br/>" );
					}
				} else {
					classLogger.WriteLineToLog( $"Writing '{line}' to htmlFile." );

					mainHTML.WriteLine( line );
				}
			}
		}

		static void CreateMasterHTMLForOne( StreamWriter writer, string swfPath ) {
			foreach ( string line in File.ReadAllLines( Path.Combine( Environment.CurrentDirectory, "Resources", "swf_template_single.html" ) ) ) {
				if ( line.Contains( "[parentFolder]" ) ) {
					string newLine = line.Replace( "[parentFolder]", masterHTML.Directory.Name );

					classLogger.WriteLineToLog( $"Found [parentFolder], replacing with '{masterHTML.DirectoryName}'." );

					writer.WriteLine( newLine );
				} else if ( line.Contains( "[file]" ) ) {
					string newLine = line.Replace( "[file]", swfPath );

					classLogger.WriteLineToLog( $"Found [file]. Appending to html file." );

					writer.WriteLine( newLine );
				} else {
					classLogger.WriteLineToLog( $"Writing '{line}' to htmlFile." );

					writer.WriteLine( line );
				}
			}
		}
		/*
		 * <summary>
		 * This method creates the ignoreList for the ignore argument.
		 * </summary>
		 */
		static int CreateIgnoreList( string[] args, int startIndex ) {
			// Creates a new ignore list.
			ignoreNames = new List<string>();
			// Checks to see if the starting arg is a curly bracket.
			if ( args[ startIndex ] == "{" ) {
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
		static void ThrowHelp( bool wrongOrInvalidArg = false ) {
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
