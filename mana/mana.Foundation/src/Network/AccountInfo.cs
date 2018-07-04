using System;

namespace mana.Foundation
{
    public sealed class AccountInfo : DataObject
    {
        public string username;

        public string password;

        public void Decode(IReadableBuffer br)
        {
            username = br.ReadUTF8();
            password = br.ReadUTF8();
        }

        public void Encode(IWritableBuffer bw)
        {
            bw.WriteUTF8(username);
            bw.WriteUTF8(password);
        }

        public void ReleaseToCache()
        {
            ObjectCache.Put(this);
        }

        public string ToFormatString(string nlIndent)
        {
            return string.Format("AccountInfo:[{0}:{1}]", username, password);
        }
    }
}
