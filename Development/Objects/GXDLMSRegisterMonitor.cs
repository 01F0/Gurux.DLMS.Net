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
using System.Xml.Serialization;
using Gurux.DLMS.ManufacturerSettings;
using Gurux.DLMS.Internal;
using Gurux.DLMS.Enums;

namespace Gurux.DLMS.Objects
{
    public class GXDLMSRegisterMonitor : GXDLMSObject, IGXDLMSBase
    {
        /// <summary> 
        /// Constructor.
        /// </summary> 
        public GXDLMSRegisterMonitor()
            : base(ObjectType.RegisterMonitor)
        {
            this.Thresholds = new object[0];            
            MonitoredValue = new GXDLMSMonitoredValue();
            Actions = new GXDLMSActionSet[0];
        }

        /// <summary> 
        /// Constructor.
        /// </summary> 
        /// <param name="ln">Logical Name of the object.</param>
        public GXDLMSRegisterMonitor(string ln)
            : base(ObjectType.RegisterMonitor, ln, 0)
        {
            this.Thresholds = new object[0];
            MonitoredValue = new GXDLMSMonitoredValue();
            Actions = new GXDLMSActionSet[0];
        }

        /// <summary> 
        /// Constructor.
        /// </summary> 
        /// <param name="ln">Logical Name of the object.</param>
        /// <param name="sn">Short Name of the object.</param>
        public GXDLMSRegisterMonitor(string ln, ushort sn)
            : base(ObjectType.RegisterMonitor, ln, sn)
        {
            this.Thresholds = new object[0];
            MonitoredValue = new GXDLMSMonitoredValue();
            Actions = new GXDLMSActionSet[0];
        }
        
        [XmlIgnore()]
        public object[] Thresholds
        {
            get;
            set;
        }

        [XmlIgnore()]
        public GXDLMSMonitoredValue MonitoredValue
        {
            get;
            internal set;
        }


        [XmlIgnore()]
        public GXDLMSActionSet[] Actions
        {
            get;
            set;
        }

        /// <inheritdoc cref="GXDLMSObject.GetValues"/>
        public override object[] GetValues()
        {
            return new object[] { LogicalName, Thresholds, MonitoredValue, Actions };
        }

        #region IGXDLMSBase Members

        int[] IGXDLMSBase.GetAttributeIndexToRead()
        {
            List<int> attributes = new List<int>();
            //LN is static and read only once.
            if (string.IsNullOrEmpty(LogicalName))
            {
                attributes.Add(1);
            }
            //Thresholds
            if (!base.IsRead(2))
            {
                attributes.Add(2);
            }
            //MonitoredValue
            if (!base.IsRead(3))
            {
                attributes.Add(3);
            }
            //Actions
            if (!base.IsRead(4))
            {
                attributes.Add(4);
            }
            return attributes.ToArray();
        }

        /// <inheritdoc cref="IGXDLMSBase.GetNames"/>
        string[] IGXDLMSBase.GetNames()
        {
            return new string[] { Gurux.DLMS.Properties.Resources.LogicalNameTxt, "Thresholds", "Monitored Value", "Actions" };            
        }

        int IGXDLMSBase.GetAttributeCount()
        {
            return 4;
        }

        int IGXDLMSBase.GetMethodCount()
        {
            return 0;
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
                return DataType.Array;
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
                return this.Thresholds;
            }
            if (index == 3)
            {
                GXByteBuffer data = new GXByteBuffer();
                data.SetUInt8((int) DataType.Structure);
                data.SetUInt8(3);
                GXCommon.SetData(data, DataType.UInt16, MonitoredValue.ObjectType); //ClassID
                GXCommon.SetData(data, DataType.OctetString, MonitoredValue.LogicalName); //Logical name.
                GXCommon.SetData(data, DataType.Int8, MonitoredValue.AttributeIndex); //Attribute Index
                return data.Array();
            }
            if (index == 4)
            {
                GXByteBuffer data = new GXByteBuffer();
                data.SetUInt8((int)DataType.Structure);
                if (Actions == null)
                {
                    data.SetUInt8(0);
                }
                else
                {
                    data.SetUInt8((byte)Actions.Length);
                    foreach (GXDLMSActionSet it in Actions)
                    {
                        data.SetUInt8((int)DataType.Structure);
                        data.SetUInt8(2);
                        data.SetUInt8((int)DataType.Structure);
                        data.SetUInt8(2);
                        GXCommon.SetData(data, DataType.OctetString, it.ActionUp.LogicalName); //Logical name.
                        GXCommon.SetData(data, DataType.UInt16, it.ActionUp.ScriptSelector); //ScriptSelector
                        data.SetUInt8((int)DataType.Structure);
                        data.SetUInt8(2);
                        GXCommon.SetData(data, DataType.OctetString, it.ActionDown.LogicalName); //Logical name.
                        GXCommon.SetData(data, DataType.UInt16, it.ActionDown.ScriptSelector); //ScriptSelector                        
                    }
                }                
                return data.Array();
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
                Thresholds = (object[]) value;
            }
            else if (index == 3)
            {
                if (MonitoredValue == null)
                {
                    MonitoredValue = new GXDLMSMonitoredValue();
                }
                MonitoredValue.ObjectType = (ObjectType)Convert.ToInt32(((object[])value)[0]);
                MonitoredValue.LogicalName = GXDLMSClient.ChangeType((byte[])((object[])value)[1], DataType.OctetString).ToString();
                MonitoredValue.AttributeIndex = Convert.ToInt32(((object[])value)[2]);
            }
            else if (index == 4)
            {
                Actions = null;
                if (value != null)
                {
                    List<GXDLMSActionSet> items = new List<GXDLMSActionSet>();
                    foreach (Object[] action_set in (Object[])value)
                    {
                        GXDLMSActionSet set = new GXDLMSActionSet();                        
                        set.ActionUp.LogicalName = GXDLMSClient.ChangeType((byte[])((Object[])action_set[0])[0], DataType.OctetString).ToString();
                        set.ActionUp.ScriptSelector = Convert.ToUInt16(((Object[])action_set[0])[1]);
                        set.ActionDown.LogicalName = GXDLMSClient.ChangeType((byte[])((Object[])action_set[1])[0], DataType.OctetString).ToString();
                        set.ActionDown.ScriptSelector = Convert.ToUInt16(((Object[])action_set[1])[1]);
                        items.Add(set);
                    }
                    Actions = items.ToArray();
                }
            }
            else
            {
                throw new ArgumentException("SetValue failed. Invalid attribute index.");
            }
        }

        byte[] IGXDLMSBase.Invoke(GXDLMSSettings settings, int index, Object parameters)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
