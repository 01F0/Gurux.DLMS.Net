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
using System.Text;
using System.ComponentModel;
using Gurux.DLMS;
using System.Xml.Serialization;
using Gurux.DLMS.ManufacturerSettings;
using Gurux.DLMS.Internal;
using Gurux.DLMS.Enums;

namespace Gurux.DLMS.Objects
{   
    public class GXDLMSRegisterActivation : GXDLMSObject, IGXDLMSBase
    {        
        /// <summary> 
        /// Constructor.
        /// </summary> 
        public GXDLMSRegisterActivation()
            : base(ObjectType.RegisterActivation)
        {
            MaskList = new List<KeyValuePair<byte[], byte[]>>();
        }

        /// <summary> 
        /// Constructor.
        /// </summary> 
        /// <param name="ln">Logical Name of the object.</param>
        public GXDLMSRegisterActivation(string ln)
            : base(ObjectType.RegisterActivation, ln, 0)
        {
            MaskList = new List<KeyValuePair<byte[], byte[]>>();
        }

        /// <summary> 
        /// Constructor.
        /// </summary> 
        /// <param name="ln">Logical Name of the object.</param>
        /// <param name="sn">Short Name of the object.</param>
        public GXDLMSRegisterActivation(string ln, ushort sn)
            : base(ObjectType.RegisterActivation, ln, sn)
        {
            MaskList = new List<KeyValuePair<byte[], byte[]>>();
        }

        /// <summary>
        ///
        /// </summary>
        [XmlIgnore()]
        public GXDLMSObjectDefinition[] RegisterAssignment
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [XmlIgnore()]
        public List<KeyValuePair<byte[], byte[]>> MaskList
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [XmlIgnore()]
        public byte[] ActiveMask
        {
            get;
            set;
        }

        /// <inheritdoc cref="GXDLMSObject.GetValues"/>
        public override object[] GetValues()
        {
            return new object[] { LogicalName, RegisterAssignment, MaskList, ActiveMask };
        }

        #region IGXDLMSBase Members

        /// <summary>
        /// Data interface do not have any methods.
        /// </summary>
        /// <param name="index"></param>
        byte[] IGXDLMSBase.Invoke(GXDLMSSettings settings, int index, Object parameters)
        {
            throw new ArgumentException("Invoke failed. Invalid attribute index.");
        }

        int[] IGXDLMSBase.GetAttributeIndexToRead()
        {
            List<int> attributes = new List<int>();
            //LN is static and read only once.
            if (string.IsNullOrEmpty(LogicalName))
            {
                attributes.Add(1);
            }
            //RegisterAssignment
            if (!base.IsRead(2))
            {
                attributes.Add(2);
            }
            //MaskList
            if (!base.IsRead(3))
            {
                attributes.Add(3);
            }
            //ActiveMask
            if (!base.IsRead(4))
            {
                attributes.Add(4);
            }
            return attributes.ToArray();
        }

        /// <inheritdoc cref="IGXDLMSBase.GetNames"/>
        string[] IGXDLMSBase.GetNames()
        {
            return new string[] { Gurux.DLMS.Properties.Resources.LogicalNameTxt, "Register Assignment", "Mask List", "Active Mask" };            
        }

        int IGXDLMSBase.GetAttributeCount()
        {
            return 4;
        }

        int IGXDLMSBase.GetMethodCount()
        {
            return 3;
        }

        override public DataType GetDataType(int index)
        {
            if (index == 1)
            {
                return DataType.OctetString;
            }
            if (index == 2)
            {
                return DataType.Array;                
            }
            if (index == 3)
            {
                return DataType.Array;
            }
            if (index == 4)
            {
                return DataType.OctetString;
            }
            throw new ArgumentException("GetDataType failed. Invalid attribute index.");
        }

        object IGXDLMSBase.GetValue(GXDLMSSettings settings, int index, int selector, object parameters)
        {
            if (index == 1)
            {
                return this.LogicalName;
            }
            if (index == 2)
            {
                GXByteBuffer data = new GXByteBuffer();     
                data.SetUInt8((byte) DataType.Array);
                if (RegisterAssignment == null)
                {
                    data.SetUInt8(0);
                }
                else
                {
                    data.SetUInt8((byte) RegisterAssignment.Length);
                    foreach (GXDLMSObjectDefinition it in RegisterAssignment)
                    {
                        data.SetUInt8((byte) DataType.Structure);
                        data.SetUInt8(2);
                        GXCommon.SetData(data, DataType.UInt16, it.ClassId);
                        GXCommon.SetData(data, DataType.OctetString, it.LogicalName);
                    }
                }
                return data.Array();
            }
            if (index == 3)
            {                
                GXByteBuffer data = new GXByteBuffer();
                data.SetUInt8((byte)DataType.Array);
                if (MaskList == null)
                {
                    data.SetUInt8(0);
                }
                else
                {
                    data.SetUInt8((byte)MaskList.Count);
                    foreach (KeyValuePair<byte[], byte[]> it in MaskList)
                    {
                        data.SetUInt8((byte)DataType.Structure);
                        data.SetUInt8(2);
                        GXCommon.SetData(data, DataType.OctetString, it.Key);
                        data.SetUInt8((byte)DataType.Array);
                        data.SetUInt8((byte)it.Value.Length);
                        foreach (byte b in it.Value)
                        {
                            GXCommon.SetData(data, DataType.UInt8, b);
                        }
                    }
                }
                return data.Array();
            }
            if (index == 4)
            {
                return ActiveMask;
            }
            throw new ArgumentException("GetValue failed. Invalid attribute index.");
        }

        void IGXDLMSBase.SetValue(GXDLMSSettings settings, int index, object value) 
        {
            if (index == 1)
            {
                if (value is string)
                {
                    LogicalName = value.ToString();
                }
                else
                {
                    LogicalName = GXDLMSClient.ChangeType((byte[])value, DataType.OctetString).ToString();
                }
            }
            else if (index == 2)
            {
                List<GXDLMSObjectDefinition> items = new List<GXDLMSObjectDefinition>();
                if (value != null)
                {                    
                    foreach(Object[] it in (Object[]) value)
                    {
                        GXDLMSObjectDefinition item = new GXDLMSObjectDefinition();
                        item.ClassId = (ObjectType) Convert.ToInt32(it[0]);
                        item.LogicalName = GXDLMSObject.ToLogicalName((byte[]) it[1]);
                        items.Add(item);
                    }                    
                }
                RegisterAssignment = items.ToArray();
            }
            else if (index == 3)
            {
                MaskList.Clear();                
                if (value != null)
                {                   
                    foreach (Object[] it in (Object[])value)
                    {
                        List<byte> index_list = new List<byte>();
                        foreach(byte b in (Object[]) it[1])
                        {
                            index_list.Add(b);
                        }
                        MaskList.Add(new KeyValuePair<byte[], byte[]>((byte[])it[0], index_list.ToArray()));
                    }                    
                }                
            }
            else if (index == 4)
            {
                if (value == null)
                {
                    ActiveMask = null;
                }
                else
                {
                    ActiveMask = (byte[])value;
                }
            }   
            else
            {
                throw new ArgumentException("SetValue failed. Invalid attribute index.");
            }
        }
        #endregion
    }
}
