using DatabaseProvider;
using DBIO;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebsiteDocTruyenChu.Helpers;

namespace WebsiteDocTruyenChu
{
    public class ChatHub : Hub
    {
        MyDB myDB = new MyDB();
        public void Send(int userID, string name, string message, int roomType, int? roomID)
        {
            // global chanel
            System.Diagnostics.Debug.WriteLine(string.Format("[{0}] {1}: {2} - {3}", userID, name, message, roomType));
            var existedUser = myDB.GetUserByUserID(userID);
            if (roomType == StaticVariables.TYPE_MESSAGE_GLOBAL && existedUser != null && existedUser.fullname == name)
            {
                var globalRoom = myDB.GetRooms().Where(r => r.type == roomType).FirstOrDefault();
                myDB.AddRecord(new Message()
                {
                    userid = existedUser.uid,
                    roomID = globalRoom.roomID,
                    content = message,
                    createdAt = DateTime.Now,
                    updatedAt = DateTime.Now,
                });
                myDB.SaveChanges();
                Clients.All.addNewGlobalMessage(name, message, DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
            }
        }
    }
}