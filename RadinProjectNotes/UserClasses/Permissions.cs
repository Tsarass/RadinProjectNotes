using ProtoBuf;
using System;

namespace RadinProjectNotes
{
    [Flags,ProtoContract]
    public enum Permissions
    {
        Admin = 1 << 0,
        Read = 1 << 1,
        Edit = 1 << 2,
        Delete = 1 << 3,
        AddComment = 1 << 4,
        EditProjectServices = 1 << 5
    }
}
