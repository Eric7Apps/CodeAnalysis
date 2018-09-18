// Copyright Eric Chauvin 2018.
// My blog is at:
// https://scientificmodels.blogspot.com/



// I'm using the word 'operator' in a broader
// sense here, like for example a parentheses or a
// comma operates on the language.  But it will be
// narrowed down later to different types of
// operators, like the mathematical operators.

// This ternary operator is not used: x?y:z
// These types aren't used: +=, -=, *=, /=,
// There are no three-character operators.



using System;
using System.Text;



namespace CodeAnalysis
{
  static class CSharpToOperators
  {

  internal static string MakeOperatorObjects( string InString )
    {
    StringBuilder SBuilder = new StringBuilder();

    char PreviousChar = ' ';
    bool IsInsideOp = false;
    bool IsInsideObject = false;
    int Last = InString.Length;
    for( int Count = 0; Count < Last; Count++ )
      {
      char TestChar = InString[Count];

      if( Count > 0 )
        PreviousChar = InString[Count - 1];
      else
        PreviousChar = ' ';

      if( TestChar == Markers.Begin )
        {
        if( IsInsideOp )
          {
          IsInsideOp = false;
          SBuilder.Append( Char.ToString(
                               Markers.End ));

          }

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

      IsInsideOp = MarkOperator( SBuilder,
                                 PreviousChar,
                                 TestChar,
                                 IsInsideOp );
      }

    string Result = SBuilder.ToString();
    return Result;
    }



  private static bool MarkOperator( StringBuilder SBuilder,
                             char PreviousChar,
                             char TestChar,
                             bool IsInsideOp )
    {
    if( !IsInsideOp )
      {
      if( IsStartCharacter( TestChar ))
        {
        SBuilder.Append( Char.ToString(
                                   Markers.Begin ));
        SBuilder.Append( Char.ToString(
                         Markers.TypeOperator ));
        SBuilder.Append( Char.ToString( TestChar ));
        return true;
        }

      // It is not the start of an operator.
      SBuilder.Append( Char.ToString( TestChar ));
      return false;
      }

    // if( !IsInsideOp )
      // {
      // ShowStatus( "This can't happen with IsInsideOp." );
      // return false;
      // }

    // At this point IsInsideOp is true and it is
    // checking if this character continues a
    // two-character operation symbol.
    if( IsSecondOperatorCharacter( TestChar,
                                   PreviousChar ))
      {
      SBuilder.Append( Char.ToString( TestChar ));
      SBuilder.Append( Char.ToString( Markers.End ));
      return false;
      }

    // At this point it's not the continuation of
    // the last operator.
    SBuilder.Append( Char.ToString( Markers.End ));

    // IsInsideOp = false;

    // Is this the start of a new operator?
    if( IsStartCharacter( TestChar ))
      {
      // IsInsideOp = true;
      SBuilder.Append( Char.ToString(
                       Markers.Begin ));
      SBuilder.Append( Char.ToString(
                       Markers.TypeOperator ));
      SBuilder.Append( Char.ToString( TestChar ));
      return true;
      }

    // This is not the start of a new operator.
    SBuilder.Append( Char.ToString( TestChar ));
    return false;
    }



  private static bool IsStartCharacter( char TestChar )
    {
    // Preprocessing is not allowed here.
    if( TestChar == '#' )
      return false;

    if( TestChar == ',' )
      return true;

    if( TestChar == '+' )
      return true;

    if( TestChar == '-' )
      return true;

    if( TestChar == '*' )
      return true;

    if( TestChar == '/' )
      return true;

    if( TestChar == '%' )
      return true;

    if( TestChar == '=' )
      return true;

    if( TestChar == '>' )
      return true;

    if( TestChar == '<' )
      return true;

    if( TestChar == '.' )
      return true;

    if( TestChar == '!' )
      return true;

    if( TestChar == '~' )
      return true;

    if( TestChar == '&' )
      return true;

    if( TestChar == '|' )
      return true;

    if( TestChar == '^' )
      return true;

    if( TestChar == ':' )
      return true;

    if( TestChar == ';' )
      return true;

    if( TestChar == '(' )
      return true;

    if( TestChar == ')' )
      return true;

    if( TestChar == '[' )
      return true;

    if( TestChar == ']' )
      return true;

    return false;
    }




  private static bool IsSecondOperatorCharacter(
                                   char TestChar,
                                   char PreviousChar )
    {
    if( PreviousChar == '+' )
      {
      // ++
      if( TestChar == '+' )
        return true;

      return false;
      }

    if( PreviousChar == '-' )
      {
      // --
      if( TestChar == '-' )
        return true;

      return false;
      }

    if( PreviousChar == '=' )
      {
      // ==
      if( TestChar == '=' )
        return true;

      return false;
      }

    if( PreviousChar == '>' )
      {
      // >=
      if( TestChar == '=' )
        return true;

      // >>
      if( TestChar == '>' )
        return true;

      return false;
      }

    if( PreviousChar == '<' )
      {
      // <=
      if( TestChar == '=' )
        return true;

      // <<
      if( TestChar == '<' )
        return true;

      return false;
      }

    if( PreviousChar == '!' )
      {
      // !=
      if( TestChar == '=' )
        return true;

      return false;
      }

    if( PreviousChar == '&' )
      {
      // &&
      if( TestChar == '&' )
        return true;

      return false;
      }

    if( PreviousChar == '|' )
      {
      // ||
      if( TestChar == '|' )
        return true;

      return false;
      }

    return false;
    }



  }
}
