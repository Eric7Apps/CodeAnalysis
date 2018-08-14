// Copyright Eric Chauvin 2018.
// My blog is at:
// https://scientificmodels.blogspot.com/



using System;
using System.Text;



namespace CodeAnalysis
{
  class CSharpToIdentifiers
  {
  private MainForm MForm;


  private CSharpToIdentifiers()
    {
    }



  internal CSharpToIdentifiers( MainForm UseForm )
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



  internal string MakeIdentifierObjects( string InString )
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
        if( TestChar == Markers.End )
          IsInsideObject = false;

        SBuilder.Append( Char.ToString( TestChar ));
        continue;
        }

      if( TestChar == Markers.Begin )
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
                             Markers.Begin ));
          SBuilder.Append( Char.ToString(
                           Markers.TypeIdentifier ));
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



  }
}
