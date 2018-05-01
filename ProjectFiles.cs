// Copyright Eric Chauvin 2018.
// My blog is at:
// ericsourcecode.blogspot.com


// That configure file that is set up from a script:
// C:\GccOriginal\fixincludes\config.h.in


// https://en.wikipedia.org/wiki/C_preprocessor


using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace CodeAnalysis
{
  public class ProjectFiles
  {
  private MainForm MForm;
  private SortedDictionary<string, string> FilesDictionary;
  private SortedDictionary<string, string> HeaderFilesDictionary;
  private string ProjectDirectory = "C:\\GccOriginal\\";
  private int FilesFound = 0;
  private int HeaderFilesFound = 0;



  private ProjectFiles()
    {
    }



  internal ProjectFiles( MainForm UseForm )
    {
    MForm = UseForm;

    // ProjectDirectory = UseProjectDirectory;

    FilesDictionary = new SortedDictionary<string, string>();
    HeaderFilesDictionary = new SortedDictionary<string, string>();
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



  internal void FindProjectFiles()
    {
    try
    {
    FilesDictionary.Clear();
    FilesFound = 0;
    HeaderFilesDictionary.Clear();
    HeaderFilesFound = 0;

    // This gets called recursively:
    SearchOneDirectory( ProjectDirectory );

    ShowStatus( " " );
    ShowStatus( "HeaderFilesFound: " + HeaderFilesFound.ToString( "N0" ));
    }
    catch( Exception Except )
      {
      ShowStatus( "Exception in FindProjectFiles():" );
      ShowStatus( Except.Message );
      }
    }



  internal void SearchOneDirectory( string DirName )
    {
    try
    {
    // ShowStatus( " " );
    // ShowStatus( "Getting files in: " + DirName );
    string[] FileEntries = Directory.GetFiles( DirName, "*.*" );
    foreach( string FileName in FileEntries )
      {
      string FileNameLower = FileName.ToLower();
      // Make files:
      if( FileNameLower.EndsWith( ".mk" ))
        continue;

// If I exclude every known file extension, then
// what's left?

      // https://en.wikipedia.org/wiki/M4_(computer_language)
      // https://en.wikipedia.org/wiki/Autoconf
      // Macro processor files:
      if( FileNameLower.EndsWith( ".m4" ))
        continue;

      // Python files:
      if( FileNameLower.EndsWith( ".py" ))
        continue;

      // Shell scripts:
      if( FileNameLower.EndsWith( ".sh" ))
        continue;

      if( FileNameLower.EndsWith( ".h" ))
        {
        ShowStatus( FileName );
        HeaderFilesDictionary[FileNameLower] = "";
        HeaderFilesFound++;
        continue;
        }

      // string ShortName = FileName.Replace( MForm.GetWebPagesDirectory(), "" );
      // ShowStatus( FileName );

      FilesFound++;
      // if( FilesFound > 1000 )
      //  return;

      // ShortName = ShortName.ToLower();

      if( !MForm.CheckEvents())
        return;

      }

    string [] SubDirEntries = Directory.GetDirectories( DirName );
    foreach( string SubDir in SubDirEntries )
      {
      if( !MForm.CheckEvents())
        return;

      if( SubDir.StartsWith( ProjectDirectory + "config" ))
        continue;

      if( SubDir.StartsWith( ProjectDirectory + "contrib" ))
        continue;

      if( SubDir.StartsWith( ProjectDirectory + "libstdc++-v3\\testsuite\\" ))
        continue;

      // Call itself recursively.
      SearchOneDirectory( SubDir );
      }
    }
    catch( Exception Except )
      {
      ShowStatus( "Exception in SearchOneDirectory():" );
      ShowStatus( Except.Message );
      }
    }




  internal void CopyDirectoryTree( string FromDir,
                          string FromDirBaseString,
                          string ToDirBaseString )
    {
    try
    {
    if( !Directory.Exists( ToDirBaseString ))
      {
      Directory.CreateDirectory( ToDirBaseString );
      ShowStatus( ToDirBaseString );
      }

    string [] SubDirEntries = Directory.GetDirectories( FromDir );
    foreach( string SubDir in SubDirEntries )
      {
      if( !MForm.CheckEvents())
        return;

      string NewDir = SubDir.Replace(
                  FromDirBaseString, ToDirBaseString );

      if( !Directory.Exists( NewDir ))
        Directory.CreateDirectory( NewDir );

      ShowStatus( NewDir );

      // Call itself recursively.
      CopyDirectoryTree( SubDir, FromDirBaseString,
                                 ToDirBaseString );

      }
    }
    catch( Exception Except )
      {
      ShowStatus( "Exception in SearchOneDirectory():" );
      ShowStatus( Except.Message );
      }
    }




/*
Mark the preprocessed lines:
.Mark = true
.Mark = false



        string[] SplitString = Line.Split( new Char[] { '\t' } );

        if( SplitString.Length < 2 )
          continue;

        string KeyWord = SplitString[0].Trim();
        string Value = SplitString[1].Trim();
        KeyWord = KeyWord.Replace( "\"", "" );
        Value = Value.Replace( "\"", "" );

        if( KeyWord == "" )
          continue;

        CDictionary[KeyWord] = Value;
        // try
        // CDictionary.Add( KeyWord, Value );




    KeyWord = KeyWord.ToLower().Trim();

    string Value;
    if( CDictionary.TryGetValue( KeyWord, out Value ))
      return Value;
    else
      return "";



=====
    KeyWord = KeyWord.ToLower().Trim();

    if( KeyWord == "" )
      {
      MForm.ShowStatus( "Can't add an empty keyword to the dictionary in ConfigureFile.cs." );
      return;
      }

    CDictionary[KeyWord] = Value;



      foreach( KeyValuePair<string, string> Kvp in CDictionary )
        {
        string Line = Kvp.Key + "\t" + Kvp.Value;


*/


  }
}
