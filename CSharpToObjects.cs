// Copyright Eric Chauvin 2018.
// My blog is at:
// https://scientificmodels.blogspot.com/



using System;
using System.Text;



namespace CodeAnalysis
{
  class CSharpToObjects
  {
  private MainForm MForm;
  private StringArray MainSArray;




  private CSharpToObjects()
    {
    }



  internal CSharpToObjects( MainForm UseForm,
                            StringArray SArrayToUse )
    {
    MForm = UseForm;

    MainSArray = SArrayToUse;
    }



  private void ShowStatus( string ToShow )
    {
    if( MForm == null )
      {
      // Writing from another thread?
      return;
      }

    MForm.ShowStatus( ToShow );
    }



  internal void ShowLines()
    {
    int Last = MainSArray.GetLast();
    for( int Count = 0; Count < Last; Count++ )
      {
      string Line = MainSArray.GetStringAt( Count );
      ShowStatus( Line );
      }
    }



  internal string MakeAllObjects()
    {
    string AllSource = MainSArray.GetArrayAsString( "\n" );
    AllSource += "\n";

    string Result = AllSource;

    // These have to be processed in this order.
    Result = MakeStringObjects( Result );
    Result = MakeCharObjects( Result );
    Result = MakeIdentifierObjects( Result );
    Result = MakeNumberObjects( Result );

    // ShowStatus( " " );
    // ShowStatus( " " );
    // ShowStatus( "///////////////////////////////" );
    // ShowStatus( "The marked string:" );
    // ShowStatus( Result );

    if( Result.Contains( Char.ToString(
                        Constants.MarkerErrorPoint )))
      {
      ShowStatus( " " );
      ShowStatus( "It had an error." );
      }

    return Result;
    }



  private bool IsNumeral( char ToTest )
    {
    if( (ToTest >= '0') && (ToTest <= '9'))
      return true;

    return false;
    }



  private bool IsLetter( char ToTest )
    {
    if( (ToTest >= 'a') && (ToTest <= 'z'))
      return true;

    if( (ToTest >= 'A') && (ToTest <= 'Z'))
      return true;

    // You could add Chinese characters or something
    // like that, so it's not just ASCII letters.

    return false;
    }



  private string MakeStringObjects( string InString )
    {
    StringBuilder SBuilder = new StringBuilder();

    InString = InString.Replace( "\\\"",
       Char.ToString( Constants.EscapedDoubleQuote ));

    int LineNumber = 1;
    bool IsInsideString = false;
    int Last = InString.Length;
    for( int Count = 0; Count < Last; Count++ )
      {
      char TestChar = InString[Count];
      if( TestChar == '\n' )
        LineNumber++;

      if( !IsInsideString )
        {
        if( TestChar == '"' )
          {
          IsInsideString = true;
          SBuilder.Append( Char.ToString(
                             Constants.MarkerBegin ));
          SBuilder.Append( Char.ToString(
                        Constants.MarkerTypeString ));
          continue;
          }
        }
      else
        {
        // It is inside.
        if( TestChar == '\n' )
          {
          // You can't get to a newline while it's
          // still inside the string.
          SBuilder.Append( Char.ToString(
                        Constants.MarkerErrorPoint ));
          SBuilder.Append( "LineNumber: " + LineNumber.ToString() );
          SBuilder.Append( "Newline inside string." );
          return SBuilder.ToString();
          }

        if( TestChar == '"' )
          {
          IsInsideString = false;
          SBuilder.Append( Char.ToString(
                                Constants.MarkerEnd ));
          continue;
          }
        }

      SBuilder.Append( Char.ToString( TestChar ));
      }

    string Result = SBuilder.ToString();

    // Put escaped quote character back in.
    Result = Result.Replace( Char.ToString(
             Constants.EscapedDoubleQuote ), "\\\"" );
    return Result;
    }



  private string MakeCharObjects( string InString )
    {
    StringBuilder SBuilder = new StringBuilder();

    // It could be '\''.
    InString = InString.Replace( "\\\'",
       Char.ToString( Constants.EscapedSingleQuote ));

    int LineNumber = 1;
    bool IsInsideChar = false;
    bool IsInsideObject = false;
    int Last = InString.Length;
    for( int Count = 0; Count < Last; Count++ )
      {
      char TestChar = InString[Count];
      if( TestChar == '\n' )
        LineNumber++;

      if( IsInsideObject )
        {
        // You can't go inside another object without
        // finding the end of the character.
        if( IsInsideChar )
          {
          SBuilder.Append( Char.ToString(
                        Constants.MarkerErrorPoint ));
          SBuilder.Append( "LineNumber: " + LineNumber.ToString() );
          SBuilder.Append( "Character doesn't end before next object." );
          return SBuilder.ToString();
          }

        if( TestChar == Constants.MarkerEnd )
          IsInsideObject = false;

        SBuilder.Append( Char.ToString( TestChar ));
        continue;
        }

      if( TestChar == Constants.MarkerBegin )
        {
        IsInsideObject = true;
        SBuilder.Append( Char.ToString( TestChar ));
        continue;
        }

      if( !IsInsideChar )
        {
        if( TestChar == '\'' )
          {
          IsInsideChar = true;
          SBuilder.Append( Char.ToString(
                             Constants.MarkerBegin ));
          SBuilder.Append( Char.ToString(
                          Constants.MarkerTypeChar ));
          continue;
          }
        }
      else
        {
        // It is inside.
        if( TestChar == '\'' )
          {
          IsInsideChar = false;
          SBuilder.Append( Char.ToString(
                               Constants.MarkerEnd ));
          continue;
          }
        }

      SBuilder.Append( Char.ToString( TestChar ));
      }

    string Result = SBuilder.ToString();

    // Put the single quote character back in.
    Result = Result.Replace( Char.ToString(
             Constants.EscapedSingleQuote ), "\\\'" );

    return Result;
    }



  private string MakeIdentifierObjects( string InString )
    {
    StringBuilder SBuilder = new StringBuilder();

    char PreviousChar = '\n';
    bool IsInsideID = false;
    bool IsInsideObject = false;
    int Position = 0;
    int Last = InString.Length;
    for( int Count = 0; Count < Last; Count++ )
      {
      char TestChar = InString[Count];

      if( Count > 0 )
        PreviousChar = InString[Count - 1];

      if( IsInsideObject )
        {
        if( TestChar == Constants.MarkerEnd )
          IsInsideObject = false;

        SBuilder.Append( Char.ToString( TestChar ));
        continue;
        }

      if( TestChar == Constants.MarkerBegin )
        {
        IsInsideObject = true;
        SBuilder.Append( Char.ToString( TestChar ));
        continue;
        }

      if( !IsInsideID )
        {
        Position = 0;
        if( IsIdentifierCharacter( TestChar,
                                   PreviousChar,
                                   Position ))
          {
          IsInsideID = true;
          SBuilder.Append( Char.ToString(
                             Constants.MarkerBegin ));
          SBuilder.Append( Char.ToString(
                    Constants.MarkerTypeIdentifier ));
          SBuilder.Append( Char.ToString( TestChar ));
          continue;
          }
        }
      else
        {
        // It is inside.
        Position++;
        if( !IsIdentifierCharacter( TestChar,
                                    PreviousChar,
                                    Position ))
           {
          IsInsideID = false;
          SBuilder.Append( Char.ToString(
                               Constants.MarkerEnd ));
          SBuilder.Append( Char.ToString( TestChar ));
          continue;
          }
        }

      SBuilder.Append( Char.ToString( TestChar ));
      }

    string Result = SBuilder.ToString();
    return Result;
    }




  private bool IsIdentifierCharacter( char ToTest,
                                      char PreviousChar,
                                      int Where )
    {
    if( Where > 1024 )
      return false;

    if( Where == 0 )
      {
      if( IsNumeral( ToTest ))
        return false;

      // No matter what the first character of the
      // identifier is, it can't start off
      // immediately following a numeral with no
      // space or anything between the two.
      // 123.45f;
      if( IsNumeral( PreviousChar ))
        return false;

      // You can't _start_ an identifier with a
      // letter immediately before it.
      if( IsLetter( PreviousChar ))
        return false;

      if( PreviousChar == '_' )
        return false;

      }

    if( (ToTest == '_') ||
         IsLetter( ToTest ) ||
         IsNumeral( ToTest ))
      return true;

    return false;
    }



  private string MakeNumberObjects( string InString )
    {
    StringBuilder SBuilder = new StringBuilder();

    char PreviousChar = '\n';
    char NextChar = '\n';
    bool IsInsideNumber = false;
    bool IsInsideObject = false;
    int Position = 0;
    int Last = InString.Length;
    for( int Count = 0; Count < Last; Count++ )
      {
      char TestChar = InString[Count];
      if( Count > 0 )
        PreviousChar = InString[Count - 1];

      if( (Count + 1) < Last )
        NextChar = InString[Count + 1];
      else
        NextChar = '\n';

      if( IsInsideObject )
        {
        if( TestChar == Constants.MarkerEnd )
          IsInsideObject = false;

        SBuilder.Append( Char.ToString( TestChar ));
        continue;
        }

      if( TestChar == Constants.MarkerBegin )
        {
        IsInsideObject = true;
        SBuilder.Append( Char.ToString( TestChar ));
        continue;
        }

      if( !IsInsideNumber )
        {
        Position = 0;
        if( IsNumberCharacter( TestChar,
                               PreviousChar,
                               NextChar,
                               Position ))
          {
          IsInsideNumber = true;
          SBuilder.Append( Char.ToString(
                             Constants.MarkerBegin ));
          SBuilder.Append( Char.ToString(
                        Constants.MarkerTypeNumber ));
          SBuilder.Append( Char.ToString( TestChar ));
          continue;
          }
        }
      else
        {
        // It is inside.
        Position++;
        if( !IsNumberCharacter( TestChar,
                                PreviousChar,
                                NextChar,
                                Position ))
          {
          IsInsideNumber = false;
          SBuilder.Append( Char.ToString(
                               Constants.MarkerEnd ));
          SBuilder.Append( Char.ToString( TestChar ));
          continue;
          }
        }

      SBuilder.Append( Char.ToString( TestChar ));
      }

    string Result = SBuilder.ToString();
    return Result;
    }



  private bool IsMarker( char TestChar )
    {
    int Value = (int)TestChar;
    if( (Value >= 0x2700) && (Value <= 0x27BF))
      return true;

    return false;
    }




  private bool IsNumberCharacter( char TestChar,
                                  char PreviousChar,
                                  char NextChar,
                                  int Position )
    {
    // Octal numbers are not used here since they are
    // very antiquated.  They would be an error.

    // At this point any numeral is necessarily a
    // number, since this is not looking inside
    // of identifiers or strings or characters.
    if( IsNumeral( TestChar ))
      return true;

    // Do some obvious false ones.
    if( TestChar == '\n' )
      return false;

    if( TestChar == ' ' )
      return false;

    if( TestChar == ';' )
      return false;

    if( TestChar == ')' )
      return false;

    if( IsLetter( TestChar ) && (Position == 0))
      return false;

    if( TestChar == '.' )
      {
      if( Position == 0 )
        return false;

      // Do validity check for more than one period.
      return true;
      }

    if( TestChar == '-' )
      {
      if( !IsNumeral( NextChar ))
        return false;

      if( Position != 0 )
        {
        // 18.46e-2
        if( !((PreviousChar == 'e') ||
              (PreviousChar == 'E')))
          return false;

        }
      }

    if( (TestChar == 'x') || (TestChar == 'X'))
      {
      if( PreviousChar != '0' )
        return false;

      // This is the only place where there can
      // be an X.
      if( Position == 1 )
        return true;
      else
        return false;

      }

    if( (TestChar == 'd') ||
        (TestChar == 'D') ||
        (TestChar == 'e') ||
        (TestChar == 'E') ||
        (TestChar == 'f') ||
        (TestChar == 'F'))
      {
      if( !IsNumeral( PreviousChar ))
        return false;

      // It would still think this is a number:
      // 1d2d3d4f5F
      // It would have to parse the whole number
      // before it can figure out if it's a valid
      // number.

      return true;
      }

    return false;
    }




  }
}
