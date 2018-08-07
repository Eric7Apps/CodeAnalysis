// Copyright Eric Chauvin 2018.
// My blog is at:
// https://scientificmodels.blogspot.com/




using System;
using System.Collections.Generic;
using System.Text;
using System.IO;



namespace CodeAnalysis
{
  class HeaderFiles
  {
  private MainForm MForm;
  private SortedDictionary<string, HeaderRec> HeaderDictionary;


  public struct HeaderRec
    {
    public StringArray FileNameArray;
    }


  private HeaderFiles()
    {
    }



  internal HeaderFiles( MainForm UseForm )
    {
    MForm = UseForm;

    HeaderDictionary = new SortedDictionary<string, HeaderRec>();
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



  internal void AddFile( string FileName )
    {
    // Path.GetDirectoryName(String)
    // Path.GetExtension(String)
    // Path.ChangeExtension(String, String)

    FileName = FileName.ToLower().Trim();

    string Key = Path.GetFileName( FileName );
    if( !HeaderDictionary.ContainsKey( Key ))
      {
      // ShowStatus( Key );
      HeaderRec Rec = new HeaderRec();
      Rec.FileNameArray = new StringArray( MForm );
      Rec.FileNameArray.
                    AppendStringToArray( FileName );

      HeaderDictionary[Key] = Rec;
      return;
      }

    if( HeaderDictionary[Key].FileNameArray.
                   StringIsInArray( FileName ))
      {
      // ShowStatus( "The filename is already in: " + Key );
      return;
      }

    // ShowStatus( "Appending: " + Key + ": " + FileName );

    HeaderDictionary[Key].FileNameArray.
                    AppendStringToArray( FileName );

    }



  internal void ShowFiles()
    {
    ShowStatus( " " );
    ShowStatus( " " );
    ShowStatus( " " );
    ShowStatus( " " );
    ShowStatus( " " );
    ShowStatus( " " );

    foreach( KeyValuePair<string, HeaderRec> Kvp in HeaderDictionary )
      {
      if( !MForm.CheckEvents())
        return;

      HeaderRec Rec = Kvp.Value;
      int Last = Rec.FileNameArray.GetLast();
      // if( Last < 2 )
        // continue;

      string ShowS = Rec.FileNameArray.
                            GetArrayAsString( "; " );

      ShowStatus( Kvp.Key + ":  " + ShowS );
      }

    ShowStatus( " " );
    ShowStatus( "Done showing files." );
    }



  }
}
