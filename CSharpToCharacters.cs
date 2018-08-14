// Copyright Eric Chauvin 2018.
// My blog is at:
// https://scientificmodels.blogspot.com/



using System;
using System.Text;



namespace CodeAnalysis
{
  class CSharpToCharacters
  {
  private MainForm MForm;



  private CSharpToCharacters()
    {
    }



  internal CSharpToCharacters( MainForm UseForm )
    {
    MForm = UseForm;
    }



  private void ShowStatus( string ToShow )
    {
    if( MForm != null )
      MForm.ShowStatus( ToShow );

    }



  internal string MakeCharacterObjects( string InString )
    {
    StringBuilder SBuilder = new StringBuilder();

    // It could be '\''.
    InString = InString.Replace( "\\\'",
       Char.ToString( Markers.EscapedSingleQuote ));

    bool IsInsideChar = false;
    bool IsInsideObject = false;
    int Last = InString.Length;
    for( int Count = 0; Count < Last; Count++ )
      {
      char TestChar = InString[Count];

      if( IsInsideObject )
        {
        // You can't go inside another object without
        // finding the end of the character.
        if( IsInsideChar )
          {
          SBuilder.Append( Char.ToString(
                        Markers.ErrorPoint ));
          SBuilder.Append( "Character doesn't end before next object." );
          return SBuilder.ToString();
          }

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

      if( !IsInsideChar )
        {
        if( TestChar == '\'' )
          {
          IsInsideChar = true;
          SBuilder.Append( Char.ToString(
                             Markers.Begin ));
          SBuilder.Append( Char.ToString(
                          Markers.TypeChar ));
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
                               Markers.End ));
          continue;
          }
        }

      SBuilder.Append( Char.ToString( TestChar ));
      }

    string Result = SBuilder.ToString();

    // Put the single quote character back in.
    Result = Result.Replace( Char.ToString(
             Markers.EscapedSingleQuote ), "\\\'" );

    return Result;
    }




  }
}
