// Copyright Eric Chauvin 2018.
// My blog is at:
// ericsourcecode.blogspot.com


using System;
using System.Text;


namespace CodeAnalysis
{
  public class SourceFileToken
  {
  private MainForm MForm;
  internal string Text = "";
  internal EnumTokenType TokenType = EnumTokenType.Character;



  internal enum EnumTokenType { StringLiteral,
                                PreProcessor,
                                Identifier,
                                Character,
                                EndOfLine };


  private SourceFileToken()
    {
    }



  internal SourceFileToken( MainForm UseForm )
    {
    MForm = UseForm;

    }


/*
  internal string GetText()
    {
    return Text;
    }
*/


/*
  internal string GetTokenTypeString()
    {
    if( TokenType == EnumTokenType.DoubleSlash )
      return "DoubleSlash";

    // TripleSlash,

    if( TokenType == EnumTokenType.SlashStar )
      return "SlashStar";

    if( TokenType == EnumTokenType.StarSlash )
      return "StarSlash";

    if( TokenType == EnumTokenType.Character )
      return "Character";

    if( TokenType == EnumTokenType.EndOfLine )
      return "EndOfLine";

    return "Enum type not found.";
    }
*/


/*
  internal EnumTokenType GetTokenType()
    {
    return TokenType;
    }
*/


/*
  internal bool SetTokenTypeFromText()
    {
    if( Text.Length == 0 )
      {
      TokenType = EnumTokenType.Character;
      return false;
      }

    if( Text == ("/" + "/") )
      {
      TokenType = EnumTokenType.DoubleSlash;
      return true;
      }

    if( Text == ("/" + "*") )
      {
      TokenType = EnumTokenType.SlashStar;
      return true;
      }

    if( Text == ("*" + "/") )
      {
      TokenType = EnumTokenType.StarSlash;
      return true;
      }

    TokenType = EnumTokenType.Character;
    return true; // For what ever a Character is.
    }
*/



  private void ShowStatus( string ToShow )
    {
    if( MForm == null )
      {
      // Writing from another thread?
      return;
      }

    MForm.ShowStatus( ToShow );
    }


/*
  internal bool IsBeyondTokenText( char ToCheck )
    {
    if( Text.Length == 0 )
      return false; // It is not beyond anything.

    if( Text.Length == 1 )
      return IsBeyondTokenText1( ToCheck );

    // if( Text.Length == 2 )
      // return IsBeyondTokenText2( ToCheck );

    // if( Text.Length == 3 )
      // return IsBeyondTokenText3( ToCheck );

    // if( Text.Length == 4 )
      // return IsBeyondTokenText4( ToCheck );

    return true;  // It's beyond any token length.
    }
*/


/*
  internal bool IsBeyondTokenText1( char ToCheck )
    {
    // if( Text == " " )
      // {
          // If a space follows this one.
         // if( Text == " " )
      // return true;
      // }

    if( Text == "/" )
      {
      if( ToCheck == '/' )
        return false;

      if( ToCheck == '*' )
        return false;

      return true; // Anything else is beyond it.
      }

    if( Text == "*" )
      {
      if( ToCheck == '/' )
        return false;

      return true; // Anything else is beyond it.
      }

    return true; // It is beyond the end of the token.
    }
*/


  /*
  internal bool IsBeyondTokenText2( char ToCheck )
    {
    if( Text == "</" )
      return true;

    if( Text == "/>" )
      return true;

    if( Text == "<!" )
      {
      if( ToCheck == '-' )
        return false; //      <!-

      return true;
      }

    if( Text == "--" )
      {
      if( ToCheck == '>' )
        return false; //      -->

      return true;
      }

    return true;
    }
    */



  /*
  internal bool IsBeyondTokenText3( char ToCheck )
    {
    if( Text == "-->" )
      return true;

    if( Text == "<!-" )
      {
      if( ToCheck == '-' )
        return false; //      <!--

      return true;
      }

    return true;
    }
    */


  /*
  internal bool IsBeyondTokenText4( char ToCheck )
    {
    // if( Text == "<!--" )
      // return true; // Anything is beyond it.

    return true;
    }
    */



  internal void AddCharacter( char ToAdd )
    {
    Text += Char.ToString( ToAdd );
    }



  }
}
