using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace sfshu
{
  class Program
  {
    static void Main(string[] args)
    {
      _PrintHeader();

      //we should have 2 params, no more, no less
      if (args.Count() != 2)
      {
        Console.WriteLine("Invalid command line parameters! Try again!");
        return;
      }

      //we assume the first param is the path we are working with
      // so we should see if it acually exists
      if (!Directory.Exists(args[0]))
      {
        Console.WriteLine(string.Format("Path does not exist: {0}", args[0]));
        return;
      }

      //figure out the algo
      HashAlgorithm hash;
      switch(args[1])
      {
        case "/md5":
          hash = HashAlgorithm.MD5;
          break;
        case "/sha1":
          hash = HashAlgorithm.SHA1;
          break;
        case "/sha256":
          hash = HashAlgorithm.SHA256;
          break;
        case "/blake2b":
          hash = HashAlgorithm.BLAKE2b;
          break;
        default:
          Console.WriteLine(string.Format("Invalid Hash Selection: {0}", args[1]));
          return;
          break;
      }

      //let the user know what we're doing, in case they keep the log
      Console.WriteLine(string.Format("Working Path: {0}", args[0]));
      Console.WriteLine(string.Format("Using Hash: {0}", hash));
      Console.WriteLine();

      var prc = new HashProcessor(hash, args[0]);
      prc.Run();
    }

    private static void _PrintHeader()
    {
      Console.WriteLine(string.Format("SFSHU - Simple File System Hashing Utility v{0}",
        Assembly.GetExecutingAssembly().GetName().Version.ToString(2)));
      Console.WriteLine("Copyright (c) 2013 Adam Caudill <adam@adamcaudill.com>");
      Console.WriteLine("------");
      Console.WriteLine();
    }
  }
}
