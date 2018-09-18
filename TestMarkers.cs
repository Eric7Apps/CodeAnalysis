// Copyright Eric Chauvin 2018.
// My blog is at:
// https://scientificmodels.blogspot.com/



using System;
using System.Text;



namespace CodeAnalysis
{
  static class TestMarkers
  {


  internal static bool TestBeginEnd( MainForm MForm, string InString )
    {
    StringBuilder SBuilder = new StringBuilder();

    string[] SplitS = InString.Split( new Char[] { Markers.Begin } );

    SBuilder.Append( SplitS[0] + "\r\n" );

    int Last = SplitS.Length;
    // This starts at 1, after the first
    // Markers.Begin.
    for( int Count = 1; Count < Last; Count++ )
      {
      string Line = SplitS[Count];
      SBuilder.Append( Line + "\r\n" );

      if( !Line.Contains( Char.ToString( Markers.End )))
        {
        MForm.ShowStatus( SBuilder.ToString() );
        MForm.ShowStatus( " " );
        MForm.ShowStatus( "The line has no end marker." );
        MForm.ShowStatus( "Line: " + Line );
        return false;
        }

      string[] SplitLine = Line.Split( new Char[] { Markers.End } );
      if( SplitLine.Length < 2 )
        {
        MForm.ShowStatus( SBuilder.ToString() );
        MForm.ShowStatus( " " );
        MForm.ShowStatus( "SplitLine.Length < 2." );
        MForm.ShowStatus( Line );
        return false;
        }
      }

    return true;
    }



  internal static string RemoveOutsideWhiteSpace( string InString )
    {
    StringBuilder SBuilder = new StringBuilder();

    bool IsInsideObject = false;
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

      if( TestChar == Markers.End )
        {
        IsInsideObject = false;
        SBuilder.Append( Char.ToString( TestChar ));
        continue;
        }

      if( IsInsideObject )
        {
        SBuilder.Append( Char.ToString( TestChar ));
        continue;
        }

      if( TestChar == ' ' )
        continue;

      if( TestChar == '\n' )
        continue;

      SBuilder.Append( Char.ToString( TestChar ));
      }

    string Result = SBuilder.ToString();
    return Result;
    }




  internal static bool TestBrackets( MainForm MForm, string InString )
    {
    StringBuilder SBuilder = new StringBuilder();
    int BracketCount = 0;
    bool IsInsideObject = false;
    int Last = InString.Length;
    for( int Count = 0; Count < Last; Count++ )
      {
      char TestChar = InString[Count];
      SBuilder.Append( Char.ToString( TestChar ));

      if( IsInsideObject )
        {
        if( TestChar == Markers.End )
          IsInsideObject = false;

        continue;
        }

      if( TestChar == Markers.Begin )
        {
        IsInsideObject = true;
        continue;
        }

      if( !((TestChar == '{') || (TestChar == '}')))
        {
        string ShowS = SBuilder.ToString();
        ShowS = ShowS.Replace(
             Char.ToString( Markers.Begin ), "\r\n" );

        MForm.ShowStatus( ShowS );
        MForm.ShowStatus( "This is not a bracket: " + Char.ToString( TestChar ));
        return false;
        }

      if( TestChar == '{' )
        BracketCount++;

      if( TestChar == '}' )
        BracketCount--;

      if( BracketCount < 0 )
        {
        string ShowS = SBuilder.ToString();
        ShowS = ShowS.Replace(
             Char.ToString( Markers.Begin ), "\r\n" );

        MForm.ShowStatus( ShowS );
        MForm.ShowStatus( "Bracket count went negative." );
        return false;
        }

      }

    if( BracketCount != 0 )
      {
      MForm.ShowStatus( "Bracket count is not zero at the end." );
      return false;
      }

    return true;
    }



  }
}
