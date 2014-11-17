﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WPMote.Connectivity.Messages
{
    public class MsgCommon
    {
        //Process of adding a message: Add size to dict->Event->Struct->Modify constructor
        
        #region "Common variables"
        
        internal static Dictionary<byte, Int16> dictMessages = new Dictionary<byte, Int16> 
        { 
            {100,sizeof(Int16)+1}, //TEST CMD
            {101,4*sizeof(byte)+sizeof(Int16)+129}, //ClientInfo: IP & DeviceName, DeviceName 128 chars max
            {150,3*sizeof(float)+sizeof(Int32)+1}, //AccelerometerData: XYZ + (int)flags
            {151,2*sizeof(Int16)+1}, //CompressedAccelData: XY
            {152,2*sizeof(bool)+1} //ClickReceived: RClick & LClick
        };

        #endregion

        #region "Message Structs"

        internal class Msg_Test
        {
            public byte ID = 100;
            //Constructors
            public Msg_Test(byte[] bData) { }

            public Msg_Test() { }

            //To byte array
            public byte[] ToByteArray
            {
                get
                {
                    var bData = new byte[dictMessages[ID]];
                    var objStream = new MemoryStream(bData);
                    var objWrite = new BinaryWriter(objStream);

                    try
                    {
                        objWrite.Write(ID);
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        objWrite.Flush();
                        objWrite.Dispose();
                        objStream.Dispose();
                    }

                    return bData;
                }
            }
        }

        internal class Msg_ClientInfo
        {
            public byte ID = 101;
            public string IPAddress;
            public string DeviceName;

            //Constructors
            public Msg_ClientInfo(byte[] bData)
            {
                var objStream = new MemoryStream(bData);
                var objRead = new BinaryReader(objStream);

                try
                {
                    IPAddress = objRead.ReadByte().ToString() + "." +
                        objRead.ReadByte().ToString() + "." +
                        objRead.ReadByte().ToString() + "." +
                        objRead.ReadByte().ToString();

                    Int16 strLength = objRead.ReadInt16();
                    string strData = Encoding.Unicode.GetString(objRead.ReadBytes(128), 0, strLength);

                    DeviceName = strData;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    objRead.Dispose();
                    objStream.Dispose();
                }
            }

            public Msg_ClientInfo(string strIP, string strName)
            {
                IPAddress = strIP;
                DeviceName = strName;
            }

            //To byte array
            public byte[] ToByteArray
            {
                get
                {
                    var bData = new byte[dictMessages[ID]];
                    var objStream = new MemoryStream(bData);
                    var objWrite = new BinaryWriter(objStream);

                    try
                    {
                        objWrite.Write(ID);

                        //IP Address to byte()
                        string[] strIPTemp = IPAddress.Split('.');

                        if (strIPTemp.Length != 4) throw new Exception("Invalid IP Address");
                        foreach (var temp in strIPTemp)
                        {
                            objWrite.Write(Convert.ToByte(temp));
                        }

                        objWrite.Write((Int16)Math.Min(Encoding.Unicode.GetByteCount(DeviceName.ToCharArray(), 0, DeviceName.Length), 128));
                        objWrite.Write(Encoding.Unicode.GetBytes(DeviceName));
                        objWrite.Flush();
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        objWrite.Dispose();
                        objStream.Dispose();
                    }

                    return bData;
                }
            }
        }

        internal class Msg_AccelerometerData
        {
            public byte ID = 150;
            public float X;
            public float Y;
            public float Z;
            public Int32 flags;

            //Constructors
            public Msg_AccelerometerData(byte[] bData)
            {
                var objStream = new MemoryStream(bData);
                var objRead = new BinaryReader(objStream);

                try
                {

                    X = objRead.ReadSingle();
                    Y = objRead.ReadSingle();
                    Z = objRead.ReadSingle();
                    flags = objRead.ReadInt32();
                }
                catch
                {
                    throw;
                }
                finally
                {
                    objRead.Dispose();
                    objStream.Dispose();
                }
            }

            public Msg_AccelerometerData(float dX, float dY, float dZ, Int32 dFlags)
            {
                X = dX;
                Y = dY;
                Z = dZ;
                flags = dFlags;
            }

            //To byte array
            public byte[] ToByteArray
            {
                get
                {
                    var bData = new byte[dictMessages[ID]];
                    var objStream = new MemoryStream(bData);
                    var objWrite = new BinaryWriter(objStream);

                    try
                    {
                        objWrite.Write(ID);

                        objWrite.Write(X);
                        objWrite.Write(Y);
                        objWrite.Write(Z);
                        objWrite.Write(flags);
                        objWrite.Flush();
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        objWrite.Dispose();
                        objStream.Dispose();
                    }

                    return bData;
                }
            }
        }

        internal class CompressedAccelData
        {
            public byte ID = 151;
            public Int16 X;
            public Int16 Y;

            //Constructors
            public CompressedAccelData(byte[] bData)
            {
                var objStream = new MemoryStream(bData);
                var objRead = new BinaryReader(objStream);

                try
                {

                    X = objRead.ReadInt16();
                    Y = objRead.ReadInt16();
                }
                catch
                {
                    throw;
                }
                finally
                {
                    objRead.Dispose();
                    objStream.Dispose();
                }
            }

            public CompressedAccelData(Int16 dX, Int16 dY)
            {
                X = dX;
                Y = dY;
            }

            //To byte array
            public byte[] ToByteArray
            {
                get
                {
                    var bData = new byte[dictMessages[ID]];
                    var objStream = new MemoryStream(bData);
                    var objWrite = new BinaryWriter(objStream);

                    try
                    {
                        objWrite.Write(ID);

                        objWrite.Write(X);
                        objWrite.Write(Y);
                        objWrite.Flush();
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        objWrite.Dispose();
                        objStream.Dispose();
                    }

                    return bData;
                }
            }
        }

        internal class ClickReceived
        {
            public byte ID = 152;
            public bool RClick;
            public bool LClick;

            //Constructors
            public ClickReceived(byte[] bData)
            {
                var objStream = new MemoryStream(bData);
                var objRead = new BinaryReader(objStream);

                try
                {

                    RClick = objRead.ReadBoolean();
                    LClick = objRead.ReadBoolean();
                }
                catch
                {
                    throw;
                }
                finally
                {
                    objRead.Dispose();
                    objStream.Dispose();
                }
            }

            public ClickReceived(bool dRClick, bool dLClick)
            {
                RClick = dRClick;
                LClick = dLClick;
            }

            //To byte array
            public byte[] ToByteArray
            {
                get
                {
                    var bData = new byte[dictMessages[ID]];
                    var objStream = new MemoryStream(bData);
                    var objWrite = new BinaryWriter(objStream);

                    try
                    {
                        objWrite.Write(ID);

                        objWrite.Write(RClick);
                        objWrite.Write(LClick);
                        objWrite.Flush();
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        objWrite.Dispose();
                        objStream.Dispose();
                    }

                    return bData;
                }
            }
        }

        #endregion

    }
}
