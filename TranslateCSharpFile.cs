// Copyright Eric Chauvin 2018.
// My blog is at:
// https://scientificmodels.blogspot.com/



using System;
using System.Text;



namespace CodeAnalysis
{
  class TranslateCSharpFile
  {
  private MainForm MForm;


  private TranslateCSharpFile()
    {
    }



  internal TranslateCSharpFile( MainForm UseForm )
    {
    MForm = UseForm;
    }



  private void ShowStatus( string ToShow )
    {
    if( MForm != null )
      MForm.ShowStatus( ToShow );

    }



  internal Token GetTokensFromFile( string FileName )
    {
    if( MForm == null )
      return null;

    string Result = SourceFile.ReadFromTextFile( MForm, FileName );
    if( Result.Length == 0 )
      {
      ShowStatus( "Nothing in Source File." );
      return null;
      }

    Result = RemoveStarComments.RemoveAllComments( MForm, Result );
    if( Result.Contains( Char.ToString(
                      Markers.ErrorPoint )))
      {
      ShowStatus( " " );
      ShowStatus( "There was an error marker after RemoveStarComments." );
      return null;
      }

    if( !MForm.CheckEvents())
      return null;

    if( !TestMarkers.TestBeginEnd( MForm, Result ))
      {
      ShowStatus( " " );
      ShowStatus( "TestBeginEnd returned false after RStComments." );
      return null;
      }


    Result = RemoveSlashComments.RemoveAllDoubleSlashComments( MForm, Result );
    if( !MForm.CheckEvents())
      return null;

    if( !TestMarkers.TestBeginEnd( MForm, Result ))
      {
      ShowStatus( " " );
      ShowStatus( "TestBeginEnd returned false after RemoveSlashComments." );
      return null;
      }

    Result = CSharpToStrings.MakeStringObjects( MForm, Result );
    if( !MForm.CheckEvents())
      return null;

    if( Result.Contains( Char.ToString(
                      Markers.ErrorPoint )))
      {
      ShowStatus( " " );
      ShowStatus( "There was an error after CSharpToStrings." );
      ShowStatus( " " );
      ShowStatus( Result );
      return null;
      }

    if( !TestMarkers.TestBeginEnd( MForm, Result ))
      {
      ShowStatus( " " );
      ShowStatus( "TestBeginEnd returned false after CSToStrings." );
      return null;
      }

    Result = CSharpToCharacters.MakeCharacterObjects( Result );
    if( !MForm.CheckEvents())
      return null;

    if( Result.Contains( Char.ToString(
                      Markers.ErrorPoint )))
      {
      ShowStatus( " " );
      ShowStatus( "There was an error marker after CSToCharacters." );
      return null;
      }

    if( !TestMarkers.TestBeginEnd( MForm, Result ))
      {
      ShowStatus( " " );
      ShowStatus( "TestBeginEnd returned false after CSToCharacters." );
      return null;
      }

    Result = CSharpToIdentifiers.MakeIdentifierObjects( MForm, Result );
    if( !MForm.CheckEvents())
      return null;

    if( !TestMarkers.TestBeginEnd( MForm, Result ))
      {
      ShowStatus( " " );
      ShowStatus( "TestBeginEnd returned false after CSToIdentifiers." );
      return null;
      }

    Result = CSharpToNumbers.MakeNumberObjects( Result );
    if( !MForm.CheckEvents())
      return null;

    if( !TestMarkers.TestBeginEnd( MForm, Result ))
      {
      ShowStatus( " " );
      ShowStatus( "TestBeginEnd returned false after CSToNumbers." );
      return null;
      }

    Result = CSharpToOperators.MakeOperatorObjects( Result );
    if( !MForm.CheckEvents())
      return null;

    if( !TestMarkers.TestBeginEnd( MForm, Result ))
      {
      ShowStatus( " " );
      ShowStatus( "TestBeginEnd returned false after CSToOperators." );
      return null;
      }

    Result = TestMarkers.RemoveOutsideWhiteSpace( Result );

    if( !TestMarkers.TestBrackets( MForm, Result ))
      {
      // ShowStatus( Result );
      return null;
      }

    IDDictionary IdentDictionary = new
                               IDDictionary( MForm );

    // CSharpFixIdentifiers CSFixIDs = new
    //                    CSharpFixIdentifiers( MForm,
    //                                IdentDictionary );

    // if( !CSFixIDs.GetIdentifiers( Result ))
      // {
      // ShowStatus( "GetIdentifiers returned false." );
      // return "";
      // }

    // Result = CSFixIDs.MakeIdentifiersLowerCase( Result );

    // IdentDictionary.ShowIDs();

    // CSFixIDs = null;
    if( !MForm.CheckEvents())
      return null;


    Result = BracketLevel.SetLevelChars( MForm, Result );
    if( Result == "" )
      {
      // ShowStatus( Result );
      return null;
      }


    Token Tk = new Token( MForm );
    Tk.AddTokensFromString( Result );
    Tk.SetLowestTokenBlocks();

    ShowStatus( " " );
    Tk.ShowTokensAtLevel( 1 );

    return Tk;
    }



  }
}
