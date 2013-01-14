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
      //see if we have a single file, path
      if (File.Exists(Path))
      {
        _RunFile(Path);
      }
      else
      {
        foreach (var file in SafeWalk.EnumerateFiles(Path, "*", SearchOption.AllDirectories))
        {
          _RunFile(file);
        }
      }
    }

    private void _RunFile(string file)
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

    private string _HashFile(string file)
    {
      string ret = string.Empty;

      switch (Algorithm)
      {
        case HashAlgorithm.MD5:
          ret = _HashStandard(file, MD5.Create());
          break;
        case HashAlgorithm.SHA1:
          ret = _HashStandard(file, new SHA1Managed());
          break;
        case HashAlgorithm.SHA256:
          ret = _HashStandard(file, new SHA256Managed());
          break;
        case HashAlgorithm.BLAKE2b:
          ret = _HashBLAKE2b(file);
          break;
      }

      return ret;
    }

    private string _HashStandard(string file, System.Security.Cryptography.HashAlgorithm algorithm)
    {
      string ret;

      using (var fs = new FileStream(file, FileMode.Open))
      {
        using (algorithm)
        {
          var hash = algorithm.ComputeHash(fs);
          ret = _FormatBytes(hash);
        }
      }

      return ret;
    }

    private string _HashBLAKE2b(string file)
    {
      string ret;

      using (var fs = new FileStream(file, FileMode.Open))
      {
        var blake = Blake2Sharp.Blake2B.Create();
        
        var data = new byte[4096];
        int cbSize;

        do
        {
          cbSize = fs.Read(data, 0, 4096);
          if (cbSize > 0)
            blake.Update(data, 0, cbSize);
        } while (cbSize > 0);

        var hash = blake.Finish();

        ret = _FormatBytes(hash);
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
