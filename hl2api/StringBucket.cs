using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace hl2api {
    public class StringBucket : IEnumerable<string>, IDisposable {

        /// <summary>
        /// The max cached lines.
        /// </summary>
        int maxCachedLines;

        /// <summary>
        /// The reader.
        /// </summary>
        protected StreamReader reader;

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
        public static StringBucket FromFile (string path) {
            return new StringBucket (File.OpenRead (path));
        }

        /// <summary>
        /// Gets all items in the cache.
        /// </summary>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public IEnumerator<string> All () {
            while (!reader.EndOfStream) {
                yield return reader.ReadLine ();
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
        /// Gets the next item in the cache.
        /// </summary>
        public async Task<string> NextAsync () {
            return await reader.ReadLineAsync ();
        }

        /// <summary>
        /// Gets the next <paramref name="n"/> items in the cache.
        /// </summary>
        /// <param name="n">N.</param>
        public async Task<Tuple<int, string[]>> NextAsync (int n) {
            var buf = new string[n];
            var i = 0;
            for (; i < n; i++) {
                var str = await reader.ReadLineAsync ();
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
        public IEnumerator<string> GetEnumerator () {
            return All ();
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>The enumerator.</returns>
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

