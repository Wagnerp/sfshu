using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace sfshu
{
  internal class HashProcessor
  {
    public HashProcessor(HashAlgorithm hash, string path)
    {
      Algorithm = hash;
      Path = path;
    }

    public HashAlgorithm Algorithm { get; private set; }
    public string Path { get; private set; }

    public void Run()
    {
      foreach (var file in Directory.EnumerateFiles(Path, "*", SearchOption.AllDirectories))
      {
        try
        {
          var hash = _HashFile(file);
          Console.WriteLine(string.Format("{0}\t{1}", file, hash));
        }
        catch (Exception ex)
        {
          Console.WriteLine(string.Format("{0}\tError: {1}", file, ex.Message));
        }
      }
    }

    private string _HashFile(string file)
    {
      string ret = string.Empty;

      switch (Algorithm)
      {
        case HashAlgorithm.MD5:
          ret = _HashMD5(file);
          break;
        case HashAlgorithm.SHA1:
          ret = _HashSHA1(file);
          break;
        case HashAlgorithm.SHA256:
          ret = _HashSHA256(file);
          break;
      }

      return ret;
    }

    private string _HashMD5(string file)
    {
      string ret;

      using (var fs = new FileStream(file, FileMode.Open))
      {
        using (var md5 = MD5.Create())
        {
          byte[] hash = md5.ComputeHash(fs);
          ret = _FormatBytes(hash);
        }
      }

      return ret;
    }

    private string _HashSHA1(string file)
    {
      string ret;

      using (var fs = new FileStream(file, FileMode.Open))
      {
        using (var sha1 = new SHA1Managed())
        {
          byte[] hash = sha1.ComputeHash(fs);
          ret = _FormatBytes(hash);
        }
      }

      return ret;
    }

    private string _HashSHA256(string file)
    {
      string ret;

      using (var fs = new FileStream(file, FileMode.Open))
      {
        using (var sha256 = new SHA256Managed())
        {
          byte[] hash = sha256.ComputeHash(fs);
          ret = _FormatBytes(hash);
        }
      }

      return ret;
    }

    private string _FormatBytes(byte[] hash)
    {
      var ret = new StringBuilder(2 * hash.Length);
      foreach (var val in hash)
      {
        ret.AppendFormat("{0:x2}", val);
      }

      return ret.ToString();
    }
  }
}
