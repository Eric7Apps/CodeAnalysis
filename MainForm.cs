// Copyright Eric Chauvin 2018.
// My blog is at:
// https://scientificmodels.blogspot.com/



// Code Analysis



using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;




namespace CodeAnalysis
{
  public partial class MainForm : Form
  {
  internal const string VersionDate = "9/16/2018";
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
    */


    ProjectFiles ProjFiles = new ProjectFiles( this );

    // ShowStatus( "Starting FindProjectFiles." );
    ProjFiles.FindProjectFiles();

    // ShowUnicodeCharacters();

    // ProjFiles.CopyDirectoryTree( "C:\\GccOriginal\\",
    //                             "C:\\GccOriginal\\",
    //                             "C:\\GccOut\\" );
    // ShowStatus( " " );
    // ShowStatus( "Finished making directories." );


    /*
    string FileName =
              "C:\\Eric\\ClimateModel\\MainForm.cs";

    TranslateCSharpFile( FileName );
    */
    }
    catch( Exception Except )
      {
      ShowStatus( "Exception in lexicalAnalysisToolStripMenuItem_Click()" );
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



  internal void ShowUnicodeCharacters()
    {
    for( int Count = 0x2200; Count <= 0x22FF; Count++ )
      {
      char ShowChar = (char)Count;
      ShowStatus( Count.ToString( "X4" ) + ": " + Char.ToString( ShowChar ));
      }

      // Basic Multilingual Plane
      // C0 Controls and Basic Latin (Basic Latin)
      //                 (0000007F)
      // C1 Controls and Latin-1 Supplement (008000FF)
      // Latin Extended-A (0100017F)
      // Latin Extended-B (0180024F)
      // IPA Extensions (025002AF)
      // Spacing Modifier Letters (02B002FF)
      // Combining Diacritical Marks (0300036F)
      // General Punctuation (2000206F)
      // Superscripts and Subscripts (2070209F)
      // Currency Symbols (20A020CF)
      // Combining Diacritical Marks for Symbols (20D020FF)
      // Letterlike Symbols (2100214F)
      // Number Forms (2150218F)
      // Arrows (219021FF)
      // Mathematical Operators (220022FF)
      // Box Drawing (2500257F)
      // Geometric Shapes (25A025FF)
      // Miscellaneous Symbols (260026FF)
      // Dingbats (270027BF)
      // Miscellaneous Symbols and Arrows (2B002BFF)
      // Control characters.
    }

      

  }
}
