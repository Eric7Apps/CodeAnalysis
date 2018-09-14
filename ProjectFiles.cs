// Copyright Eric Chauvin 2018.
// My blog is at:
// https://scientificmodels.blogspot.com/



using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace CodeAnalysis
{
  public class ProjectFiles
  {
  private MainForm MForm;
  // private SortedDictionary<string, string> FilesDictionary;
  // private SortedDictionary<string, string> HeaderFilesDictionary;
  private string ProjectDirectory = "C:\\Eric\\";
  // private string ProjectDirectory = "C:\\glibc\\";
  // private string ProjectDirectory = "C:\\cygwin64\\";
  // private int FilesFound = 0;
  private HeaderFiles HeaderF;




  private ProjectFiles()
    {
    }



  internal ProjectFiles( MainForm UseForm )
    {
    MForm = UseForm;

    // ProjectDirectory = UseProjectDirectory;

    // FilesDictionary = new SortedDictionary<string, string>();
    HeaderF = new HeaderFiles( MForm );
    }



  private void ShowStatus( string ToShow )
    {
    if( MForm != null )
      MForm.ShowStatus( ToShow );

    }



  internal void FindProjectFiles()
    {
    try
    {
    // FilesDictionary.Clear();
    // FilesFound = 0;

    // This gets called recursively:
    SearchOneDirectory( ProjectDirectory );

    ShowStatus( " " );
    ShowStatus( "Finished searching files." );

    // HeaderF.ShowFiles();

    // ShowStatus( "HeaderFilesFound: " + HeaderFilesFound.ToString( "N0" ));
    }
    catch( Exception Except )
      {
      ShowStatus( "Exception in FindProjectFiles():" );
      ShowStatus( Except.Message );
      }
    }



  internal bool SearchOneDirectory( string DirName )
    {
    try
    {
    if( DirName.ToLower().Contains( "\\obj" ))
      return true;

    if( DirName.ToLower().Contains( "\\.vs" ))
      return true;

    if( DirName.ToLower().Contains( "\\bin" ))
      return true;

    if( DirName.ToLower().Contains( "\\properties" ))
      return true;

    ShowStatus( " " );
    ShowStatus( "Getting files in: " + DirName );

    string[] FileEntries = Directory.GetFiles( DirName, "*.*" );
    foreach( string FileName in FileEntries )
      {
      if( !MForm.CheckEvents())
        return false;

      if( !ProcessOneFile( FileName ))
        return false;

      // FilesFound++;
      // if( FilesFound > 1000 )
        // return;

      }

    string [] SubDirEntries = Directory.GetDirectories( DirName );
    foreach( string SubDir in SubDirEntries )
      {
      if( !MForm.CheckEvents())
        return false;

      // Call itself recursively.
      if( !SearchOneDirectory( SubDir ))
        return false;

      }

    return true;
    }
    catch( Exception Except )
      {
      ShowStatus( "Exception in SearchOneDirectory():" );
      ShowStatus( Except.Message );
      return false;
      }
    }



  private bool ProcessOneFile( string FileName )
    {
    // if( IsKnownExtension( FileName ))
      // return;

    string FileNameLower = FileName.ToLower();
    if( FileNameLower.Contains( ".designer." ))
      return true;

    if( FileNameLower.Contains( "blankfile.cs" ))
      return true;

    // I think .hh files are only used in libcc1,
    // which I think is related to the GDB debugger.
    if( FileNameLower.EndsWith( ".cs" ))
    // if( FileNameLower.EndsWith( ".h" )) // ||
    //    FileNameLower.EndsWith( ".hh" ))
      {
      // string ToFile = Path.GetFileName( FileName );
      // ToFile = "\\Eric\\TestGcc\\include\\" + ToFile;

      // Compare the bytes in each file if ToFile
      // exists, and see if they are the same.
      // File.ReadAllBytes()

      ShowStatus( FileName );

      TranslateCSharpFile TranslateCS = new
                       TranslateCSharpFile( MForm );

      string FileS = TranslateCS.TranslateFile( FileName );
      if( FileS == "" )
        {
        ShowStatus( " " );
        ShowStatus( "TranslateCS returned empty string for: " + FileName );
        return false;
        }
      // File.Copy( FileName, ToFile, true );
      // ShowStatus( ToFile );
      HeaderF.AddFile( FileName );
      }

    return true;
    }




  private bool IsKnownExtension( string FileName )
    {
    string FileNameLower = FileName.ToLower();

    if( !FileNameLower.Contains( "." ))
        return true;

    if( FileNameLower.Contains( "changelog" ))
        return true;

    if( FileNameLower.Contains( "\\readme." ))
        return true;


    if( FileNameLower.EndsWith( ".h" ))
      return true;

    if( FileNameLower.EndsWith( ".hh" ))
      return true;

    if( FileNameLower.EndsWith( ".c" ))
      return true;

    if( FileNameLower.EndsWith( ".cc" ))
      return true;

    if( FileNameLower.EndsWith( ".lib" ))
      return true;

    if( FileNameLower.EndsWith( ".awk" ))
      return true;

    // Shell scripts:
    if( FileNameLower.EndsWith( ".sh" ))
      return true;

    // Make files:
    if( FileNameLower.EndsWith( ".mk" ))
      return true;

    // Python files:
    if( FileNameLower.EndsWith( ".py" ))
      return true;


    // Makefile definitions
    if( FileNameLower.EndsWith( ".def" ))
      return true;

    if( FileNameLower.EndsWith( ".in" ))
      return true;

    if( FileNameLower.EndsWith( ".tpl" ))
      return true;


    // https://en.wikipedia.org/wiki/M4_(computer_language)
    // https://en.wikipedia.org/wiki/Autoconf
    // Macro processor files:
    if( FileNameLower.EndsWith( ".m4" ))
      return true;

    if( FileNameLower.EndsWith( ".gitattributes" ))
      return true;

    if( FileNameLower.EndsWith( ".gitignore" ))
      return true;

    // Configuration identification script.
    if( FileNameLower.EndsWith( ".guess" ))
      return true;

    if( FileNameLower.EndsWith( ".rpath" ))
      return true;

    if( FileNameLower.EndsWith( ".sub" ))
      return true;

    if( FileNameLower.EndsWith( ".ac" ))
      return true;

    if( FileNameLower.EndsWith( ".runtime" ))
      return true;

    if( FileNameLower.EndsWith( ".el" ))
      return true;

    if( FileNameLower.EndsWith( ".x" ))
      return true;

    if( FileNameLower.EndsWith( ".md" ))
      return true;

    if( FileNameLower.EndsWith( ".opt" ))
      return true;

    if( FileNameLower.EndsWith( ".build" ))
      return true;

    if( FileNameLower.EndsWith( "config.gcc" ))
      return true;

    if( FileNameLower.EndsWith( "config.host" ))
      return true;


    if( FileNameLower.EndsWith( ".l" ))
      return true;

    if( FileNameLower.EndsWith( ".pd" ))
      return true;

    if( FileNameLower.EndsWith( ".m" ))
      return true;

    if( FileNameLower.EndsWith( ".texi" ))
      return true;

    if( FileNameLower.EndsWith( ".gperf" ))
      return true;

    return false;
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




  }
}
