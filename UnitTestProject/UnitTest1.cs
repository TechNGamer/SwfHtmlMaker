using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using Utility.FS;

namespace UnitTestProject {
	[TestClass]
	public class UnitTest1 {
		[TestMethod]
		public void FSSpiderTester() {

			FSSpider.GetAllSubFiles( new DirectoryInfo( Environment.CurrentDirectory ).Root.FullName, new string[] { } );
		}
	}
}
