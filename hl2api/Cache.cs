﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace hl2api {
    public class Cache : IDisposable {

        /// <summary>
        /// The max cached lines.
        /// </summary>
        int max_cached_lines;

        /// <summary>
        /// The reader.
        /// </summary>
        protected StreamReader reader;

        /// <summary>
        /// Gets the max cached lines.
        /// </summary>
        /// <value>The max cached lines.</value>
        public int MaxCachedLines => max_cached_lines;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:hl2api.Cache"/> class.
        /// </summary>
        /// <param name="data_stream">Data stream.</param>
        /// <param name="max_cached_lines">Max cached lines.</param>
        protected Cache (Stream data_stream, int max_cached_lines) {
            this.max_cached_lines = max_cached_lines;
            reader = new StreamReader (data_stream);
        }

        /// <summary>
        /// Gets all items in the cache.
        /// </summary>
        public IEnumerator<string> All () {
            yield return reader.ReadLine ();
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
        public string[] Next (int n) {
            var buf = new string[n];
            for (var i = 0; i < n; i++) {
                buf[i] = reader.ReadLine ();
            }
            return buf;
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
        public async Task<string[]> NextAsync (int n) {
            var buf = new string[n];
            for (var i = 0; i < n; i++) {
                buf[i] = await reader.ReadLineAsync ();
            }
            return buf;
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

