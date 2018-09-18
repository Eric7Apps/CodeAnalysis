// Copyright Eric Chauvin 2018.
// My blog is at:
// https://scientificmodels.blogspot.com/



// This is only the first approximation.
// It will have to parse the whole number in a
// later stage before it can figure out if it's
// really a valid number or not.

// Octal numbers are not used here since they are
// antiquated.



using System;
using System.Text;



namespace CodeAnalysis
{
  static class CSharpToNumbers
  {

  private static bool IsNumeral( char ToTest )
    {
    if( (ToTest >= '0') && (ToTest <= '9'))
      return true;

    return false;
    }



  private static bool IsLetter( char ToTest )
    {
    if( (ToTest >= 'a') && (ToTest <= 'z'))
      return true;

    if( (ToTest >= 'A') && (ToTest <= 'Z'))
      return true;

    // You could add Chinese characters or something
    // like that, so it's not just ASCII letters.

    return false;
    }



  internal static string MakeNumberObjects( string InString )
    {
    StringBuilder SBuilder = new StringBuilder();

    char PreviousChar = '\n';
    char NextChar = '\n';
    bool IsInsideNumber = false;
    bool IsInsideObject = false;
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

      if( TestChar == Markers.End )
        {
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

      if( IsInsideObject )
        {
        SBuilder.Append( Char.ToString( TestChar ));
        continue;
        }

      if( !IsInsideNumber )
        {
        if( IsNumberStart( TestChar ))
          {
          IsInsideNumber = true;
          SBuilder.Append( Char.ToString(
                                     Markers.Begin ));
          SBuilder.Append( Char.ToString(
                                Markers.TypeNumber ));
          SBuilder.Append( Char.ToString( TestChar ));
          continue;
          }

        SBuilder.Append( Char.ToString( TestChar ));
        continue;
        }

      // At this point it is inside a number.
      if( !IsNumberCharacterContinued( TestChar,
                                       PreviousChar,
                                       NextChar ))
        {
        IsInsideNumber = false;
        SBuilder.Append( Char.ToString( Markers.End ));
        SBuilder.Append( Char.ToString( TestChar ));
        continue;
        }

      // It is continuing inside a number.
      SBuilder.Append( Char.ToString( TestChar ));
      }

    string Result = SBuilder.ToString();
    return Result;
    }



  private static bool IsNumberStart( char TestChar )
    {
    // At this point any numeral is necessarily a
    // number, since this is not looking inside
    // of identifiers or strings or characters.
    if( IsNumeral( TestChar ))
      return true;

    // If it is before the number starts then the
    // negative sign is an operator.  Possibly a
    // unary operator.
    // if( TestChar == '-' )

    return false;
    }




  private static bool IsNumberCharacterContinued(
                                  char TestChar,
                                  char PreviousChar,
                                  char NextChar )
    {
    if( IsNumeral( TestChar ))
      return true;

    // This would allow 1.2.3.4.5.6 and so on...
    if( TestChar == '.' )
      return true;

    if( TestChar == '-' )
      {
      if( !IsNumeral( NextChar ))
        return false;

      // 18.46e-2
      if( !((PreviousChar == 'e') ||
            (PreviousChar == 'E')))
        return false;

      }

    // This would allow 0x0x0x0x0 and so on...
    if( (TestChar == 'x') || (TestChar == 'X'))
      {
      if( PreviousChar == '0' )
        return true;

      }

    // 18.46e-2d
    // 0xABCD
    if( (TestChar == 'a') ||
        (TestChar == 'A') ||
        (TestChar == 'b') ||
        (TestChar == 'B') ||
        (TestChar == 'c') ||
        (TestChar == 'C') ||
        (TestChar == 'd') ||
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

      // This still allows all kinds of things that
      // aren't numbers, like 01abcdefu0x.

      return true;
      }

    return false;
    }



  }
}
