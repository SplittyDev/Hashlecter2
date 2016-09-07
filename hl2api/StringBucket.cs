using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace hl2api {
    public class StringBucket : IEnumerable<string>, IDisposable {

        /// <summary>
        /// The reader.
        /// </summary>
        readonly StreamReader reader;

        /// <summary>
        /// Whether the stream has reached its end.
        /// </summary>
        /// <value><c>true</c> if the stream has reached its end.</value>
        public bool EndOfStream => reader.EndOfStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:hl2api.Cache"/> class.
        /// </summary>
        /// <param name="data_stream">Data stream.</param>
        protected StringBucket (Stream data_stream) {
            reader = new StreamReader (data_stream);
        }

        /// <summary>
        /// Creates a new bucket from the specified file.
        /// </summary>
        /// <returns>The file.</returns>
        /// <param name="path">Path.</param>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static StringBucket FromFile (string path) {
            return new StringBucket (File.OpenRead (path));
        }

        /// <summary>
        /// Gets all items in the cache.
        /// </summary>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        IEnumerator<string> All () {
            while (true) {
                var line = reader.ReadLine ();
                if (line == null)
                    yield break;
                yield return line;
            }
        }

        /// <summary>
        /// Gets the next item in the cache.
        /// </summary>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public string Next () {
            return reader.ReadLine ();
        }

        /// <summary>
        /// Gets the next <paramref name="n"/> items in the cache.
        /// </summary>
        /// <param name="n">N.</param>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public Tuple<int, string[]> Next (int n) {
            var buf = new string[n];
            var i = 0;
            for (; i < n; i++) {
                var str = reader.ReadLine ();
                if (str == null)
                    break;
                buf[i] = str;
            }
            return new Tuple<int, string[]> (i, buf);
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>The enumerator.</returns>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public IEnumerator<string> GetEnumerator () {
            return All ();
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>The enumerator.</returns>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        IEnumerator IEnumerable.GetEnumerator () {
            return All ();
        }

        /// <summary>
        /// Releases all resource used by the <see cref="T:hl2api.Cache"/> object.
        /// </summary>
        /// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="T:hl2api.Cache"/>. The
        /// <see cref="Dispose"/> method leaves the <see cref="T:hl2api.Cache"/> in an unusable state. After calling
        /// <see cref="Dispose"/>, you must release all references to the <see cref="T:hl2api.Cache"/> so the garbage
        /// collector can reclaim the memory that the <see cref="T:hl2api.Cache"/> was occupying.</remarks>
        public void Dispose () {
            reader.Dispose ();
        }
    }
}

