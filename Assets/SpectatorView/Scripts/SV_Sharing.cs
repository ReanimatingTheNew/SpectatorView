using UnityEngine;
using HoloToolkit.Sharing;
using System.Collections.Generic;
using System;

public class SV_Sharing : SpectatorView.SV_Singleton<SV_Sharing>
{
    #region Public Fields

    [Tooltip("Allow send messages from Editor")]
    public bool debug;

    /// <summary>
    /// Message enum containing our information bytes to share.
    /// The first message type has to start with UserMessageIDStart
    /// so as not to conflict with HoloToolkit internal messages.
    /// </summary>
    public enum TestMessageID : byte
    {
        HeadTransform = MessageID.UserMessageIDStart,
        // Primitives
        Byte,
        Int,
        Long,
        Float,
        Double,
        String,
        Bool,

        Json,

        Transform,
        UserAvatar
    }

    public enum UserMessageChannels
    {
        Anchors = MessageChannel.UserMessageChannelStart,
    }

    [HideInInspector]
    public bool Initialized = false;

    public delegate void MessageCallback(NetworkInMessage msg);

    #endregion

    #region Private Fields

    /// <summary>
    /// Helper object that we use to route incoming message callbacks to the member
    /// functions of this class
    /// </summary>
    private NetworkConnectionAdapter connectionAdapter;

    /// <summary>
    /// Cache the connection object for the sharing service
    /// </summary>
    private NetworkConnection serverConnection;

    private SharingStage sharingStage;

    private Dictionary<TestMessageID, MessageCallback> _MessageHandlers = new Dictionary<TestMessageID, MessageCallback>();

    #endregion

    #region Public Properties

    /// <summary>
    /// Cache the local user's ID to use when sending messages
    /// </summary>
    public long localUserID
    {
        get; set;
    }

    public Dictionary<TestMessageID, MessageCallback> MessageHandlers
    {
        get
        {
            return _MessageHandlers;
        }
    }

    // is message can be sent?
    public bool CanBeSent
    {
        get
        {
            var editor = false;

#if UNITY_EDITOR
            editor = true;
#endif

            return !editor || debug
            && serverConnection != null
            && serverConnection.IsConnected();
        }
    }

    #endregion

    #region Private Properties

    /// <summary>
    /// Get first value of TestMessageID
    ///  <summary>
    private TestMessageID MinID
    {
        get
        {
            var enumVals = Enum.GetValues(typeof(TestMessageID));

            return (TestMessageID)enumVals.GetValue(0);
        }
    }

    /// <summary>
    /// Get last value of TestMessageID
    ///  <summary>
    private TestMessageID MaxID
    {
        get
        {
            var enumVals = Enum.GetValues(typeof(TestMessageID));

            return (TestMessageID)enumVals.GetValue(enumVals.Length - 1);
        }
    }

    #endregion

    #region Main Methods

    void Start()
    {
        InitializeMessageHandlers();
    }

    void Update()
    {
        // if local copy of sharing stage is not set
        if (sharingStage == null)
        {
            // we set it in this method
            InitializeMessageHandlers();
        }
    }

    #endregion

    #region Events

    void OnMessageReceived(NetworkConnection connection, NetworkInMessage msg)
    {
        // read bytes from message
        byte messageType = msg.ReadByte();
        // then convert bytes to TestMessageID and get relative to it MessageHandler
        MessageCallback messageHandler = MessageHandlers[(TestMessageID)messageType];

        // if message handler for this id is not null
        if (messageHandler != null)
        {
            messageHandler(msg);
        }
    }

    void OnDisable()
    {
        Initialized = false;
    }

    protected override void OnDestroy()
    {
        RemoveCallbacks();

        base.OnDestroy();
    }

    #endregion

    #region Utility Methods

    private NetworkOutMessage CreateMessage(byte MessageType)
    {
        NetworkOutMessage msg = serverConnection.CreateMessage(MessageType);
        msg.Write(MessageType);
        // Add the local userID so that the remote clients know whose message they are receiving
        msg.Write(localUserID);

        return msg;
    }

    // index all TestMessageID and create handlers for it
    private void InitializeMessageHandlers()
    {
        // save instance of sharing stage
        sharingStage = SharingStage.Instance;

        // if something goes wrong we stop here
        if (sharingStage == null
            || sharingStage.Manager == null)
        {
            return;
        }

        // setup connection and adapter
        serverConnection = sharingStage.Manager.GetServerConnection();
        connectionAdapter = new NetworkConnectionAdapter();

        // Cache the local user ID
        localUserID = SharingStage.Instance.Manager.GetLocalUser().GetID();

        AddCallbacks();

        Initialized = true;
    }

    public void AddCallbacks()
    {
        // index all TestMessageID
        for (byte index = (byte)MinID; index <= (byte)MaxID; index++)
        {
            // if MessageHandlers not contains this key
            if (MessageHandlers.ContainsKey((TestMessageID)index) == false)
            {
                // add new handler
                MessageHandlers.Add((TestMessageID)index, null);
            }

            // add listener for this handler
            serverConnection.AddListener(index, connectionAdapter);
        }

        // set callback
        connectionAdapter.MessageReceivedCallback += OnMessageReceived;
    }

    public void RemoveCallbacks()
    {
        if (serverConnection != null)
        {
            for (byte index = (byte)MinID; index <= (byte)MaxID; index++)
            {
                serverConnection.RemoveListener(index, connectionAdapter);
            }

            // remove callback
            connectionAdapter.MessageReceivedCallback -= OnMessageReceived;
        }
    }

    #endregion

    #region Sharing Methods

    #region Primitives
        
    private void SendPrimitive(NetworkOutMessage msg)
    {
        serverConnection.Broadcast(
            msg,
            MessagePriority.Immediate,
            MessageReliability.ReliableOrdered,
            MessageChannel.Avatar);
    }

    /// <summary>
    ///  <summary>Send Byte message</summary>
    /// </summary>
    /// <param name="value"></param>
    public void SendByte(byte value, string tag = "", bool sendAnyway = false)
    {
        if (sendAnyway || CanBeSent)
        {
            NetworkOutMessage msg = CreateMessage((byte)TestMessageID.Byte);

            AddVal(msg, tag); // write tag first

            AddVal(msg, value); // then write value

            SendPrimitive(msg);
        }
    }

    /// <summary>
    ///  <summary>Send Int message</summary>
    /// </summary>
    /// <param name="value"></param>
    public void SendInt(int value, string tag = "", bool sendAnyway = false)
    {
        if (sendAnyway || CanBeSent)
        {
            NetworkOutMessage msg = CreateMessage((byte)TestMessageID.Int);

            AddVal(msg, tag); // write tag first

            AddVal(msg, value); // then write value

            SendPrimitive(msg);
        }
    }

    /// <summary>
    ///  <summary>Send Int message</summary>
    /// </summary>
    /// <param name="value"></param>
    public void SendLong(long value, string tag = "", bool sendAnyway = false)
    {
        if (sendAnyway || CanBeSent)
        {
            NetworkOutMessage msg = CreateMessage((byte)TestMessageID.Long);

            AddVal(msg, tag); // write tag first

            AddVal(msg, value); // then write value

            SendPrimitive(msg);
        }
    }

    /// <summary>
    ///  <summary>Send Float message</summary>
    /// </summary>
    /// <param name="value"></param>
    public void SendFloat(float value, string tag = "", bool sendAnyway = false)
    {
        if (sendAnyway || CanBeSent)
        {
            NetworkOutMessage msg = CreateMessage((byte)TestMessageID.Float);

            AddVal(msg, tag); // write tag first

            AddVal(msg, value); // then write value

            SendPrimitive(msg);
        }
    }

    /// <summary>
    ///  <summary>Send Double message</summary>
    /// </summary>
    /// <param name="value"></param>
    public void SendDouble(double value, string tag = "", bool sendAnyway = false)
    {
        if (sendAnyway || CanBeSent)
        {
            NetworkOutMessage msg = CreateMessage((byte)TestMessageID.Double);

            AddVal(msg, tag); // write tag first

            AddVal(msg, value); // then write value

            SendPrimitive(msg);
        }
    }

    /// <summary>
    ///  <summary>Send String message</summary>
    /// </summary>
    /// <param name="value"></param>
    public void SendString(string value, string tag = "", bool sendAnyway = false)
    {
        if (sendAnyway || CanBeSent)
        {
            NetworkOutMessage msg = CreateMessage((byte)TestMessageID.String);

            AddVal(msg, tag); // write tag first

            AddVal(msg, value); // then write value

            SendPrimitive(msg);
        }
    }

    /// <summary>
    ///  <summary>Send String message</summary>
    /// </summary>
    /// <param name="value"></param>
    public void SendBool(bool value, string tag = "", bool sendAnyway = false)
    {
        if (sendAnyway || CanBeSent)
        {
            NetworkOutMessage msg = CreateMessage((byte)TestMessageID.Bool);

            AddVal(msg, tag); // write tag first

            AddVal(msg, value ? 1 : 0); // then write value

            SendPrimitive(msg);
        }
    }

    #region Universal Method

    /// <summary>
    ///  <summary>Universal sharing method for primitives</summary>
    /// </summary>
    /// <param name="value"></param>
    /// <param name="tag"></param>
    public void SendValue(byte value, string tag = "", bool sendAnyway = false)
    {
        SendByte(value, tag, sendAnyway);
    }
    // int
    public void SendValue(int value, string tag = "", bool sendAnyway = false)
    {
        SendInt(value, tag, sendAnyway);
    }
    // long
    public void SendValue(long value, string tag = "", bool sendAnyway = false)
    {
        SendLong(value, tag, sendAnyway);
    }
    // float
    public void SendValue(float value, string tag = "", bool sendAnyway = false)
    {
        SendFloat(value, tag, sendAnyway);
    }
    // double
    public void SendValue(double value, string tag = "", bool sendAnyway = false)
    {
        SendDouble(value, tag, sendAnyway);
    }
    // string
    public void SendValue(string value, string tag = "", bool sendAnyway = false)
    {
        SendString(value, tag, sendAnyway);
    }
    // bool
    public void SendValue(bool value, string tag = "", bool sendAnyway = false)
    {
        SendBool(value, tag, sendAnyway);
    }

    #endregion

    #endregion

    /// <summary>
    ///  <summary>Send Json message</summary>
    /// </summary>
    /// <param name="value"></param>
    public void SendJson(object value, string tag = "", bool sendAnyway = false)
    {
        if (sendAnyway || CanBeSent)
        {
            NetworkOutMessage msg = CreateMessage((byte)TestMessageID.Json);

            AddVal(msg, tag); // write tag first

            AddVal(msg, JsonUtility.ToJson(value)); // then write value

            SendPrimitive(msg);
        }
    }

    public void SendTransform(Vector3 position, Quaternion rotation, Vector3 scale, string tag = "", bool sendAnyway = false)
    {
        if (sendAnyway || CanBeSent)
        {
            NetworkOutMessage msg = CreateMessage((byte)TestMessageID.Transform);

            AddVal(msg, tag); // write tag first

            AddTransform(msg, position, rotation, scale); // then write value

            serverConnection.Broadcast(
                msg,
                MessagePriority.Immediate,
                MessageReliability.ReliableOrdered,
                MessageChannel.Avatar);
        }
    }

    public void SendHeadTransform(Vector3 position, Quaternion rotation, Vector3 scale, byte HasAnchor)
    {
        if (CanBeSent)
        {
            NetworkOutMessage msg = CreateMessage((byte)TestMessageID.HeadTransform);

            AddTransform(msg, position, rotation, scale);

            msg.Write(HasAnchor);
            
            serverConnection.Broadcast(
                msg,
                MessagePriority.Immediate,
                MessageReliability.UnreliableSequenced,
                MessageChannel.Avatar);
        }
    }

    public void SendUserAvatar(int UserAvatarID)
    {
        if (CanBeSent)
        {
            NetworkOutMessage msg = CreateMessage((byte)TestMessageID.UserAvatar);

            msg.Write(UserAvatarID);

            serverConnection.Broadcast(
                msg,
                MessagePriority.Medium,
                MessageReliability.Reliable,
                MessageChannel.Avatar);
        }
    }

    #endregion

    #region Writing Methods

    /// <summary>
    /// Write primitives to message
    /// Support data types: 
    //  byte, int, long, float, double, string
    /// <summary>
    /// <param name="msg"></param>
    /// <param name="value"></param>
    void AddVal(NetworkOutMessage msg, byte value)
    {
        msg.Write(value);
    }
    // int
    void AddVal(NetworkOutMessage msg, int value)
    {
        msg.Write(value);
    }
    // long
    void AddVal(NetworkOutMessage msg, long value)
    {
        msg.Write(value);
    }
    // float
    void AddVal(NetworkOutMessage msg, float value)
    {
        msg.Write(value);
    }
    // double
    void AddVal(NetworkOutMessage msg, double value)
    {
        msg.Write(value);
    }
    // string
    void AddVal(NetworkOutMessage msg, string value)
    {
        byte[] strBytes = System.Text.Encoding.ASCII.GetBytes(value);
        long byteSize = (long)strBytes.Length;
        msg.Write(byteSize);
        msg.WriteArray(strBytes, (uint)byteSize);
    }

    /// <summary>
    /// Add transform to message
    /// <summary>
    /// <param name="msg"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <param name="scale"></param>
    void AddTransform(NetworkOutMessage msg, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        AddVector3(msg, position);
        AddQuaternion(msg, rotation);
        AddVector3(msg, scale);
    }

    /// <summary>
    /// Add Vector3 to message
    /// <summary>
    /// <param name="msg"></param>
    /// <param name="vector"></param>
    void AddVector3(NetworkOutMessage msg, Vector3 vector)
    {
        msg.Write(vector.x);
        msg.Write(vector.y);
        msg.Write(vector.z);
    }

    /// <summary>
    /// Add Quaternion to message
    ///  <summary>
    /// <param name="msg"></param>
    /// <param name="rotation"></param>
    void AddQuaternion(NetworkOutMessage msg, Quaternion rotation)
    {
        msg.Write(rotation.x);
        msg.Write(rotation.y);
        msg.Write(rotation.z);
        msg.Write(rotation.w);
    }

    #endregion

    #region Reading Methods

    // id and tag
    private void ReadIdAndTag(NetworkInMessage msg, bool onlyValue, out long userId, out string tag)
    {
        userId = 0;
        tag = "";

        if (!onlyValue)
        {
            userId = msg.ReadInt64(); // firstly read user id

            tag = GetString(msg); // secondly read message tag
        }
    }

    /// <summary>
    /// <summary>Read Byte from message. Don't forget read user id first (long)</summary>
    /// </summary>
    /// <param name="msg"></param>
    /// <returns></returns>
    public SharingData ReadByte(NetworkInMessage msg, bool onlyValue = false)
    {
        long userId = 0;
        string tag = "";
        // firstly we read user_id and message_tag if onlyValue == false
        ReadIdAndTag(msg, onlyValue, out userId, out tag); 

        var value = msg.ReadByte(); // lastly we read value

        return new SharingData(userId, tag, value);
    }

    /// <summary>
    /// <summary>Read Int from message. Don't forget read user id first (long)</summary>
    /// </summary>
    /// <param name="msg"></param>
    /// <returns></returns>
    public SharingData ReadInt(NetworkInMessage msg, bool onlyValue = false)
    {
        long userId = 0;
        string tag = "";
        // firstly we read user_id and message_tag if onlyValue == false
        ReadIdAndTag(msg, onlyValue, out userId, out tag);

        var value = msg.ReadInt32(); // lastly we read value

        return new SharingData(userId, tag, value);
    }

    /// <summary>
    /// <summary>Read Long from message. Don't forget read user id first (long)</summary>
    /// </summary>
    /// <param name="msg"></param>
    /// <returns></returns>
    public SharingData ReadLong(NetworkInMessage msg, bool onlyValue = false)
    {
        long userId = 0;
        string tag = "";
        // firstly we read user_id and message tag if onlyValue == false
        ReadIdAndTag(msg, onlyValue, out userId, out tag); 

        var value = msg.ReadInt64(); // lastly we read value

        return new SharingData(userId, tag, value);
    }

    /// <summary>
    /// <summary>Read Float from message. Don't forget read user id first (long)</summary>
    /// </summary>
    /// <param name="msg"></param>
    /// <returns></returns>
    public SharingData ReadFloat(NetworkInMessage msg, bool onlyValue = false)
    {
        long userId = 0;
        string tag = "";
        // firstly we read user_id and message tag if onlyValue == false
        ReadIdAndTag(msg, onlyValue, out userId, out tag);

        var value = msg.ReadFloat(); // lastly we read value

        return new SharingData(userId, tag, value);
    }

    /// <summary>
    /// <summary>Read Double from message. Don't forget read user id first (long)</summary>
    /// </summary>
    /// <param name="msg"></param>
    /// <returns></returns>
    public SharingData ReadDouble(NetworkInMessage msg, bool onlyValue = false)
    {
        long userId = 0;
        string tag = "";
        // firstly we read user_id and message tag if onlyValue == false
        ReadIdAndTag(msg, onlyValue, out userId, out tag);

        var value = msg.ReadDouble(); // lastly we read value

        return new SharingData(userId, tag, value);
    }

    public SharingData ReadBool(NetworkInMessage msg, bool onlyValue = false)
    {
        var data = ReadInt(msg, onlyValue);

        return new SharingData(data.userId, data.tag, data.intValue > 0 ? true : false);
    }

    // private method for reading string
    private string GetString(NetworkInMessage msg)
    {
        long strSize = msg.ReadInt64();
        byte[] strData = new byte[(uint)strSize];
        msg.ReadArray(strData, (uint)strSize);

        return System.Text.Encoding.ASCII.GetString(strData);
    }

    /// <summary>
    /// <summary>Read String from message. Don't forget read user id first (long)</summary>
    /// </summary>
    /// <param name="msg"></param>
    /// <returns></returns>
    public SharingData ReadString(NetworkInMessage msg, bool onlyValue = false)
    {
        long userId = 0;
        string tag = "";
        // firstly we read user_id and message tag if onlyValue == false
        ReadIdAndTag(msg, onlyValue, out userId, out tag);

        var value = GetString(msg); // lastly we read value

        return new SharingData(userId, tag, value);
    }

    // read Vector3 as 3 floats
    private Vector3 GetVector3(NetworkInMessage msg)
    {
        return new Vector3(msg.ReadFloat(), msg.ReadFloat(), msg.ReadFloat());
    }

    public SharingData ReadVector3(NetworkInMessage msg, bool onlyValue = false)
    {
        long userId = 0;
        string tag = "";
        // firstly we read user_id and message tag if onlyValue == false
        ReadIdAndTag(msg, onlyValue, out userId, out tag);

        var value = GetVector3(msg); // lastly we read value

        return new SharingData(userId, tag, value);
    }

    // read Quaternion as 4 floats
    private Quaternion GetQuaternion(NetworkInMessage msg)
    {
        return new Quaternion(msg.ReadFloat(), msg.ReadFloat(), msg.ReadFloat(), msg.ReadFloat());
    }

    public SharingData ReadQuaternion(NetworkInMessage msg, bool onlyValue = false)
    {
        long userId = 0;
        string tag = "";
        // firstly we read user_id and message tag if onlyValue == false
        ReadIdAndTag(msg, onlyValue, out userId, out tag); 

        var value = GetQuaternion(msg); // lastly we read value

        return new SharingData(userId, tag, value);
    }

    #endregion

    #region Nester Classes

    [Serializable]
    public class SharingData
    {
        #region Public Fields

        public long userId;
        public string tag;

        public byte byteValue;
        public int intValue;
        public long longValue;
        public float floatValue;
        public double doubleValue;
        public bool boolValue;
        public string stringValue;

        public Vector3 vector3Value = Vector3.zero;
        public Quaternion quaternionValue = Quaternion.identity;

        #endregion

        #region Constructors

        // byte
        public SharingData(long userId, string tag, byte byteValue = 0)
        {
            this.userId = userId;
            this.tag = tag;

            this.byteValue = byteValue;
        }
        // int
        public SharingData(long userId, string tag, int intValue = 0)
        {
            this.userId = userId;
            this.tag = tag;

            this.intValue = intValue;
        }
        // long
        public SharingData(long userId, string tag, long longValue = 0)
        {
            this.userId = userId;
            this.tag = tag;

            this.longValue = longValue;
        }
        // float
        public SharingData(long userId, string tag, float floatValue = 0)
        {
            this.userId = userId;
            this.tag = tag;

            this.floatValue = floatValue;
        }
        // double
        public SharingData(long userId, string tag, double doubleValue = 0)
        {
            this.userId = userId;
            this.tag = tag;

            this.doubleValue = doubleValue;
        }
        // bool
        public SharingData(long userId, string tag, bool boolValue = false)
        {
            this.userId = userId;
            this.tag = tag;

            this.boolValue = boolValue;
        }
        // string
        public SharingData(long userId, string tag, string stringValue = "")
        {
            this.userId = userId;
            this.tag = tag;

            this.stringValue = stringValue;
        }
        // vector 3
        public SharingData(long userId, string tag, Vector3 vector3Value)
        {
            this.userId = userId;
            this.tag = tag;

            this.vector3Value = vector3Value;
        }

        // vector 3
        public SharingData(long userId, string tag, Quaternion quaternionValue)
        {
            this.userId = userId;
            this.tag = tag;

            this.quaternionValue = quaternionValue;
        }

        #endregion
    }

    #endregion
}