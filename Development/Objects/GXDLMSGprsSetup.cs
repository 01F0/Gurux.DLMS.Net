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
using Gurux.DLMS;
using System.ComponentModel;
using System.Xml.Serialization;
using Gurux.DLMS.ManufacturerSettings;
using Gurux.DLMS.Internal;
using Gurux.DLMS.Enums;

namespace Gurux.DLMS.Objects
{
    public class GXDLMSGprsSetup : GXDLMSObject, IGXDLMSBase
    {       
        /// <summary> 
        /// Constructor.
        /// </summary> 
        public GXDLMSGprsSetup()
            : base(ObjectType.GprsSetup)
        {
            DefaultQualityOfService = new GXDLMSQosElement();
            RequestedQualityOfService = new GXDLMSQosElement();
        }

        /// <summary> 
        /// Constructor.
        /// </summary> 
        /// <param name="ln">Logical Name of the object.</param>
        public GXDLMSGprsSetup(string ln)
            : base(ObjectType.GprsSetup, ln, 0)
        {
            DefaultQualityOfService = new GXDLMSQosElement();
            RequestedQualityOfService = new GXDLMSQosElement();
        }

        /// <summary> 
        /// Constructor.
        /// </summary> 
        /// <param name="ln">Logical Name of the object.</param>
        /// <param name="sn">Short Name of the object.</param>
        public GXDLMSGprsSetup(string ln, ushort sn)
            : base(ObjectType.GprsSetup, ln, sn)
        {
            DefaultQualityOfService = new GXDLMSQosElement();
            RequestedQualityOfService = new GXDLMSQosElement();
        }

        [XmlIgnore()]
        public string APN
        {
            get;
            set;
        }

        [XmlIgnore()]
        public long PINCode
        {
            get;
            set;
        }

        [XmlIgnore()]
        public GXDLMSQosElement DefaultQualityOfService
        {
            get;
            set;
        }

        [XmlIgnore()]
        public GXDLMSQosElement RequestedQualityOfService
        {
            get;
            set;
        }

        /// <inheritdoc cref="GXDLMSObject.GetValues"/>
        public override object[] GetValues()
        {
            return new object[] { LogicalName, APN, PINCode, 
                new object[] { DefaultQualityOfService, RequestedQualityOfService } };
        }

        #region IGXDLMSBase Members


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
            //APN
            if (!base.IsRead(2))
            {
                attributes.Add(2);
            }            
            //PINCode
            if (!base.IsRead(3))
            {
                attributes.Add(3);
            }
            //DefaultQualityOfService + RequestedQualityOfService
            if (!base.IsRead(4))
            {
                attributes.Add(4);
            }
            return attributes.ToArray();
        }

        /// <inheritdoc cref="IGXDLMSBase.GetNames"/>
        string[] IGXDLMSBase.GetNames()
        {
            return new string[] {Gurux.DLMS.Properties.Resources.LogicalNameTxt, 
                "APN", "PIN Code",
                "Default Quality Of Service and Requested Quality Of Service"};
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
                return DataType.OctetString;
            }
            if (index == 3)
            {
                return DataType.UInt16;
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
                return APN;
            }
            if (index == 3)
            {
                return PINCode;
            }
            if (index == 4)
            {
                GXByteBuffer data = new GXByteBuffer();
                data.SetUInt8((byte)DataType.Structure);
                //Add count            
                data.SetUInt8((byte)2);
                data.SetUInt8((byte)DataType.Structure);
                data.SetUInt8((byte)5);
                if (DefaultQualityOfService != null)
                {
                    GXCommon.SetData(data, DataType.UInt8, DefaultQualityOfService.Precedence);
                    GXCommon.SetData(data, DataType.UInt8, DefaultQualityOfService.Delay);
                    GXCommon.SetData(data, DataType.UInt8, DefaultQualityOfService.Reliability);
                    GXCommon.SetData(data, DataType.UInt8, DefaultQualityOfService.PeakThroughput);
                    GXCommon.SetData(data, DataType.UInt8, DefaultQualityOfService.MeanThroughput);
                }
                else
                {
                    GXCommon.SetData(data, DataType.UInt8, 0);
                    GXCommon.SetData(data, DataType.UInt8, 0);
                    GXCommon.SetData(data, DataType.UInt8, 0);
                    GXCommon.SetData(data, DataType.UInt8, 0);
                    GXCommon.SetData(data, DataType.UInt8, 0);
                }
                if (RequestedQualityOfService != null)
                {
                    GXCommon.SetData(data, DataType.UInt8, RequestedQualityOfService.Precedence);
                    GXCommon.SetData(data, DataType.UInt8, RequestedQualityOfService.Delay);
                    GXCommon.SetData(data, DataType.UInt8, RequestedQualityOfService.Reliability);
                    GXCommon.SetData(data, DataType.UInt8, RequestedQualityOfService.PeakThroughput);
                    GXCommon.SetData(data, DataType.UInt8, RequestedQualityOfService.MeanThroughput);
                }
                else
                {
                    GXCommon.SetData(data, DataType.UInt8, 0);
                    GXCommon.SetData(data, DataType.UInt8, 0);
                    GXCommon.SetData(data, DataType.UInt8, 0);
                    GXCommon.SetData(data, DataType.UInt8, 0);
                    GXCommon.SetData(data, DataType.UInt8, 0);
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
                if (value is string)
                {
                    APN = value.ToString();
                }
                else
                {
                    APN = GXDLMSClient.ChangeType((byte[])value, DataType.String).ToString();
                }
            }
            else if (index == 3)
            {
                PINCode = Convert.ToInt16(value);
            }
            else if (index == 4)
            {
                DefaultQualityOfService.Precedence = DefaultQualityOfService.Delay = DefaultQualityOfService.Reliability = DefaultQualityOfService.PeakThroughput = DefaultQualityOfService.MeanThroughput = 0;
                RequestedQualityOfService.Precedence = RequestedQualityOfService.Delay = RequestedQualityOfService.Reliability = RequestedQualityOfService.PeakThroughput = RequestedQualityOfService.MeanThroughput = 0;
                if (value != null)
                {
                    Object[] tmp = (Object[])value;
                    DefaultQualityOfService.Precedence = Convert.ToByte((tmp[0] as Object[])[0]);
                    DefaultQualityOfService.Delay = Convert.ToByte((tmp[0] as Object[])[1]);
                    DefaultQualityOfService.Reliability = Convert.ToByte((tmp[0] as Object[])[2]);
                    DefaultQualityOfService.PeakThroughput = Convert.ToByte((tmp[0] as Object[])[3]);
                    DefaultQualityOfService.MeanThroughput = Convert.ToByte((tmp[0] as Object[])[4]);
                    RequestedQualityOfService.Precedence = Convert.ToByte((tmp[1] as Object[])[0]);
                    RequestedQualityOfService.Delay = Convert.ToByte((tmp[1] as Object[])[1]);
                    RequestedQualityOfService.Reliability = Convert.ToByte((tmp[1] as Object[])[2]);
                    RequestedQualityOfService.PeakThroughput = Convert.ToByte((tmp[1] as Object[])[3]);
                    RequestedQualityOfService.MeanThroughput = Convert.ToByte((tmp[1] as Object[])[4]);
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
