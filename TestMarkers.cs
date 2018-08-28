// Copyright Eric Chauvin 2018.
// My blog is at:
// https://scientificmodels.blogspot.com/



using System;
using System.Text;



namespace CodeAnalysis
{
  class TestMarkers
  {
  private MainForm MForm;


  private TestMarkers()
    {
    }



  internal TestMarkers( MainForm UseForm )
    {
    MForm = UseForm;
    }



  private void ShowStatus( string ToShow )
    {
    if( MForm != null )
      MForm.ShowStatus( ToShow );

    }



  internal bool TestBeginEnd( string InString )
    {
    string[] SplitS = InString.Split( new Char[] { Markers.Begin } );

    StringBuilder SBuilder = new StringBuilder();

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
        ShowStatus( SBuilder.ToString() );
        ShowStatus( " " );
        ShowStatus( "The line has no end marker." );
        ShowStatus( "Line: " + Line );
        return false;
        }

      string[] SplitLine = Line.Split( new Char[] { Markers.End } );
      if( SplitLine.Length < 2 )
        {
        ShowStatus( SBuilder.ToString() );
        ShowStatus( " " );
        ShowStatus( "SplitLine.Length < 2." );
        ShowStatus( Line );
        return false;
        }

      // ShowStatus( SplitLine[1] );
      }

    return true;
    }



  internal string RemoveOutsideWhiteSpace( string InString )
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




  internal bool TestBrackets( string InString )
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

        ShowStatus( ShowS );
        ShowStatus( "This is not a bracket: " + Char.ToString( TestChar ));

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

        ShowStatus( ShowS );
        ShowStatus( "Bracket count went negative." );
        return false;
        }

      }

    if( BracketCount != 0 )
      {
      ShowStatus( "Bracket count is not zero at the end." );
      return false;
      }

    return true;
    }



  }
}
