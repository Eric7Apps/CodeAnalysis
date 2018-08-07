// Copyright Eric Chauvin 2018.
// My blog is at:
// https://scientificmodels.blogspot.com/



using System;
using System.Text;



namespace CodeAnalysis
{
  static class Constants
  {
  // Marker symbols go from 0x2700 to 0x27BF.
  internal const char EscapedSingleQuote = (char)0x2700;
  internal const char EscapedDoubleQuote = (char)0x2701;
  internal const char MarkerBegin = (char)0x2702;
  internal const char MarkerEnd = (char)0x2703;
  internal const char MarkerTypeString = (char)0x2704;
  internal const char MarkerTypeChar = (char)0x2705;
  internal const char MarkerTypeNumber = (char)0x2706;
  internal const char MarkerTypeIdentifier = (char)0x2707;
  internal const char MarkerErrorPoint = (char)0x2708;



  }
}
