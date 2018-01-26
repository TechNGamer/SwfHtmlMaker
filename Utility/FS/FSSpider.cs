using System;
using System.Collections.Generic;
using System.IO;
#if DEBUG
using System.Diagnostics;
#endif

namespace Utility.FS {
	public static class FSSpider {
		public static List<string> GetAllSubFiles( string path, string[] ignoreList, bool verbose = false, string extLookFor = "*" ) {
			List<string> files = new List<string>();
			foreach(FileSystemInfo fsObject in new DirectoryInfo( path ).EnumerateFileSystemInfos() ) {
				bool skip = false;

				foreach(string str in ignoreList ) {
					if ( fsObject.Name.Contains( str ) ) {
						if ( verbose ) {
							Console.WriteLine( $"'{fsObject.FullName}' contains '{str}' which is on the ignore list." );
							#if DEBUG
							Debug.WriteLine( $"'{fsObject.FullName}' contains '{str}' which is on the ignore list." );
							#endif
						}
						skip = true;
						break;
					}
				}

				if ( !skip ) {
					if ( fsObject.Attributes.HasFlag( FileAttributes.Directory ) ) {
						if ( verbose ) {
							Console.WriteLine( $"Found directory {fsObject.FullName}." );
							#if DEBUG
							Debug.WriteLine( $"Found directory {fsObject.FullName}." );
							#endif
						}
						try {
							if ( verbose ) {
								Console.WriteLine( $"Looking into sub directory '{fsObject.FullName}'." );
								#if DEBUG
								Debug.WriteLine( $"Looking into sub directory '{fsObject.FullName}'." );
								#endif
							}
							files.AddRange( GetAllSubFiles( fsObject.FullName, ignoreList, verbose, extLookFor ) );
						} catch (Exception e) {
							if ( verbose ) {
								Console.WriteLine( $"An error occuered: {e.Message}" );
								#if DEBUG
								Debug.WriteLine( $"An error occuered: {e.Message}" );
								#endif
							}
						}
					} else {
						if ( verbose ) {
							Console.WriteLine( $"Found file '{fsObject.FullName}'" );
							#if DEBUG
							Debug.WriteLine( $"Found file '{fsObject.FullName}'" );
							#endif
						}
						if ( extLookFor == "*" || fsObject.Extension.Equals( extLookFor ) ) {
							if ( verbose ) {
								Console.WriteLine( $"Adding file '{fsObject.FullName}' to the list." );
								#if DEBUG
								Debug.WriteLine( $"Adding file '{fsObject.FullName}' to the list." );
								#endif
							}
							files.Add( fsObject.FullName );
						}
					}
				}
			}

			return files;
		}
	}
}
