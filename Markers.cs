// Copyright Eric Chauvin 2018.
// My blog is at:
// https://scientificmodels.blogspot.com/



using System;
using System.Text;



namespace CodeAnalysis
{
  static class Markers
  {
  // Marker symbols go from 0x2700 to 0x27BF.
  internal const char EscapedSingleQuote = (char)0x2700;
  internal const char EscapedDoubleQuote = (char)0x2701;
  internal const char Begin = (char)0x2702;
  internal const char End = (char)0x2703;
  internal const char TypeString = (char)0x2704;
  internal const char TypeChar = (char)0x2705;
  internal const char TypeNumber = (char)0x2706;
  internal const char TypeIdentifier = (char)0x2707;
  internal const char ErrorPoint = (char)0x2708;
  internal const char TypeLineNumber = (char)0x2709;



  static internal bool IsMarker( char TestChar )
    {
    int Value = (int)TestChar;
    if( (Value >= 0x2700) && (Value <= 0x27BF))
      return true;

    return false;
    }



  }
}
