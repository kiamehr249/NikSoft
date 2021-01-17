using NikSoft.NikModel;
using System.Collections.Generic;

namespace NikSoft.UILayer
{
    public interface INotification
    {
        void SetErrorMessage(string msg);

        void SetSuccessMessage(string msg);

        void SetOptionalMessage(string msg, MessageType mtype, Layout mlayout, int timeout = 0);

        void SetErrorMessage(List<string> msg);

        void SetSuccessMessage(List<string> msg);

        void SetOptionalMessage(List<string> msg, MessageType mtype, Layout mlayout, int timeout = 0);
    }
}