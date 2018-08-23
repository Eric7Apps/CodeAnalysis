// Copyright Eric Chauvin 2018.
// My blog is at:
// https://scientificmodels.blogspot.com/



using System;
using System.Text;



namespace CodeAnalysis
{
  class CSharpToNumbers
  {
  private MainForm MForm;



  private CSharpToNumbers()
    {
    }



  internal CSharpToNumbers( MainForm UseForm )
    {
    MForm = UseForm;
    }



  private void ShowStatus( string ToShow )
    {
    if( MForm != null )
      MForm.ShowStatus( ToShow );

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



  internal string MakeNumberObjects( string InString )
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
        if( TestChar == Markers.End )
          IsInsideObject = false;

        SBuilder.Append( Char.ToString( TestChar ));
        continue;
        }

      if( TestChar == Markers.Begin )
        {
        if( IsInsideNumber )
          {
          IsInsideNumber = false;
          SBuilder.Append( Char.ToString(
                               Markers.End ));
          }

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
                                     Markers.Begin ));
          SBuilder.Append( Char.ToString(
                                Markers.TypeNumber ));
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
                               Markers.End ));
          SBuilder.Append( Char.ToString( TestChar ));
          continue;
          }
        }

      SBuilder.Append( Char.ToString( TestChar ));
      }

    string Result = SBuilder.ToString();
    return Result;
    }




  private bool IsNumberCharacter( char TestChar,
                                  char PreviousChar,
                                  char NextChar,
                                  int Position )
    {
    // This function is only the first approximation.
    // It will have to parse the whole number in a
    // later stage before it can figure out if it's
    // really a valid number or not.

    // Octal numbers are not used here since they are
    // antiquated.

    // If it doesn't see a decimal point or the e
    // letter for the exponent, or the d or f suffix,
    // then it assumes it's an integer value.

    // At this point any numeral is necessarily a
    // number, since this is not looking inside
    // of identifiers or strings or characters.
    if( IsNumeral( TestChar ))
      return true;

    // Do some obvious false ones and return from
    // this function sooner, even though this logic
    // is redundant.

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

    // 18.46e-2d
    if( (TestChar == 'd') ||
        (TestChar == 'D') ||
        (TestChar == 'e') ||
        (TestChar == 'E') ||
        (TestChar == 'f') ||
        (TestChar == 'F') ||
        (TestChar == 'l') ||
        (TestChar == 'L') ||
        (TestChar == 'p') || // Binary exponent.
        (TestChar == 'P') ||
        (TestChar == 'u') ||
        (TestChar == 'U'))
      {
      // 12lu is 12 as an unsigned long in C.
      // if( !IsNumeral( PreviousChar ))
        // return false;

      // It would still think this is a number:
      // 1d2d3d4f5F

      return true;
      }

    return false;
    }




  }
}
