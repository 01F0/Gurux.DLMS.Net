//
// --------------------------------------------------------------------------
//  Gurux Ltd
// 
//
//
// Filename:        $HeadURL$
//
// Version:         $Revision$,
//                  $Date$
//                  $Author$
//
// Copyright (c) Gurux Ltd
//
//---------------------------------------------------------------------------
//
//  DESCRIPTION
//
// This file is a part of Gurux Device Framework.
//
// Gurux Device Framework is Open Source software; you can redistribute it
// and/or modify it under the terms of the GNU General Public License 
// as published by the Free Software Foundation; version 2 of the License.
// Gurux Device Framework is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of 
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details.
//
// More information of Gurux products: http://www.gurux.org
//
// This code is licensed under the GNU General Public License v2. 
// Full text may be retrieved at http://www.gnu.org/licenses/gpl-2.0.txt
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gurux.DLMS.Objects
{
    public class GXApplicationContextName
    {
        public string LogicalName
        {
            get;
            internal set;
        }
        public byte JointIsoCtt
        {
            get;
            internal set;
        }
        public byte Country
        {
            get;
            internal set;
        }
        public UInt16 CountryName
        {
            get;
            internal set;
        }
        public byte IdentifiedOrganization
        {
            get;
            internal set;
        }
        public byte DlmsUA
        {
            get;
            internal set;
        }
        public byte ApplicationContext
        {
            get;
            internal set;
        }
        public byte ContextId
        {
            get;
            internal set;
        }

        public override string ToString()
        {
            return LogicalName + " " + JointIsoCtt.ToString() + " " + 
                Country.ToString() + " " + CountryName.ToString() + " " + 
                IdentifiedOrganization.ToString() + " " + DlmsUA.ToString() + " " + 
                ApplicationContext.ToString() + " " + ContextId.ToString();
        }
    }
}
