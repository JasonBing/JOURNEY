
//------------------------------------------------------------------------------
// 
//     This code was generated by a SAP. NET Connector Proxy Generator Version 2.0
//     Created at 2006-12-13
//     Created from Windows
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// 
//------------------------------------------------------------------------------
using System;
//using System.Text;
//using System.Collections;
//using System.Runtime.InteropServices;
using System.Xml.Serialization;
//using System.Web.Services;
//using System.Web.Services.Description;
//using System.Web.Services.Protocols;
using SAP.Connector;

namespace RFCProxyBuilder
{
  /// <summary>
  /// A typed collection of RFC_FLDS_U elements.
  /// </summary>
  [Serializable]
  public class RFC_FLDS_UTable : SAPTable 
  {
  
    /// <summary>
    /// Returns the element type RFC_FLDS_U.
    /// </summary>
    /// <returns>The type RFC_FLDS_U.</returns>
    public override Type GetElementType() 
    {
        return (typeof(RFC_FLDS_U));
    }

    /// <summary>
    /// Creates an empty new row of type RFC_FLDS_U.
    /// </summary>
    /// <returns>The newRFC_FLDS_U.</returns>
    public override object CreateNewRow()
    { 
        return new RFC_FLDS_U();
    }
     
    /// <summary>
    /// The indexer of the collection.
    /// </summary>
    public RFC_FLDS_U this[int index] 
    {
        get 
        {
            return ((RFC_FLDS_U)(List[index]));
        }
        set 
        {
            List[index] = value;
        }
    }
        
    /// <summary>
    /// Adds a RFC_FLDS_U to the end of the collection.
    /// </summary>
    /// <param name="value">The RFC_FLDS_U to be added to the end of the collection.</param>
    /// <returns>The index of the newRFC_FLDS_U.</returns>
    public int Add(RFC_FLDS_U value) 
    {
        return List.Add(value);
    }
        
    /// <summary>
    /// Inserts a RFC_FLDS_U into the collection at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index at which value should be inserted.</param>
    /// <param name="value">The RFC_FLDS_U to insert.</param>
    public void Insert(int index, RFC_FLDS_U value) 
    {
        List.Insert(index, value);
    }
        
    /// <summary>
    /// Searches for the specified RFC_FLDS_U and returnes the zero-based index of the first occurrence in the collection.
    /// </summary>
    /// <param name="value">The RFC_FLDS_U to locate in the collection.</param>
    /// <returns>The index of the object found or -1.</returns>
    public int IndexOf(RFC_FLDS_U value) 
    {
        return List.IndexOf(value);
    }
        
    /// <summary>
    /// Determines wheter an element is in the collection.
    /// </summary>
    /// <param name="value">The RFC_FLDS_U to locate in the collection.</param>
    /// <returns>True if found; else false.</returns>
    public bool Contains(RFC_FLDS_U value) 
    {
        return List.Contains(value);
    }
        
    /// <summary>
    /// Removes the first occurrence of the specified RFC_FLDS_U from the collection.
    /// </summary>
    /// <param name="value">The RFC_FLDS_U to remove from the collection.</param>
    public void Remove(RFC_FLDS_U value) 
    {
        List.Remove(value);
    }

    /// <summary>
    /// Copies the contents of the RFC_FLDS_UTable to the specified one-dimensional array starting at the specified index in the target array.
    /// </summary>
    /// <param name="array">The one-dimensional destination array.</param>           
    /// <param name="index">The zero-based index in array at which copying begins.</param>           
    public void CopyTo(RFC_FLDS_U[] array, int index) 
    {
        List.CopyTo(array, index);
	}
  }
}
