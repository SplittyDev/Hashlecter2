using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using api = hl2api;

namespace hl2test {

    [TestFixture]
    public class StringBucket {

        readonly Stopwatch watch = new Stopwatch ();

        const string FILENAME = @"dict/rockyou.txt";
        const int LINES = 14344391;
        const int COUNT = 4096;

        void Report () {
            watch.Stop ();
            Console.WriteLine ($"{new StackFrame (1).GetMethod ().Name}: {watch.ElapsedMilliseconds}ms");
            watch.Reset ();
        }

        [Test]
        public void Enumerator () {
            if (!File.Exists (FILENAME)) {
                Assert.Ignore ();
                return;
            }
            var lines = 0;
            var cache = api.StringBucket.FromFile (FILENAME);
            watch.Start ();
            foreach (var str in cache) {
                ++lines;
            }
            Report ();
            Assert.AreEqual (LINES, lines);
        }

        [Test]
        public void EnumeratorParallel () {
            if (!File.Exists (FILENAME)) {
                Assert.Ignore ();
                return;
            }
            var lines = 0;
            var cache = api.StringBucket.FromFile (FILENAME);
            watch.Start ();
            Parallel.ForEach (cache, _ => {
                Interlocked.Increment (ref lines);
            });
            Report ();
            Assert.AreEqual (LINES, lines);
        }

        [Test]
        public void Next_A () {
            if (!File.Exists (FILENAME)) {
                Assert.Ignore ();
                return;
            }
            var lines = 0;
            var cache = api.StringBucket.FromFile (FILENAME);
            string str;
            watch.Start ();
            while ((str = cache.Next ()) != null) {
                ++lines;
            }
            Report ();
            Assert.AreEqual (LINES, lines);
        }

        [Test]
        public void Next_B () {
            if (!File.Exists (FILENAME)) {
                Assert.Ignore ();
                return;
            }
            var lines = 0;
            var cache = api.StringBucket.FromFile (FILENAME);
            Tuple<int, string[]> data;
            watch.Start ();
            while ((data = cache.Next (COUNT)).Item1 > 0) {
                lines += data.Item1;
            }
            Report ();
            Assert.AreEqual (LINES, lines);
        }
    }
}

