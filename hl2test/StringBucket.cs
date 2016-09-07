using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using api = hl2api;

namespace hl2test {

    [TestFixture]
    public class StringBucket {

        [Test]
        public void Enumerator () {
            const string FILENAME = @"dict/cain.txt";
            if (!File.Exists (FILENAME)) {
                Assert.Ignore ();
                return;
            }
            var lines = 0;
            var cache = api.StringBucket.FromFile (FILENAME);
            foreach (var str in cache) {
                ++lines;
            }
            Assert.AreEqual (306706, lines);
        }

        [Test]
        public void EnumeratorParallel () {
            const string FILENAME = @"dict/cain.txt";
            if (!File.Exists (FILENAME)) {
                Assert.Ignore ();
                return;
            }
            var lines = 0;
            var cache = api.StringBucket.FromFile (FILENAME);
            Parallel.ForEach (cache, _ => {
                Interlocked.Increment (ref lines);
            });
            Assert.AreEqual (306706, lines);
        }

        [Test]
        public void Next_A () {
            const string FILENAME = @"dict/cain.txt";
            if (!File.Exists (FILENAME)) {
                Assert.Ignore ();
                return;
            }
            var lines = 0;
            var cache = api.StringBucket.FromFile (FILENAME);
            while (!cache.EndOfStream) {
                cache.Next ();
                ++lines;
            }
            Assert.AreEqual (306706, lines);
        }

        [Test]
        public void Next_B () {
            const string FILENAME = @"dict/cain.txt";
            if (!File.Exists (FILENAME)) {
                Assert.Ignore ();
                return;
            }
            var lines = 0;
            var cache = api.StringBucket.FromFile (FILENAME);
            while (!cache.EndOfStream) {
                lines += cache.Next (128).Item1;
            }
            Assert.AreEqual (306706, lines);
        }

        [Test]
        public async void NextAsync_A () {
            const string FILENAME = @"dict/cain.txt";
            if (!File.Exists (FILENAME)) {
                Assert.Ignore ();
                return;
            }
            var lines = 0;
            var cache = api.StringBucket.FromFile (FILENAME);
            while (!cache.EndOfStream) {
                await cache.NextAsync ();
                ++lines;
            }
            Assert.AreEqual (306706, lines);
        }

        [Test]
        public async void NextAsync_B () {
            const string FILENAME = @"dict/cain.txt";
            if (!File.Exists (FILENAME)) {
                Assert.Ignore ();
                return;
            }
            var lines = 0;
            var cache = api.StringBucket.FromFile (FILENAME);
            while (!cache.EndOfStream) {
                lines += (await cache.NextAsync (128)).Item1;
            }
            Assert.AreEqual (306706, lines);
        }
    }
}

