//code by: strudso - http://stackoverflow.com/users/105511/strudso
//from: http://stackoverflow.com/a/5957525/230543

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

internal static class SafeWalk
{
  public static IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOpt)
  {
    try
    {
      var dirFiles = Enumerable.Empty<string>();
      if (searchOpt == SearchOption.AllDirectories)
      {
        dirFiles = Directory.EnumerateDirectories(path)
                            .SelectMany(x => EnumerateFiles(x, searchPattern, searchOpt));
      }
      return dirFiles.Concat(Directory.EnumerateFiles(path, searchPattern));
    }
    catch (UnauthorizedAccessException)
    {
      return Enumerable.Empty<string>();
    }
  }
}