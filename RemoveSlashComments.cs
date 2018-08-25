// Copyright Eric Chauvin 2018.
// My blog is at:
// https://scientificmodels.blogspot.com/




using System;
using System.Text;



namespace CodeAnalysis
{
  class RemoveSlashComments
  {
  private MainForm MForm;



  private RemoveSlashComments()
    {
    }



  internal RemoveSlashComments( MainForm UseForm )
    {
    MForm = UseForm;
    }



  private void ShowStatus( string ToShow )
    {
    if( MForm != null )
      MForm.ShowStatus( ToShow );

    }



  internal string RemoveAllDoubleSlashComments( string InString )
    {
    StringBuilder SBuilder = new StringBuilder();
    string[] SplitS = InString.Split( new Char[] { '\n' } );
    int Last = SplitS.Length;
    for( int Count = 0; Count < Last; Count++ )
      {
      string Line = SplitS[Count];
      Line = RemoveDoubleSlashComments( Line );

      string StartString =
              Char.ToString( Markers.Begin ) +
              Char.ToString( Markers.TypeLineNumber );

      string TLine = Line.Trim();
      if( !TLine.StartsWith( StartString ))
        SBuilder.Append( Line + "\n" );

      }

    return SBuilder.ToString();
    }



  private string RemoveDoubleSlashComments( string Line )
    {
    // This line with the two slashes in the URL should
    // not be considered to be a comment.
    //     GetPage( "https://gcc.gnu.org/" );

    // Notice the escaped slash in front of the quote
    // character here at the end of the string:
    // "c:\\BrowserECFiles\\PageFiles\\";

    StringBuilder SBuilder = new StringBuilder();

    Line = Line.Replace( "\\\\",
         Char.ToString( Markers.EscapedSlash ));

    Line = Line.Replace( "\\\"",
         Char.ToString( Markers.EscapedDoubleQuote ));

    // This double quote inside single quotes can't
    // be inside of a normal string literal.
    string SingleQuoteCharStr = "\'\"\'";
    if( SingleQuoteCharStr.Length != 3 )
      {
      ShowStatus( "SingleQuoteCharStr.Length != 3" ); 
      return Char.ToString( Markers.ErrorPoint );
      }

    Line = Line.Replace( SingleQuoteCharStr,
         Char.ToString( Markers.QuoteAsSingleCharacter ));

    string DoubleSlash = "/" + "/";
    Line = Line.Replace( DoubleSlash, Char.ToString( Markers.DoubleSlash ));

    int LineLength = Line.Length;
    bool IsInside = true;
    bool IsInsideString = false;
    for( int Count = 0; Count < LineLength; Count++ )
      {
      char TestChar = Line[Count];

      if( IsInsideString )
        {
        if( TestChar == '"' )
          IsInsideString = false;

        }
      else
        {
        // It's not inside the string.
        if( TestChar == '"' )
          IsInsideString = true;

        }

      if( !IsInsideString )
        {
        if( TestChar == Markers.DoubleSlash )
          {
          // This will stay false until it gets to
          // the Begin marker for the line number.
          IsInside = false;
          }
        }

      // This is for the line number markers.
      if( TestChar == Markers.Begin )
        IsInside = true;

      if( IsInside )
        SBuilder.Append( Char.ToString( TestChar ));

      }

    string Result = SBuilder.ToString();

    Result = Result.Replace( Char.ToString( Markers.DoubleSlash ), DoubleSlash );

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
