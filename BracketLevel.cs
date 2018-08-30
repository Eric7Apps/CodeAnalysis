// Copyright Eric Chauvin 2018.
// My blog is at:
// https://scientificmodels.blogspot.com/



using System;
using System.Text;



namespace CodeAnalysis
{
  class BracketLevel
  {
  private MainForm MForm;



  private BracketLevel()
    {
    }



  internal BracketLevel( MainForm UseForm )
    {
    MForm = UseForm;
    }



  private void ShowStatus( string ToShow )
    {
    if( MForm != null )
      MForm.ShowStatus( ToShow );

    }



  internal string SetLevelChars( string InString )
    {
    StringBuilder SBuilder = new StringBuilder();

    int Level = 0;
    bool IsInsideObject = false;
    int Last = InString.Length;
    for( int Count = 0; Count < Last; Count++ )
      {
      char TestChar = InString[Count];

      if( TestChar == Markers.End )
        {
        IsInsideObject = false;
        SBuilder.Append( Char.ToString( TestChar ));
        continue;
        }

      if( TestChar == Markers.Begin )
        {
        IsInsideObject = true;
        SBuilder.Append( Char.ToString( TestChar ));
        // Mark the level in each object.
        SBuilder.Append( Level.ToString());
        continue;
        }

      if( IsInsideObject )
        {
        SBuilder.Append( Char.ToString( TestChar ));
        continue;
        }

      if( !((TestChar == '{') || (TestChar == '}')))
        {
        string ShowS = SBuilder.ToString();
        ShowS = ShowS.Replace(
             Char.ToString( Markers.Begin ), "\r\n" );

        ShowStatus( ShowS );
        ShowStatus( "This should only be a bracket: " + Char.ToString( TestChar ));
        return "";
        }

      if( TestChar == '{' )
        Level++;

      if( TestChar == '}' )
        Level--;

      if( Level < 0 )
        {
        string ShowS = SBuilder.ToString();
        ShowS = ShowS.Replace(
             Char.ToString( Markers.Begin ), "\r\n" );

        ShowStatus( ShowS );
        ShowStatus( "Bracket count went negative." );
        return "";
        }
      }

    if( Level != 0 )
      {
      ShowStatus( "Bracket count is not zero at the end." );
      return "";
      }

    string Result = SBuilder.ToString();
    ShowAsTokens( Result );
    return Result;
    }



  internal void ShowAsTokens( string InString )
    {
    ShowStatus( " " );
    ShowStatus( " " );
    ShowStatus( "As Tokens:" );

    string[] SplitS = InString.Split( new Char[] { Markers.Begin } );
    int Last = SplitS.Length;
    // Count starts at 1:
    for( int Count = 1; Count < Last; Count++ )
      {
      string Line = SplitS[Count];
      ShowStatus( Line );
      }

    }



  }
}
