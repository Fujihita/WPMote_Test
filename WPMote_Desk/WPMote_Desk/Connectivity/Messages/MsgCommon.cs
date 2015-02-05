﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WPMote_Desk.Connectivity.Messages
{
    //Process of adding a message: Add size to dict->Event->Struct->Modify constructor

    abstract class MsgCommon
    {
        #region "Common variables"

        internal const int BUFFER_SIZE = 256;

        internal static Dictionary<byte, Int16> dictMessages = new Dictionary<byte, Int16> 
        { 
            {100,sizeof(Int16)+1}, //TEST CMD
            {101,4*sizeof(byte)+sizeof(Int16)+129}, //ClientInfo: IP & DeviceName, DeviceName 128 chars max
            {150,3*sizeof(float)+sizeof(Int32)+1}, //AccelerometerData: XYZ + (int)flags
            {151,3*sizeof(Int16)+1}, //CompressedAccelData: XYZ
            {152,2*sizeof(bool)+1}, //ClickReceived: RClick & LClick
            {153,2*sizeof(byte)+sizeof(bool)+sizeof(bool)+1} //KeyBDReceived: Key & state
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

                        objWrite.Write((Int16)Math.Min(Encoding.Unicode.GetByteCount(DeviceName.ToCharArray(),0,DeviceName.Length), 128));
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

                    X=objRead.ReadSingle();
                    Y=objRead.ReadSingle();
                    Z=objRead.ReadSingle();
                    flags=objRead.ReadInt32();
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
            public Int16 Z;

            //Constructors
            public CompressedAccelData(byte[] bData)
            {
                var objStream = new MemoryStream(bData);
                var objRead = new BinaryReader(objStream);

                try
                {

                    X = objRead.ReadInt16();
                    Y = objRead.ReadInt16();
                    Z = objRead.ReadInt16();
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

            public CompressedAccelData(Int16 dX, Int16 dY, Int16 dZ)
            {
                X = dX;
                Y = dY;
                Z = dZ;
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

                    RClick=objRead.ReadBoolean();
                    LClick=objRead.ReadBoolean();
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
                RClick=dRClick;
                LClick=dLClick;
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

        internal class KeyBDReceived
        {
            public byte ID = 153;
            public byte KeyBD;
            public bool KeyState;
            public bool extended;

            //Constructors
            public KeyBDReceived(byte[] bData)
            {
                var objStream = new MemoryStream(bData);
                var objRead = new BinaryReader(objStream);

                try
                {

                    KeyBD = objRead.ReadByte();
                    KeyState = objRead.ReadBoolean();
                    extended = objRead.ReadBoolean();
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

            public KeyBDReceived(byte dKeyBD, bool dKeyState, bool extended)
            {
                KeyBD = dKeyBD;
                KeyState = dKeyState;
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

                        objWrite.Write(KeyBD);
                        objWrite.Write(KeyState);
                        objWrite.Write(extended);
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
