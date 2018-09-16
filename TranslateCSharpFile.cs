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



  internal string TranslateFile( string FileName )
    {
    if( MForm == null )
      return "";

    SourceFile SFile = new SourceFile( MForm );
    string Result = SFile.ReadFromTextFile( FileName );
    SFile = null; // It should be freed.

    if( Result.Length == 0 )
      {
      ShowStatus( "Nothing in Source File." );
      return "";
      }

    TestMarkers TMarkers = new TestMarkers( MForm );

    RemoveStarComments RStComments = new
                        RemoveStarComments( MForm );

    Result = RStComments.RemoveAllComments( Result );
    RStComments = null;

    // ShowStatus( Result );

    if( Result.Contains( Char.ToString(
                      Markers.ErrorPoint )))
      {
      ShowStatus( " " );
      ShowStatus( "There was an error marker after RSComments." );
      return "";
      }

    if( !MForm.CheckEvents())
      return "";

    if( !TMarkers.TestBeginEnd( Result ))
      {
      ShowStatus( " " );
      ShowStatus( "TestBeginEnd returned false after RStComments." );
      return "";
      }


    RemoveSlashComments RemoveSlComments = new
                        RemoveSlashComments( MForm );

    Result = RemoveSlComments.RemoveAllDoubleSlashComments( Result );
    RemoveSlComments = null;

    if( !MForm.CheckEvents())
      return "";

    if( !TMarkers.TestBeginEnd( Result ))
      {
      ShowStatus( " " );
      ShowStatus( "TestBeginEnd returned false after RemoveSlComments." );
      return "";
      }

    CSharpToStrings CSToStrings = new
                              CSharpToStrings( MForm );

    Result = CSToStrings.MakeStringObjects( Result );
    CSToStrings = null; // It should be freed.
    if( !MForm.CheckEvents())
      return "";

    if( Result.Contains( Char.ToString(
                      Markers.ErrorPoint )))
      {
      ShowStatus( " " );
      ShowStatus( "There was an error after CSToStrings." );
      ShowStatus( " " );
      ShowStatus( Result );
      return "";
      }

    if( !TMarkers.TestBeginEnd( Result ))
      {
      ShowStatus( " " );
      ShowStatus( "TestBeginEnd returned false after CSToStrings." );
      return "";
      }

    CSharpToCharacters CSToCharacters = new
                              CSharpToCharacters( MForm );

    Result = CSToCharacters.MakeCharacterObjects( Result );
    CSToCharacters = null; // It should be freed.
    if( !MForm.CheckEvents())
      return "";

    if( Result.Contains( Char.ToString(
                      Markers.ErrorPoint )))
      {
      ShowStatus( " " );
      ShowStatus( "There was an error marker after CSToCharacters." );
      return "";
      }

    if( !TMarkers.TestBeginEnd( Result ))
      {
      ShowStatus( " " );
      ShowStatus( "TestBeginEnd returned false after CSToCharacters." );
      return "";
      }

    CSharpToIdentifiers CSToIdentifiers = new
                          CSharpToIdentifiers( MForm );

    Result = CSToIdentifiers.MakeIdentifierObjects( Result );
    CSToIdentifiers = null;
    if( !MForm.CheckEvents())
      return "";

    if( !TMarkers.TestBeginEnd( Result ))
      {
      ShowStatus( " " );
      ShowStatus( "TestBeginEnd returned false after CSToIdentifiers." );
      return "";
      }

    CSharpToNumbers CSToNumbers = new
                              CSharpToNumbers( MForm );

    Result = CSToNumbers.MakeNumberObjects( Result );
    CSToNumbers = null;
    if( !MForm.CheckEvents())
      return "";

    if( !TMarkers.TestBeginEnd( Result ))
      {
      ShowStatus( " " );
      ShowStatus( "TestBeginEnd returned false after CSToNumbers." );
      return "";
      }

    CSharpToOperators CSToOperators = new
                           CSharpToOperators( MForm );
    Result = CSToOperators.MakeOperatorObjects( Result );
    CSToOperators = null;
    if( !MForm.CheckEvents())
      return "";

    if( !TMarkers.TestBeginEnd( Result ))
      {
      ShowStatus( " " );
      ShowStatus( "TestBeginEnd returned false after CSToOperators." );
      return "";
      }

    Result = TMarkers.RemoveOutsideWhiteSpace( Result );

    if( !TMarkers.TestBrackets( Result ))
      {
      // ShowStatus( Result );
      return "";
      }

    IDDictionary IdentDictionary = new
                               IDDictionary( MForm );

    CSharpFixIdentifiers CSFixIDs = new
                        CSharpFixIdentifiers( MForm,
                                    IdentDictionary );

    // if( !CSFixIDs.GetIdentifiers( Result ))
      // {
      // ShowStatus( "GetIdentifiers returned false." );
      // return "";
      // }

    Result = CSFixIDs.MakeIdentifiersLowerCase( Result );

    // IdentDictionary.ShowIDs();

    CSFixIDs = null;
    if( !MForm.CheckEvents())
      return "";


    BracketLevel BracketLev = new BracketLevel( MForm );
    Result = BracketLev.SetLevelChars( Result );
    BracketLev = null;

    if( Result == "" )
      {
      // ShowStatus( Result );
      return "";
      }


    Token Tk = new Token( MForm );
    Tk.AddTokensFromString( Result );
    Tk.SetLowestTokenBlocks();

    ShowStatus( " " );
    Tk.ShowTokensAtLevel( 1 );

    // return "";

    // ShowStatus( Result );
    ShowStatus( " " );
    ShowStatus( "That's it." );
    ShowStatus( " " );
    ShowStatus( " " );
    return Result;
    }



  }
}
