// Copyright Eric Chauvin 2018.
// My blog is at:
// https://scientificmodels.blogspot.com/



using System;
using System.Text;



namespace CodeAnalysis
{
  static class CSharpToStrings
  {


  internal static string MakeStringObjects( MainForm MForm, string InString )
    {
    // In C, a wide character string literal looks
    // like:  L"Hello world!" with the L in front of
    // it.

    StringBuilder SBuilder = new StringBuilder();

    // Notice the double slash in front of the quote
    // character here at the end of the string:
    // "c:\\BrowserECFiles\\PageFiles\\";

    InString = InString.Replace( "\\\\",
                Char.ToString( Markers.EscapedSlash ));

    InString = InString.Replace( "\\\"",
                Char.ToString( Markers.EscapedDoubleQuote ));

    string SingleQuoteCharStr = "\'\"\'";
    if( SingleQuoteCharStr.Length != 3 )
      {
      MForm.ShowStatus( "SingleQuoteCharStr.Length != 3" );
      return Char.ToString( Markers.ErrorPoint );
      }

    // This means that the double quote inside single
    // quotes will just be put inside the string
    // literal.
    InString = InString.Replace( SingleQuoteCharStr,
                Char.ToString( Markers.QuoteAsSingleCharacter ));

    bool IsInsideObject = false;
    bool IsInsideString = false;
    int Last = InString.Length;
    for( int Count = 0; Count < Last; Count++ )
      {
      char TestChar = InString[Count];

      if( TestChar == Markers.Begin )
        {
        IsInsideObject = true;
        SBuilder.Append( Char.ToString( TestChar ));
        continue;
        }

      if( IsInsideObject )
        {
        // You can't go inside another object without
        // finding the end of the string.
        if( IsInsideString )
          {
          SBuilder.Append( Char.ToString(
                        Markers.ErrorPoint ));
          SBuilder.Append( "String doesn't end before the next object." );
          return SBuilder.ToString();
          }

        if( TestChar == Markers.End )
          IsInsideObject = false;

        SBuilder.Append( Char.ToString( TestChar ));
        continue;
        }

      if( !IsInsideString )
        {
        if( TestChar == '"' )
          {
          IsInsideString = true;
          SBuilder.Append( Char.ToString(
                                    Markers.Begin ));
          SBuilder.Append( Char.ToString(
                                Markers.TypeString ));
          continue;
          }
        }
      else
        {
        // It is inside.
        if( TestChar == '"' )
          {
          IsInsideString = false;
          SBuilder.Append( Char.ToString(
                                      Markers.End ));
          continue;
          }
        }

      SBuilder.Append( Char.ToString( TestChar ));
      }

    string Result = SBuilder.ToString();

    Result = Result.Replace(
      Char.ToString( Markers.QuoteAsSingleCharacter ),
      SingleQuoteCharStr );

    Result = Result.Replace( Char.ToString(
                     Markers.EscapedDoubleQuote ), "\\\"" );

    Result = Result.Replace( Char.ToString(
                     Markers.EscapedSlash ), "\\\\" );

    return Result;
    }




  }
}
