using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace HomeBrain
{
    public static class EXT
    {
        public static string F(this string fmt, params object[] args)
        {
            return string.Format(fmt, args);
        }

        public static byte[] Compress(this byte[] data)
        {
            using (var output = new MemoryStream())
            {
                using (var dstream = new DeflateStream(output, CompressionMode.Compress))
                    dstream.Write(data, 0, data.Length);
                return output.ToArray();
            }
        }

        public static byte[] Decompress(this byte[] data)
        {
            using (var input = new MemoryStream(data))
            using (var output = new MemoryStream())
            {
                using (DeflateStream dstream = new DeflateStream(input, CompressionMode.Decompress))
                    dstream.CopyTo(output);
                return output.ToArray();
            }
        }
        public static byte[] GetBytes(this string text)
        {
            return Encoding.UTF8.GetBytes(text);
        }
        public static string GetString(this byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }
        public static byte[] ToBin(this object obj)
        {
            var bs = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            using (var stream = new System.IO.MemoryStream())
            {
                bs.Serialize(stream, obj);
                return stream.ToArray();
            }
        }
        public static T FromBin<T>(this byte[] bytes)
        {
            return (T)bytes.FromBin();
        }
        public static object FromBin(this byte[] bytes)
        {
            var bs = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            using (var stream = new System.IO.MemoryStream(bytes))
                return bs.Deserialize(stream);
        }
    }
}
