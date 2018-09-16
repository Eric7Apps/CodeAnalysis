// Copyright Eric Chauvin 2018.
// My blog is at:
// https://scientificmodels.blogspot.com/



using System;
using System.Text;



namespace CodeAnalysis
{
  class Token
  {
  private MainForm MForm;
  private string Text = "";
  private int Level = 0;
  private char TokenType = ' '; // Markers.TypeString
  // This Token can contain other Tokens.
  // For ordinary tokens the array would be null.
  private Token[] TokenArray = null;
  private int TokenArrayLast = 0;



  private Token()
    {
    }



  internal Token( MainForm UseForm )
    {
    MForm = UseForm;
    }



  private void ShowStatus( string ToShow )
    {
    if( MForm != null )
      MForm.ShowStatus( ToShow );

    }


/*
  internal void ClearArray()
    {
    TokenArray = null;
    TokenArrayLast = 0;
    }
*/



  internal bool AppendTokenToArray( Token Tk )
    {
    try
    {
    if( TokenArray == null )
      TokenArray = new Token[256];

    if( TokenArrayLast >= TokenArray.Length )
      {
      Array.Resize( ref TokenArray, TokenArray.Length + 1024 );
      }

    TokenArray[TokenArrayLast] = Tk;
    TokenArrayLast++;
    return true;
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in Token.AppendTokenToArray(). " + Except.Message );
      return false;
      }
    }




  internal bool AddTokensFromString( string InString )
    {
    string[] SplitS = InString.Split( new Char[] { Markers.Begin } );

    // ShowStatus( "Split at zero: >" + SplitS[0] + "<" );
    // ShowStatus( " " );

    int Last = SplitS.Length;
    // Count starts at 1:
    for( int Count = 1; Count < Last; Count++ )
      {
      string Line = SplitS[Count];
      // ShowStatus( " " );
      // ShowStatus( Line );
      Token Tk = MakeTokenFromString( Line );
      if( Tk == null )
        return false;

      if( !AppendTokenToArray( Tk ))
        return false;

      }

    // ShowTokens();
    return true;
    }



  private void ShowTokens()
    {
    ShowStatus( " " );
    ShowStatus( " " );
    ShowStatus( "Showing Tokens." );

    for( int Count = 0; Count < TokenArrayLast; Count++ )
      {
      Token Tk = TokenArray[Count];
      ShowStatus( " " );
      if( Tk.TokenType == Markers.TypeLineNumber )
        ShowStatus( "Line Number: " );

      ShowStatus( "Text: " + Tk.Text );
      ShowStatus( "Level: " + Tk.Level.ToString() );
      }
    }




  private Token MakeTokenFromString( string InString )
    {
    try
    {
    Token Tk = new Token( MForm );
    string LevelStr = "";
    int Position = 0;
    int Last = InString.Length;
    for( int Count = 0; Count < Last; Count++ )
      {
      char TestChar = InString[Count];
      if( Markers.IsMarker( TestChar ))
        {
        Tk.TokenType = TestChar;
        Position = Count + 1;
        break;
        }

      LevelStr += Char.ToString( TestChar );
      }

    // ShowStatus( "LevelStr: " + LevelStr );
    Tk.Level = Int32.Parse( LevelStr );
    // ShowStatus( "Level: " + Tk.Level.ToString());

    for( int Count = Position; Count < Last; Count++ )
      {
      char TestChar = InString[Count];
      if( Markers.IsMarker( TestChar ))
        break;

      Tk.Text += Char.ToString( TestChar );
      }

    // ShowStatus( "Text: " + Tk.Text );
    return Tk;
    }
    catch( Exception Except )
      {
      ShowStatus( "Exception in Token.MakeTokenFromString():" );
      ShowStatus( Except.Message );
      return null;
      }
    }




  private int GetLowestLevel()
    {
    int Lowest = 2000000000;
    for( int Count = 0; Count < TokenArrayLast; Count++ )
      {
      Token Tk = TokenArray[Count];
      if( Lowest > Tk.Level )
        Lowest = Tk.Level;

      // if( Tk.TokenType == Markers.TypeLineNumber )
      }

    return Lowest;
    }



  internal bool SetLowestTokenBlocks()
    {
    if( TokenArray == null )
      return true; // Nothing to do.

    Token[] TempTokenArray = new Token[TokenArrayLast];
    int TempTokenArrayLast = 0;

    int Lowest = GetLowestLevel();
    // ShowStatus( " " );
    // ShowStatus( "Lowest: " + Lowest.ToString());

    Token BlockTk = null;
    for( int Count = 0; Count < TokenArrayLast; Count++ )
      {
      Token Tk = TokenArray[Count];
      if( Lowest == Tk.Level )
        {
        if( BlockTk != null )
          {
          TempTokenArray[TempTokenArrayLast] = BlockTk;
          TempTokenArrayLast++;
          // Do this recursively.
          if( !BlockTk.SetLowestTokenBlocks())
            return false;

          BlockTk = null;
          }

        TempTokenArray[TempTokenArrayLast] = Tk;
        TempTokenArrayLast++;
        }
      else
        {
        if( BlockTk == null )
          {
          BlockTk = new Token( MForm );
          BlockTk.Text = "Block";
          }

        if( !BlockTk.AppendTokenToArray( Tk ))
          return false;

        }
      }

    if( BlockTk != null )
      {
      ShowStatus( "BlockTK should be null here." );
      return false;
      }

    // ShowStatus( "End of this one." );

    TokenArray = TempTokenArray;
    TokenArrayLast = TempTokenArrayLast;
    return true;
    }




  internal void ShowTokensAtLevel( int ShowLevel )
    {
    if( TokenArray == null )
      {
      if( TokenType == Markers.TypeLineNumber )
        return;

      if( Level == ShowLevel )
        ShowStatus( Text );

      return;
      }


    ShowStatus( "Block" );

    for( int Count = 0; Count < TokenArrayLast; Count++ )
      {
      Token Tk = TokenArray[Count];
      Tk.ShowTokensAtLevel( ShowLevel );
      }
    }



  }
}
