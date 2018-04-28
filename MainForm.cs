// Copyright Eric Chauvin 2018.
// My blog is at:
// ericsourcecode.blogspot.com


// Code Analysis


// Do math as a language.  With a language parser
// and all.  It supercedes the older form of the
// language of mathematics.


using System;
// using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
// using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;




namespace CodeAnalysis
{
  public partial class MainForm : Form
  {
  internal const string VersionDate = "4/28/2018";
  internal const int VersionNumber = 09; // 0.9
  // private System.Threading.Mutex SingleInstanceMutex = null;
  // private bool IsSingleInstance = false;
  private bool IsClosing = false;
  private bool Cancelled = false;
  internal const string MessageBoxTitle = "Code Analysis";
  private string DataDirectory = "";
  // private ConfigureFile ConfigFile;


  public MainForm()
    {
    InitializeComponent();

    SetupDirectories();

    this.Font = new System.Drawing.Font( "Consolas", 34.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
    this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
    }




  private void SetupDirectories()
    {
    try
    {
    DataDirectory = Application.StartupPath + "\\Data\\";
    if( !Directory.Exists( DataDirectory ))
      Directory.CreateDirectory( DataDirectory );

    }
    catch( Exception )
      {
      MessageBox.Show( "Error: The directory could not be created.", MessageBoxTitle, MessageBoxButtons.OK);
      return;
      }
    }



  internal bool CheckEvents()
    {
    if( IsClosing )
      return false;

    Application.DoEvents();

    if( Cancelled )
      return false;

    return true;
    }



  internal bool GetIsClosing()
    {
    return IsClosing;
    }



  internal void ShowStatus(string Status)
    {
    if (IsClosing)
      return;

    MainTextBox.AppendText( Status + "\r\n" );
    }



  private void exitToolStripMenuItem_Click(object sender, EventArgs e)
    {
    Close();
    }



  private void lexicalAnalysisToolStripMenuItem_Click(object sender, EventArgs e)
    {
    try
    {
    /*
    OpenFileDialog1.Title = "Code Analysis";
    OpenFileDialog1.InitialDirectory = "C:\\Eric\\"; // DataDirectory;
    OpenFileDialog1.Filter = "All files (*.*)|*.*";

    if( OpenFileDialog1.ShowDialog() != DialogResult.OK )
      return;

    // Path.GetFileName( OpenFileDialog1.FileName );
    string FileName = OpenFileDialog1.FileName;


    // string FileName = GccPath + "toplev.c";
    // string FileName = GccPath + "toplev.h";

    // A shell script: config.gcc
    // string FileName = GccPath + "config.in";
    // #include "config.h"
    */


    // string FileName = "C:\\JavaSource\\javax\\swing\\JFrame.java";
    // ProcessOneJavaFile( FileName );

    // ProjectFiles ProjFiles = new ProjectFiles( this );
    // ProjFiles.FindProjectFiles();

    // ProjFiles.CopyDirectoryTree( "C:\\JavaSource",
    //                             "C:\\JavaSource",
    //                             "C:\\JavaSourceOut" );
    // ShowStatus( " " );
    // ShowStatus( "Finished making directories." );


    // That configure file that is set up from a
    // script:
    // string FileName =
    //      "C:\\GccOriginal\\fixincludes\\config.h.in";

    // string FileName =
    //      "C:\\GccOriginal\\gcc\\main.c";

    string FileName =
         "C:\\GccOriginal\\gcc\\toplev.c";


    SourceFile SFile = new SourceFile( this, FileName );
    // SFile.ShowDingbatCharacters();
    SFile.DoPreprocessing();
    }
    catch( Exception Except )
      {
      ShowStatus( "Exception in lexicalAnalysisToolStripMenuItem_Click()" );
      ShowStatus( Except.Message );
      }
    }




  private void ProcessOneJavaFile( string FileName )
    {
    try
    {
    LexAnalysis1 Lex1 = new LexAnalysis1( this, FileName );
    Lex1.ProcessSyntax();
    Lex1.ProcessComments();

    }
    catch( Exception Except )
      {
      ShowStatus( "Exception in ProcessOneFile():" );
      ShowStatus( Except.Message );
      }
    }



  private void MainForm_KeyDown(object sender, KeyEventArgs e)
    {
    if( e.KeyCode == Keys.Escape ) //  && (e.Alt || e.Control || e.Shift))
      {
      ShowStatus( "Cancelled." );
      Cancelled = true;
      }
    }




  }
}
