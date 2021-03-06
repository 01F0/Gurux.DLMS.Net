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

//
// --------------------------------------------------------------------------
//  Gurux Ltd
// 
namespace Gurux.DLMS
{
    using System;
    using System.Text;

    /// <summary> 
    /// Byte array class is used to save received bytes.
    /// </summary>
    public class GXByteBuffer
    {
        /// <summary>
        /// Array capacity increase size.
        /// </summary>
        const int ArrayCapacity = 10;
                
        ///<summary>
        ///Constructor. 
        ///</summary>        
        public GXByteBuffer()
        {

        }

        
        ///<summary>
        /// Constructor.
        ///</summary>
        ///<param name="capacity">
        /// Buffer capacity.
        ///</param>        
        public GXByteBuffer(UInt16 capacity)
        {
            Capacity = capacity;
        }

        
        ///<summary>
        /// Constructor.
        ///</summary>
        ///<param name="value">
        /// Byte array to attach. 
        ///</param>        
        public GXByteBuffer(byte[] value)
        {
            Capacity = (UInt16)value.Length;
            Set(value);
        }

        
        ///<summary>
        /// Clear buffer but do not release memory. 
        ///</summary>        
        public void Clear()
        {
            Position = 0;
            Size = 0;
        }

        
        ///<summary>
        /// Buffer capacity.
        ///</summary>
        public UInt16 Capacity
        {
            get
            {
                if (Data == null)
                {
                    return 0;
                }
                return (UInt16)Data.Length;
            }
            set
            {
                if (value == 0)
                {
                    Data = null;
                    Size = 0;
                    Position = 0;
                }
                else
                {
                    if (Data == null)
                    {
                        Data = new byte[value];
                    }
                    else
                    {
                        byte[] tmp = Data;
                        Data = new byte[value];
                        if (Size < value)
                        {
                            Buffer.BlockCopy(tmp, 0, Data, 0, Size);
                        }
                        else
                        {
                            Buffer.BlockCopy(tmp, 0, Data, 0, Capacity);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Byte buffer read position.
        /// </summary>
        public UInt16 Position
        {
            get;
            set;
        }

        /// <summary>
        /// Byte buffer data size.
        /// </summary>
        public UInt16 Size
        {
            get;
            set;
        }

        /// <summary>
        /// Returs data as byte array.
        /// </summary>
        /// <returns></returns>
        public byte[] Array()
        {
            byte[] tmp = new byte[Size];
            Buffer.BlockCopy(Data, 0, tmp, 0, Size);
            return tmp;
        }

        /// <summary>
        /// Move content from source to destanation.
        /// </summary>
        /// <param name="srcPos">Source position.</param>
        /// <param name="destPos">Destination position.</param>
        /// <param name="count">Item count.</param>
        public void Move(int srcPos, int destPos, int count)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count");
            }
            Buffer.BlockCopy(Data, srcPos, Data, destPos, count);
        }

        /// <summary>
        /// Push the given byte into this buffer at the current position, and then increments the position.
        /// </summary>
        /// <param name="value">The value to be added.</param>       
        public void SetUInt8(byte value)
        {
            SetUInt8(Size, value);
            ++Size;
        }

        /// <summary>
        /// Push the given enumeration value as byte into this buffer at the current position, and then increments the position.
        /// </summary>
        /// <param name="value">The value to be added.</param> 
        internal void SetUInt8(Enum value)
        {
            SetUInt8(Convert.ToByte(value));
        }

        /// <summary>
        /// Get UInt8 value from byte array from the current position and then increments the position.
        /// </summary>
        public byte GetUInt8()
        {
            byte value = GetUInt8(Position);
            ++Position;
            return value;
        }

        /// <summary>
        /// Push the given UInt8 into this buffer at the given position.
        /// </summary>
        /// <param name="index">Zero based byte index where value is set.</param>
        /// <param name="value"> The byte to be added.</param>    
        public void SetUInt8(UInt16 index, byte value)
        {
            if (index >= Capacity)
            {
                Capacity = (UInt16)(index + ArrayCapacity);
            }
            Data[index] = value;
        }

        /// <summary>
        /// Push the given UInt16 into this buffer at the current position, and then increments the position.
        /// </summary>
        /// <param name="value">The value to be added.</param>    
        public void SetUInt16(UInt16 value)
        {
            SetUInt16(Size, value);
            Size += 2;
        }

        /// <summary>
        /// Push the given Int16 into this buffer at the current position, and then increments the position.
        /// </summary>
        /// <param name="value">The value to be added.</param>    
        public void SetInt16(Int16 value)
        {
            SetInt16(Size, value);
            Size += 2;
        }

        /// <summary>
        /// Get UInt16 value from byte array from the current position and then increments the position.
        /// </summary>
        public UInt16 GetUInt16()
        {
            UInt16 value = GetUInt16(Position);
            Position += 2;
            return value;
        }

        /// <summary>
        /// Push the given UInt16 into this buffer at the given position.
        /// </summary>
        /// <param name="index">Zero based byte index where value is set.</param>
        /// <param name="value">The value to be added.</param>    
        public void SetUInt16(int index, UInt16 value)
        {
            if (index + 2 >= Capacity)
            {
                Capacity = (UInt16) (index + ArrayCapacity);
            }
            Data[index] = (byte)((value >> 8) & 0xFF);
            Data[index + 1] = (byte)(value & 0xFF);
        }

        /// <summary>
        /// Push the given Int16 into this buffer at the given position.
        /// </summary>
        /// <param name="index">Zero based byte index where value is set.</param>
        /// <param name="value">The value to be added.</param>    
        public void SetInt16(int index, Int16 value)
        {
            if (index + 2 >= Capacity)
            {
                Capacity = (UInt16)(index + ArrayCapacity);
            }
            Data[index] = (byte)((value >> 8) & 0xFF);
            Data[index + 1] = (byte)(value & 0xFF);
        }

        /// <summary>
        /// Push the given UInt32 into this buffer at the current position, and then increments the position.
        /// </summary>
        /// <param name="value"> The value to be added.</param>    
        public void SetUInt32(UInt32 value)
        {
            SetUInt32(Size, value);
            Size += 4;
        }

        /// <summary>
        /// Push the given UInt32 into this buffer at the current position, and then increments the position.
        /// </summary>
        /// <param name="value"> The value to be added.</param>    
        public void SetInt32(Int32 value)
        {
            SetInt32(Size, value);
            Size += 4;
        }

        /// <summary>
        /// Get UInt32 value from byte array from the current position and then increments the position.
        /// </summary>
        public UInt32 GetUInt32()
        {
            UInt32 value = GetUInt32(Position);
            Position += 4;
            return value;
        }

        /// <summary>
        /// Push the given UInt32 into this buffer at the given position.
        /// </summary>
        /// <param name="index">Zero based byte index where value is set.</param>
        /// <param name="value"> The value to be added.</param>    
        public void SetUInt32(int index, UInt32 item)
        {
            if (index + 4 >= Capacity)
            {
                Capacity = (UInt16) (index + ArrayCapacity);
            }
            Data[index] = (byte)((item >> 24) & 0xFF);
            Data[index + 1] = (byte)((item >> 16) & 0xFF);
            Data[index + 2] = (byte)((item >> 8) & 0xFF);
            Data[index + 3] = (byte)(item & 0xFF);
        }

        /// <summary>
        /// Push the given UInt32 into this buffer at the given position.
        /// </summary>
        /// <param name="index">Zero based byte index where value is set.</param>
        /// <param name="value"> The value to be added.</param>    
        public void SetInt32(int index, Int32 item)
        {
            if (index + 4 >= Capacity)
            {
                Capacity = (UInt16)(index + ArrayCapacity);
            }
            Data[index] = (byte)((item >> 24) & 0xFF);
            Data[index + 1] = (byte)((item >> 16) & 0xFF);
            Data[index + 2] = (byte)((item >> 8) & 0xFF);
            Data[index + 3] = (byte)(item & 0xFF);
        }

        /// <summary>
        /// Push the given UInt64 into this buffer at the current position, and then increments the position.
        /// </summary>
        /// <param name="value"> The value to be added.</param>    
        public void SetUInt64(UInt64 value)
        {
            SetUInt64(Size, value);
            Size += 8;
        }

        /// <summary>
        /// Push the given UInt64 into this buffer at the current position, and then increments the position.
        /// </summary>
        /// <param name="value"> The value to be added.</param>    
        public void SetInt64(Int64 value)
        {
            SetInt64(Size, value);
            Size += 8;
        }


        /// <summary>
        /// Get UInt64 value from byte array from the current position and then increments the position.
        /// </summary>
        public UInt64 GetUInt64()
        {
            UInt64 value = BitConverter.ToUInt64(Data, Position);
            Position += 8;
            return value;
        }

        /// <summary>
        /// Push the given UInt64 into this buffer at the given position.
        /// </summary>
        /// <param name="index">Zero based byte index where value is set.</param>
        /// <param name="value"> The value to be added.</param>    
        public void SetUInt64(int index, UInt64 item)
        {
            if (index + 8 >= Capacity)
            {
                Capacity = (UInt16) (index + ArrayCapacity);
            }
            Data[Size + 7] = (byte)((item >> 56) & 0xFF);
            Data[Size + 6] = (byte)((item >> 48) & 0xFF);
            Data[Size + 5] = (byte)((item >> 40) & 0xFF);
            Data[Size + 4] = (byte)((item >> 32) & 0xFF);
            Data[Size + 3] = (byte)((item >> 24) & 0xFF);
            Data[Size + 2] = (byte)((item >> 16) & 0xFF);
            Data[Size + 1] = (byte)((item >> 8) & 0xFF);
            Data[Size] = (byte)(item & 0xFF);
        }

        /// <summary>
        /// Push the given UInt64 into this buffer at the given position.
        /// </summary>
        /// <param name="index">Zero based byte index where value is set.</param>
        /// <param name="value"> The value to be added.</param>    
        public void SetInt64(int index, Int64 item)
        {
            if (index + 8 >= Capacity)
            {
                Capacity = (UInt16)(index + ArrayCapacity);
            }
            Data[Size + 7] = (byte)((item >> 56) & 0xFF);
            Data[Size + 6] = (byte)((item >> 48) & 0xFF);
            Data[Size + 5] = (byte)((item >> 40) & 0xFF);
            Data[Size + 4] = (byte)((item >> 32) & 0xFF);
            Data[Size + 3] = (byte)((item >> 24) & 0xFF);
            Data[Size + 2] = (byte)((item >> 16) & 0xFF);
            Data[Size + 1] = (byte)((item >> 8) & 0xFF);
            Data[Size] = (byte)(item & 0xFF);
        }

        /// <summary>
        /// Get Int8 value from byte array from the current position and then increments the position.
        /// </summary>
        public sbyte GetInt8()
        {
            sbyte value = (sbyte)GetUInt8(Position);
            ++Position;
            return value;
        }

        /// <summary>
        /// Get UInt8 value from byte array.
        /// </summary>
        /// <param name="index">Byte index.</param>
        public byte GetUInt8(int index)
        {
            if (index >= Size)
            {
                throw new System.OutOfMemoryException();
            }
            return Data[index];
        }

        /// <summary>
        /// Get UInt16 value from byte array.
        /// </summary>
        /// <param name="index">Byte index.</param>
        public UInt16 GetUInt16(int index)
        {
            if (index + 2 > Size)
            {
                throw new System.OutOfMemoryException();
            }
            return (UInt16)(((Data[index] & 0xFF) << 8) | (Data[index + 1] & 0xFF));
        }

        /// <summary>
        /// Get UInt32 value from byte array from the current position and then increments the position.
        /// </summary>
        public Int32 GetInt32()
        {
            Int32 value = (Int32)GetUInt32(Position);
            Position += 4;
            return value;
        }

        /// <summary>
        /// Get Int16 value from byte array from the current position and then increments the position.
        /// </summary>
        public Int16 GetInt16()
        {
            Int16 value = (Int16)GetInt16(Position);
            Position += 2;
            return value;
        }

        /// <summary>
        /// Get Int16 value from byte array.
        /// </summary>
        /// <param name="index">Byte index.</param>
        public Int16 GetInt16(int index)
        {
            if (index + 2 > Size)
            {
                throw new System.OutOfMemoryException();
            }
            return (Int16)(((Data[index] & 0xFF) << 8) | (Data[index + 1] & 0xFF));
        }

        /// <summary>
        /// Get Int32 value from byte array.
        /// </summary>
        /// <param name="index">Byte index.</param>
        public UInt32 GetUInt32(int index)
        {
            if (index + 4 > Size)
            {
                throw new System.OutOfMemoryException();
            }
            return (UInt32)((Data[index] & 0xFF) << 24 | (Data[index + 1] & 0xFF) << 16 |
                (Data[index + 2] & 0xFF) << 8 | (Data[index + 3] & 0xFF));            
        }

        /// <summary>
        /// Get float value from byte array from the current position and then increments the position.
        /// </summary>
        public float GetFloat()
        {
            float value = BitConverter.ToSingle(Data, Position);
            Position += 4;
            return value;
        }

        /// <summary>
        /// Get double value from byte array from the current position and then increments the position.
        /// </summary>
        public double GetDouble()
        {
            double value = BitConverter.ToDouble(Data, Position);
            Position += 8;
            return value;
        }

        /// <summary>
        /// Get Int64 value from byte array from the current position and then increments the position.
        /// </summary>
        public Int64 GetInt64()
        {
            Int64 value = BitConverter.ToInt64(Data, Position);
            Position += 8;
            return value;
        }


        /// <summary>
        /// The data as byte array.
        /// </summary>
        public byte[] Data
        {
            get;
            set;
        }

        /// <summary>
        /// Get String value from byte array.
        /// </summary>
        /// <param name="count">Byte count.</param>
        public string GetString(int count)
        {
            string str =  ASCIIEncoding.ASCII.GetString(Data, Position, count);
            Position = (UInt16) (Position + count);
            return str;
        }

        /// <summary>
        /// Get String value from byte array.
        /// </summary>
        /// <param name="index">Byte index.</param>
        /// <param name="count">Byte count.</param>
        public string GetString(int index, int count)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count");
            }
            if (index + count > Size)
            {
                throw new System.OutOfMemoryException();
            }
            return ASCIIEncoding.ASCII.GetString(Data, index, count);
        }

        /// <summary>
        /// Push the given byte array into this buffer at the current position, and then increments the position.
        /// </summary>
        /// <param name="value"> The value to be added.</param>
        public void Set(byte[] value)
        {
            if (value != null)
            {
                Set(value, 0, value.Length);
            }
        }

        /// <summary>
        /// Set new value to byte array.
        /// </summary>
        /// <param name="value">Byte array to add.</param>
        /// <param name="index">Byte index.</param>
        /// <param name="count">Byte count.</param>
        public void Set(byte[] value, int index, int count)
        {
            if (value != null && count != 0)
            {
                if (Size + count > Capacity)
                {
                    Capacity = (UInt16) (Size + count + ArrayCapacity);
                }
                Buffer.BlockCopy(value, index, Data, Size, count);
                Size += (UInt16) count;
            }
        }
        
        /// <summary>
        /// Add new object to the byte buffer.      
        /// </summary>
        /// <param name="value">Value to add.</param>
        public void Add(object value)
        {
            if (value != null)
            {
                if (value is byte[])
                {
                    Set((byte[])value);
                }
                else if (value is byte)
                {
                    SetUInt8((byte)value);
                }
                else if (value is UInt16)
                {
                    SetUInt16((UInt16)value);
                }
                else if (value is UInt32)
                {
                    SetUInt32((UInt32)value);
                }
                else if (value is UInt64)
                {
                    SetUInt64((UInt64)value);
                }
                else if (value is Int16)
                {
                    SetInt16((Int16)value);
                }
                else if (value is Int32)
                {
                    SetInt32((Int32)value);
                }
                else if (value is Int64)
                {
                    SetInt64((Int64)value);
                }

                else if (value is string)
                {
                    Set(ASCIIEncoding.ASCII.GetBytes((string) value));
                }
                else
                {
                    throw new ArgumentException("Invalid object type.");
                }
            }
        }

        /// <summary>
        /// Get value from the byte array.
        /// </summary>
        /// <param name="target">Target array.</param>
        public void Get(byte[] target)
        {
            if (Size - Position < target.Length)
            {
                throw new OutOfMemoryException();
            }
            Buffer.BlockCopy(Data, Position, target, 0, target.Length);
            Position += (UInt16) target.Length;
        }
                
        /// <summary>
        /// Compares, whether two given arrays are similar starting from current position.
        /// </summary>
        /// <param name="arr">Array to compare.</param>
        /// <returns>True, if arrays are similar. False, if the arrays differ.</returns>
        public bool Compare(byte[] arr)
        {
            if (Size - Position < arr.Length)
            {
                return false;
            }
            byte[] bytes = new byte[arr.Length];
            Get(bytes);
            bool ret = Gurux.DLMS.Internal.GXCommon.Compare(bytes, arr);
            if (!ret)
            {
                this.Position -= (UInt16) arr.Length;
            }
            return ret;
        }

        public override string ToString()
        {
            return Gurux.DLMS.Internal.GXCommon.ToHex(Data, true, 0, Size);
        }
    }
}
