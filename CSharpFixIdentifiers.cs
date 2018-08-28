// Copyright Eric Chauvin 2018.
// My blog is at:
// https://scientificmodels.blogspot.com/




using System;
using System.Text;



namespace CodeAnalysis
{
  class CSharpFixIdentifiers
  {
  private MainForm MForm;
  private IDDictionary IdentDictionary;



  private CSharpFixIdentifiers()
    {
    }



  internal CSharpFixIdentifiers( MainForm UseForm,
                        IDDictionary UseIDDictionary )
    {
    MForm = UseForm;

    IdentDictionary = UseIDDictionary;
    }



  private void ShowStatus( string ToShow )
    {
    if( MForm != null )
      MForm.ShowStatus( ToShow );

    }




  internal bool GetIdentifiers( string InString )
    {
    string[] SplitS = InString.Split( new Char[] { Markers.TypeIdentifier } );

    int Last = SplitS.Length;
    // Notice that this starts at Count = 1.
    for( int Count = 1; Count < Last; Count++ )
      {
      string Line = SplitS[Count];
      if( Line == null )
        {
        ShowStatus( "The line was null at: " + Count.ToString( "N0" ) );
        return false;
        }

      if( Line.Length == 0 )
        {
        ShowStatus( "The line has length zero at: " + Count.ToString( "N0" ) );
        return false;
        }

      if( EndMarkerCount( Line ) == 0 )
        {
        ShowStatus( "This line does not have an end marker." );
        ShowStatus( ">" + Line + "<" );
        ShowStatus( "Length is: " + Line.Length.ToString( "N0" ));
        ShowStatus( InString );
        return false;
        }

      // ShowStatus( " " );
      // ShowStatus( "Original: " + Line );

      string[] SplitLine = Line.Split( new Char[] { Markers.End }, 2 );
      if( SplitLine.Length < 2 )
        {
        ShowStatus( "The SplitLine length was less than 2." );
        ShowStatus( ">" + Line + "<" );
        return false;
        }

      if( !IdentDictionary.AddIdentifier( SplitLine[0] ))
        {
        ShowStatus( "ID dictionary returned false." );
        return false;
        }

      // IDArray[Count] = SplitLine[0].ToLower();
      }

    return true;
    }



  private int EndMarkerCount( string Line )
    {
    int HowMany = 0;
    int Last = Line.Length;
    for( int Count = 0; Count < Last; Count++ )
      {
      if( Line[Count] == Markers.End )
        HowMany++;

      }

    return HowMany;
    }




  internal string MakeIdentifiersLowerCase( string InString )
    {
    StringBuilder SBuilder = new StringBuilder();

    bool IsInsideID = false;
    int Last = InString.Length;
    for( int Count = 0; Count < Last; Count++ )
      {
      char TestChar = InString[Count];

      if( TestChar == Markers.TypeIdentifier )
        {
        IsInsideID = true;
        SBuilder.Append( Char.ToString( TestChar ));
        continue;
        }

      if( TestChar == Markers.End )
        {
        IsInsideID = false;
        SBuilder.Append( Char.ToString( TestChar ));
        continue;
        }

      //  if( Markers.IsMarker( TestChar ))

      string ToChange = Char.ToString( TestChar );
      if( IsInsideID )
        ToChange = ToChange.ToLower();

      SBuilder.Append( ToChange );
      }

    string Result = SBuilder.ToString();
    return Result;
    }



  }
}
