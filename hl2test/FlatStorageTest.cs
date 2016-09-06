using NUnit.Framework;
using System;
using System.IO;
using hl2api;

namespace hl2test {
    
    [TestFixture]
    public class FlatStorageTest {
        
        [Test]
        public void TestGenerator () {
            const string FILENAME = "dict/cain.txt";
            if (!File.Exists (FILENAME)) {
                Assert.Ignore ();
                return;
            }
            var lines = 0;
            var cache = FlatStorage.FromFile (FILENAME, 1024);
            foreach (var str in cache) {
                ++lines;
            }
            Assert.AreEqual (306706, lines);
        }
    }
}

