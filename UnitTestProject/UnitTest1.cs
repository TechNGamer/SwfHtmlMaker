using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;
using Utility.FS;

namespace UnitTestProject {
	[TestClass]
	public class UnitTest1 {
		[TestMethod]
		public void FSSpiderTester() {

			FSSpider.GetAllSubs( new DirectoryInfo( Environment.CurrentDirectory ).Root.FullName, new string[] { } );
		}
	}
}
