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
  // private string ProjectDirectory = "C:\\GccOriginal\\";
  // private string ProjectDirectory = "C:\\glibc\\";
  private string ProjectDirectory = "C:\\cygwin64\\";
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
    // FilesDictionary.Clear();
    // FilesFound = 0;

    // This gets called recursively:
    SearchOneDirectory( ProjectDirectory );

    ShowStatus( " " );
    ShowStatus( "Finished searching files." );

    HeaderF.ShowFiles();

    // ShowStatus( "HeaderFilesFound: " + HeaderFilesFound.ToString( "N0" ));
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
      if( !MForm.CheckEvents())
        return;

      ProcessOneFile( FileName );

      // FilesFound++;
      // if( FilesFound > 1000 )
        // return;

      }

    string [] SubDirEntries = Directory.GetDirectories( DirName );
    foreach( string SubDir in SubDirEntries )
      {
      if( !MForm.CheckEvents())
        return;

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



  private void ProcessOneFile( string FileName )
    {
    // if( FileNameLower.StartsWith(
    //           "c:\\gccoriginal\\libstdc++-v3\\" ))
    //    return true;

    // if( IsUnusedDirectory( FileName ))
      // return;

    // if( IsKnownExtension( FileName ))
      // return;

    string FileNameLower = FileName.ToLower();
    // if( !FileNameLower.EndsWith( "stdio.h" ))
      // return;

    // Set up locked keys or something.
    // Appending: stdio.h:
    // c:\gccoriginal\glibc\include\stdio.h


    // ShowStatus( FileNameLower );

    // I think .hh files are only used in libcc1,
    // which I think is related to the GDB debugger.
    if( FileNameLower.EndsWith( ".h" )) // ||
    //    FileNameLower.EndsWith( ".hh" ))
      {
      // string ToFile = Path.GetFileName( FileName );
      // ToFile = "\\Eric\\TestGcc\\include\\" + ToFile;

      // Compare the bytes in each file if ToFile
      // exists, and see if they are the same.
      // File.ReadAllBytes()

      // File.Copy( FileName, ToFile, true );
      // ShowStatus( ToFile );
      HeaderF.AddFile( FileName );
      }
    }



  private bool IsUnusedDirectory( string FileName )
    {
    string FileNameLower = FileName.ToLower();

    if( FileNameLower.StartsWith(
             "c:\\gccoriginal\\config\\" ))
        return true;

    if( FileNameLower.StartsWith(
             "c:\\gccoriginal\\contrib\\" ))
        return true;

    if( FileNameLower.StartsWith(
               "c:\\gccoriginal\\gcc\\ada\\" ))
        return true;

    if( FileNameLower.StartsWith(
               "c:\\gccoriginal\\gcc\\testsuite\\" ))
        return true;

    if( FileNameLower.StartsWith(
               "c:\\gccoriginal\\gcc\\doc\\" ))
        return true;

    if( FileNameLower.StartsWith(
               "c:\\gccoriginal\\gcc\\config\\" ))
        return true;

    // Drupal Portable Object?
    if( FileNameLower.StartsWith(
               "c:\\gccoriginal\\gcc\\po\\" ))
        return true;

    // Just In Time Compiler library.
    if( FileNameLower.StartsWith(
               "c:\\gccoriginal\\gcc\\jit\\" ))
        return true;

    if( FileNameLower.StartsWith(
               "c:\\gccoriginal\\gotools" ))
        return true;

    if( FileNameLower.StartsWith(
               "c:\\gccoriginal\\intl\\" ))
        return true;

    if( FileNameLower.StartsWith(
               "c:\\gccoriginal\\libatomic\\" ))
        return true;

    // I think this is related to GDB, the Gnu
    // debugger.
    if( FileNameLower.StartsWith(
               "c:\\gccoriginal\\libcc1\\" ))
        return true;

    if( FileNameLower.StartsWith(
               "c:\\gccoriginal\\libcpp\\" ))
        return true;

    if( FileNameLower.StartsWith(
               "c:\\gccoriginal\\libffi\\" ))
        return true;

    if( FileNameLower.StartsWith(
               "c:\\gccoriginal\\libgcc\\" ))
        return true;

    if( FileNameLower.StartsWith(
               "c:\\gccoriginal\\libgo\\" ))
        return true;

    if( FileNameLower.StartsWith(
               "c:\\gccoriginal\\libgfortran\\" ))
        return true;

    if( FileNameLower.StartsWith(
               "c:\\gccoriginal\\libgomp\\" ))
        return true;

    if( FileNameLower.StartsWith(
               "c:\\gccoriginal\\libbacktrace\\" ))
        return true;

    if( FileNameLower.StartsWith(
               "c:\\gccoriginal\\libhsail-rt\\" ))
        return true;

    if( FileNameLower.StartsWith(
               "c:\\gccoriginal\\libiberty\\" ))
        return true;

    if( FileNameLower.StartsWith(
               "c:\\gccoriginal\\libitm\\" ))
        return true;

    if( FileNameLower.StartsWith(
               "c:\\gccoriginal\\libmpx\\" ))
        return true;

    if( FileNameLower.StartsWith(
               "c:\\gccoriginal\\liboffloadmic\\" ))
        return true;

    if( FileNameLower.StartsWith(
               "c:\\gccoriginal\\libsanitizer\\" ))
        return true;

    if( FileNameLower.StartsWith(
               "c:\\gccoriginal\\libstdc++-v3\\" ))
        return true;

    if( FileNameLower.StartsWith(
               "c:\\gccoriginal\\libquadmath\\" ))
        return true;

    if( FileNameLower.StartsWith(
               "c:\\gccoriginal\\libssp\\" ))
        return true;

    if( FileNameLower.StartsWith(
               "c:\\gccoriginal\\libvtv\\" ))
        return true;

    if( FileNameLower.StartsWith(
               "c:\\gccoriginal\\zlib\\" ))
        return true;

    if( FileNameLower.StartsWith(
               "c:\\gccoriginal\\lto-plugin\\" ))
        return true;


    return false;
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
