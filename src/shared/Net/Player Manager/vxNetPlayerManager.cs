using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VerticesEngine;


namespace VerticesEngine.Net
{
    public class vxNetPlayerManager
    {
        /// <summary>
        /// An entity collection containing all Network Players
        /// </summary>
        public Dictionary<string, vxNetPlayerInfo> Players = new Dictionary<string, vxNetPlayerInfo>();

        public vxNetPlayerManager()
        {

        }

        public vxNetPlayerInfo this[string id]
        {
            get {
                if(Players.TryGetValue(id, out var info))
                {
                    return info;
                }
                else
                {
                    vxConsole.WriteError("Missing Player Id " + id);

                    var nullinfo = new vxNetPlayerInfo();
                    nullinfo.EntityState = new vxNetEntityState();
                    nullinfo.ID = string.Empty;
                    nullinfo.UserName = string.Empty;
                    nullinfo.PlayerIndex = -1;
                    nullinfo.Status = vxEnumNetPlayerStatus.None;
                    nullinfo.EntityState = new vxNetEntityState();
                    nullinfo.Platform = vxPlatformType.Steam;
                    nullinfo.PlatformPlayerID = "";
                    return nullinfo;
                }
            }
            set
            {
                if(Players.ContainsKey(id))
                    Players[id] = value;
            }
        }

        public bool TryGetPlayerInfo(string key, out vxNetPlayerInfo info)
        {
            return Players.TryGetValue(key, out info);
        }

        public void Add(vxNetPlayerInfo entity)
        {
            if(Players.ContainsKey(entity.ID)==false)
                Players.Add(entity.ID, entity);
        }

        public bool Contains(vxNetPlayerInfo entity)
        {
            return Players.ContainsKey(entity.ID);
        }
    }
}
